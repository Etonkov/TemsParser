using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

using TemsParser.CustomAttributes;
using TemsParser.Behaviors;
using TemsParser.Processing;
using TemsParser.Common;
using TemsParser.Extentions;
using TemsParser.IO;
using TemsParser.Models.Config;
using TemsParser.Models.Settings;
using TemsParser.Models.Parsing;
using TemsParser.Models.TemsFileInfo;
using TemsParser.Messages;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TemsParser.Models.Parsing.Feedback;
using TemsParser.Extentions.Model.Config;
using TemsParser.Models.Parsing.Repository;
using TemsParser.Models.Parsing.ThreadHelpers;
using TemsParser.Models.Parsing.Comparison;

namespace TemsParser.ViewModels
{
    public class ParsingViewModel : ViewModelBase
    {
        private readonly Object LockObj = new Object();
        private readonly CancellationToken CancelToken;
        private readonly CancellationTokenSource CancelTokenSource;
        private readonly Thread Parsing;
        private int _progressValue;
        private string _phase;
        private bool _isParsing;
        private long _currentRow;
        private long _progressStep;
        //private ThreadCounter _counter;
        private bool _isParsingFailed;
        public readonly ConfigModel FilteredConfig;
        public readonly ConfigModel Config;
        public readonly SettingsModel Settings;


        public ParsingViewModel(ConfigModel config, SettingsModel settings)
        {
            CancelTokenSource = new CancellationTokenSource();
            CancelToken = CancelTokenSource.Token;
            _isParsingFailed = false;
            ProgressValue = 0;
            ProgressMinimum = 0;
            ProgressMaximum = 100;
            Feedback = new ObservableCollection<FeedbackItemModel>();
            AbortCommand = new Command(ex => Abort(), ce => IsParsing);
            ExportCommand = new AsyncCommand(ex => Export(), ce => !IsParsing);

            CancelCommand = new Command(ex =>
            {
                if (IsParsing)
                {
                    Abort();
                }
                else
                {
                    Close();
                }
            });

            Config = config;
            Settings = settings;
            FilteredConfig = config.GetFiltered(settings);

            Directory = Path.GetDirectoryName(Settings.OpenFiles.FirstOrDefault()) + @"\";

            Parsing = new Thread(Parse);
            Parsing.IsBackground = true;
            Parsing.Start();
        }

        public ICommand ExportCommand { get; private set; }

        public ICommand AbortCommand { get; private set; }

        public bool IsParsing
        {
            get
            {
                return _isParsing;
            }
            private set
            {
                if (_isParsing != value)
                {
                    _isParsing = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }


        public int ProgressMinimum { get; private set; }

        public int ProgressMaximum { get; private set; }


        public int ProgressValue
        {
            get
            {
                return _progressValue;
            }
            private set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Phase
        {
            get
            {
                return _phase;
            }
            private set
            {
                if (_phase != value)
                {
                    _phase = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Directory { get; private set; }

        public ObservableCollection<FeedbackItemModel> Feedback { get; private set; }


        private void RowParsed(object s, EventArgs e)
        {
            lock (LockObj)
            {
                _currentRow++;

                if (_currentRow % _progressStep == 0)
                {
                    ProgressValue++;
                }
            }
        }

        private void Parse()
        {
            IsParsing = true;

            var files = Settings.OpenFiles;
            var readers = new List<TemsFileReader>();
            var technologiesList = FilteredConfig.TechnologiesList;
            Phase = "Оценка объёма работ...";

            foreach (var filePath in files)
            {
                TemsFileReader reader = default(TemsFileReader);
                var fileName = StringParser.GetFileName(filePath);

                try
                {
                    reader = new TemsFileReader(filePath, fileName, technologiesList);

                    if (reader.BodyRowCount > 0)
                    {
                        string message;

                        if (reader.HeaderInfo.ColumnInfoList.Count() > 1)
                        {
                            var techs = String.Join(",", reader.HeaderInfo.ColumnInfoList.Select(ci => ci.TechnologyItem));
                            message = String.Format("Определены технологии: {0}.", techs);
                        }
                        else
                        {
                            message = String.Format("Определена технология: {0}.",
                                                    reader.HeaderInfo.ColumnInfoList.FirstOrDefault().TechnologyItem);
                        }

                        var feedback = new FeedbackItemModel(FeedbackStatus.OK, fileName, message);
                        AddToFeedback(feedback);
                        readers.Add(reader);
                    }
                    else
                    {
                        var message = "Файл не содержит данных для обработки.";
                        var feedback = new FeedbackItemModel(FeedbackStatus.Warning, fileName, message);
                        AddToFeedback(feedback);
                        reader.Dispose();
                    }
                }
                catch (Exception e)
                {
                    var message = String.Format("Ошибка чтения параметров файла: {0}", e.Message);
                    var feedback = new FeedbackItemModel(FeedbackStatus.Error, fileName, message);
                    AddToFeedback(feedback);

                    if (reader != null)
                    {
                        reader.Dispose();
                    }
                }

                if (CancelToken.IsCancellationRequested)
                {
                    if (reader != null)
                    {
                        reader.Dispose();
                    }

                    return;
                }
            }

            readers = readers.OrderBy(fr => fr.BodyRowCount).ToList();
            long totalRows = readers.Select(fr => fr.BodyRowCount).Sum();

            if (totalRows > 100)
            {
                ProgressMaximum = 100;
                _progressStep = totalRows / ProgressMaximum;
            }
            else
            {
                _progressStep = 1;
                ProgressMaximum = (int)totalRows;
            }

            var operList = FilteredConfig.OperatorsList;

            foreach (var readerItem in readers)
            {
                // Prepare data.
                Phase = String.Format("Парсинг файла {0}...", readerItem.FileName);

                using (var parser = new TemsFileParser(readerItem, Settings))
                {
                    IEnumerable<TechnologyListItemModel> techList =
                        readerItem.HeaderInfo.ColumnInfoList.Select(ci => ci.TechnologyItem).ToList();

                    var directoryBase = Directory + readerItem.FileName.Split('.')[0];
                    var repositoryManager = new RepositoryManager(Settings, techList, operList, directoryBase);
                    var repository = repositoryManager.GetRepository();

                    // Subscribe to events.
                    parser.BestLevelFoundEvent += repository.AddValue;

                    // If comparison needed.
                    if (Settings.CompareOperatorsEnabled && (operList.Count > 1))
                    {
                        var comparator = new Comparator(techList, operList, directoryBase);
                        parser.BestLevelFoundEvent += comparator.AddValue;
                        parser.Finished += (s, ea) =>
                        {
                            var fileCount = comparator.Save();

                            if (fileCount > 0)
                            {
                                var message =
                                    String.Format("Результаты сравнения сохранены. Файлов: {0}", fileCount);

                                var feedback = new FeedbackItemModel(FeedbackStatus.OK, readerItem.FileName, message);
                                AddToFeedback(feedback);
                            }
                        };
                    }

                    parser.RowParsed += RowParsed;

                    // Parsing.
                    try
                    {
                        parser.Parse(CancelToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (Exception e)
                    {
                        string message = String.Format("Ошибка парсинга файла: {0}", e.Message);
                        var feedback = new FeedbackItemModel(FeedbackStatus.Error, readerItem.FileName, message);
                        AddToFeedback(feedback);
                        continue;
                    }

                    try
                    {
                        var fileCount = repository.Save();

                        if (fileCount > 0)
                        {
                            var message =
                                String.Format("Результаты парсинга сохранены. Файлов: {0}", fileCount);

                            var feedback = new FeedbackItemModel(FeedbackStatus.OK, readerItem.FileName, message);
                            AddToFeedback(feedback);
                        }
                        else
                        {
                            var message = "Нет данных для сохранения.";
                            var feedback = new FeedbackItemModel(FeedbackStatus.Warning, readerItem.FileName, message);
                            AddToFeedback(feedback);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = String.Format("Ошибка сохранения результатов: {0}", e.Message);
                        var feedback = new FeedbackItemModel(FeedbackStatus.Error, readerItem.FileName, message);
                        AddToFeedback(feedback);
                    }

                    if (CancelToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }

            // Finish parsing.
            IsParsing = false;

            if (_isParsingFailed)
            {
                Phase = "Ошибка!";
                Alarms.ShowError("Парсинг выполнен", "Парсинг выполнен с ошибками.");
            }
            else
            {
                Phase = "Выполнено!";
                Alarms.ShowInfo("Парсинг выполнен", "Парсинг выполнен успешно.");
            }

            ProgressValue = 0;
        }

        private void Abort()
        {
            string message = "Текущая операция будет прервана. Продолжить?";

            if (Alarms.ShowQuestion("Отмена операции", message))
            {
                if (Parsing.IsAlive)
                {
                    Parsing.Abort();
                }

                Close();

                CancelTokenSource.Cancel();
            }
        }

        private void AddToFeedback(FeedbackItemModel feedbackItem)
        {
            if (feedbackItem.Status == FeedbackStatus.Error)
            {
                _isParsingFailed = true;
            }

            Application.Current.Dispatcher.Invoke(() => Feedback.Add(feedbackItem));
        }

        private async Task Export()
        {

            var header = "Статус\tФайл\tСообщение\tВремя";
            var body = String.Join("\n", Feedback);
            await FileDialog.SaveTxtAsync(String.Join("\n", header, body));
        }
    }
}
