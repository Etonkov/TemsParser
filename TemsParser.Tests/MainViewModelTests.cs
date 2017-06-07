using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemsParser.ViewModels;
using TemsParser.Models.Config;
using TemsParser.Models.Settings;


namespace TemsParser.Tests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void MainViewModel_ClearSettings()
        {
            // Arrange.
            var configModel = new ConfigModel()
            {
                OperatorsList = new System.Collections.ObjectModel.ObservableCollection<OperatorListItemModel>()
                {
                    new OperatorListItemModel()
                    {
                        Name = "MTS"
                    },
                    new OperatorListItemModel()
                    {
                        Name = "MegaFon"
                    }
                },
                TechnologiesList = new System.Collections.ObjectModel.ObservableCollection<TechnologyListItemModel>()
                {
                    new TechnologyListItemModel()
                    {
                        Name = "2G"
                    },
                    new TechnologyListItemModel()
                    {
                        Name = "3G"
                    },
                }
            };

            var configModel2 = new ConfigModel()
            {
                OperatorsList = new System.Collections.ObjectModel.ObservableCollection<OperatorListItemModel>()
                {
                    new OperatorListItemModel()
                    {
                        Name = "MTS"
                    }
                },
                TechnologiesList = new System.Collections.ObjectModel.ObservableCollection<TechnologyListItemModel>()
                {
                    new TechnologyListItemModel()
                    {
                        Name = "2G"
                    }
                }
            };

            var settingsModel = new SettingsModel()
            {
                SelectedOperators = new HashSet<string>()
                {
                    "MTS",
                    "MegaFon",
                    "MustRemoved"
                },
                SelectedTechnologies = new HashSet<string>()
                {
                    "2G",
                    "3G",
                    "MustRemoved"
                }
            };

            var vm = new MainViewModel(configModel, settingsModel);


            // Act + assert.
            Assert.IsFalse(vm.Settings.SelectedTechnologies.Contains("MustRemoved"));
            Assert.IsFalse(vm.Settings.SelectedOperators.Contains("MustRemoved"));
        }
    }
}
