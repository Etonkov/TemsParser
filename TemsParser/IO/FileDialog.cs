using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using TemsParser.Messages;
using TemsParser.Models.Config;
using TemsParser.Extentions;
using TemsParser.Extentions.Model.Config;

namespace TemsParser.IO
{
    public class FileDialog
    {
        private FileDialog() { }


        public static async Task<ImportConfigResult> ImportConfigXmlAsync()
        {
            bool result = false;
            var config = default(ConfigModel);
            var ofd = new OpenFileDialog();
            ofd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                result = true;

                await Task.Run(() =>
                {
                    var serializer = new XmlSerializer(typeof(ConfigModel));
                    var warnings = new List<string>();

                    serializer.UnknownAttribute += (s, e) =>
                    {
                        var message =
                            String.Format("[строка:{0};позиция:{1}] Неизвестный аттрибут:{2}",
                                          e.LineNumber,
                                          e.LinePosition,
                                          e.Attr.Name);

                        warnings.Add(message);
                    };

                    serializer.UnknownElement += (s, e) =>
                    {
                        var message =
                            String.Format("[строка:{0};позиция:{1}] Неизвестный элемент:{2}",
                                          e.LineNumber,
                                          e.LinePosition,
                                          e.Element.Name);

                        warnings.Add(message);
                    };

                    using (var fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        XmlReader reader = XmlReader.Create(fs);

                        try
                        {
                            config = (ConfigModel)serializer.Deserialize(reader);
                        }
                        catch (InvalidOperationException)
                        {
                            Alarms.ShowError("Ошибка импорта конфигурации XML", "Проверьте корректность файла.");
                            result = false;
                        }
                    }

                    if (warnings.Count > 0)
                    {
                        var title = "Неизвестные обекты XML";
                        var header = String.Format("В файле {0} обнаружены неизвестные объекты XML:", ofd.FileName);
                        Alarms.ShowWarning(title, header, warnings);
                    }
                });

                if (result)
                {
                    config.OnXmlDeserialized();
                    ICollection<ValidationResult> validationResults;

                    if (config.Validate(out validationResults) == false)
                    {
                        var messages = validationResults.Select(o => o.ErrorMessage).Distinct();
                        var title = "Ошибка валидации";
                        var header = String.Format("В файле {0} обнаружены следующие ошибки:", ofd.FileName);
                        result = false;
                        Alarms.ShowError(title, header, messages);
                    }
                }
            }

            return new ImportConfigResult(result, config);
        }

        public static async Task ExportConfigXmlAsync(ConfigModel config)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (sfd.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    var serializer = new XmlSerializer(typeof(ConfigModel));

                    using (var fs = new FileStream(sfd.FileName, System.IO.FileMode.Create))
                    {
                        try
                        {
                            serializer.Serialize(fs, config);
                        }
                        catch (Exception e)
                        {
                            Alarms.ShowError("Ошибка сохранения файла", e.ToString());
                        }
                    }
                });
            }
        }

        public static IEnumerable<string> SelectTxtFiles()
        {
            var result = new List<string>();

            var ofd = new OpenFileDialog() 
            {
                RestoreDirectory = true,
                Multiselect = true,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == true)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    result.Add(fileName);
                }
            }

            return result;
        }

        public static async Task SaveTxtAsync(string text)
        {
            var sfd = new SaveFileDialog()
            {
                RestoreDirectory = true,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (sfd.ShowDialog() == true)
            {
                await Task.Run(() =>
                    {
                        try
                        {
                            File.WriteAllText(sfd.FileName, text);
                        }
                        catch (Exception e)
                        {
                            Alarms.ShowError("Ошибка сохранения файла", e.ToString());
                        }
                    });
            }
        }
    }
}
