using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Settings;
using TemsParser.Extentions;
using System.Diagnostics;
using System.Threading;
using TemsParser.Extentions.Model.Config;

namespace TemsParser.Tests
{
    [TestClass]
    public class ConfigExtentionsTests
    {
        [TestMethod]
        public void ConfigExtentions_GetFiltered()
        {
            // Arrange.
            // Prepare ConfigModel and SettingsModel instance to test.

            #region arrange

            SettingsModel settings = new SettingsModel();
            settings.SelectedRegion = "Брянск";
            settings.SelectedTechnologies = new HashSet<string>() { "2G", "3G" };
            settings.SelectedOperators = new HashSet<string>() { "МТС", "Tele2" };


            var br = new RegionListItemModel() { Name = "Брянск" };
            var vl = new RegionListItemModel() { Name = "Владимир" };
            var ka = new RegionListItemModel() { Name = "Калуга" };
            var ku = new RegionListItemModel() { Name = "Курск" };
            var nn = new RegionListItemModel() { Name = "Н.Новгород" };
            var or = new RegionListItemModel() { Name = "Орёл" };
            var ry = new RegionListItemModel() { Name = "Рязань" };
            var tu = new RegionListItemModel() { Name = "Тула" };

            var g2 = new TechnologyListItemModel()
            {
                Name = "2G",
                LatitudeColumnName = "All-Latitude",
                LongitudeColumnName = "All-Longitude",
                FreqColumnNamePart = "All-Scanned ARFCN",
                LevelColumnNamePart = "All-Scanned RxLev (dBm)"
            };
            var g3 = new TechnologyListItemModel()
            {
                Name = "3G",
                LatitudeColumnName = "All-Latitude",
                LongitudeColumnName = "All-Longitude",
                FreqColumnNamePart = "All-Sc Best UARFCN",
                LevelColumnNamePart = "All-Sc Best Aggr Ec (dBm)"
            };
            var g4 = new TechnologyListItemModel()
            {
                Name = "4G",
                LatitudeColumnName = "All-Latitude",
                LongitudeColumnName = "All-Longitude",
                FreqColumnNamePart = "All-Sc Best EARFCN",
                LevelColumnNamePart = "All-Sc Best RSRP (dBm)"
            };

            var megafon = new OperatorListItemModel() { Name = "МегаФон" };
            var mts = new OperatorListItemModel() { Name = "МТС" };
            var beeline = new OperatorListItemModel() { Name = "Билайн" };
            var tele2 = new OperatorListItemModel() { Name = "Tele2" };

            ConfigModel config = new ConfigModel()
            {
                RegionsList = new ObservableCollection<RegionListItemModel>()
                {
                    br,
                    vl,
                    ka,
                    ku,
                    nn,
                    or,
                    ry,
                    tu
                },
                TechnologiesList = new ObservableCollection<TechnologyListItemModel>()
                {
                    g2,
                    g3,
                    g4
                },
                OperatorsList = new ObservableCollection<OperatorListItemModel>()
                {
                    megafon,
                    mts,
                    beeline,
                    tele2
                },
            };

            var opers = new ObservableCollection<OperatorModel>()
            {
                new OperatorModel()
                {
                     Config = config,
                     OperatorListItem = megafon,
                     Freqs = new ObservableCollection<FreqModel>()
                     {
                         new FreqModel()
                         {
                             Config = config,
                             Spectrum = "10-124,1022,1024"
                         }
                     }
                },
                new OperatorModel()
                {
                     Config = config,
                     OperatorListItem = mts,
                     Freqs = new ObservableCollection<FreqModel>()
                     {
                         new FreqModel()
                         {
                             Config = config,
                             Spectrum = "10-124,1022,1024"
                         }
                     }
                },
                new OperatorModel()
                {
                     Config = config,
                     OperatorListItem = beeline,
                     Freqs = new ObservableCollection<FreqModel>()
                     {
                         new FreqModel()
                         {
                             Config = config,
                             Spectrum = "10-124,1022,1024"
                         }
                     }
                },
                new OperatorModel()
                {
                     Config = config,
                     OperatorListItem = tele2,
                     Freqs = new ObservableCollection<FreqModel>()
                     {
                         //new FreqModel()
                         //{
                         //    Config = config,
                         //    Spectrum = "10-124,1022,1024"
                         //}
                     }
                }
            };

            var techs = new ObservableCollection<TechnologyModel>()
            {
                new TechnologyModel()
                {
                    Config = config,
                    TechnologyListItem = g2,
                    Operators = opers
                },
                new TechnologyModel()
                {
                    Config = config,
                    TechnologyListItem = g3,
                    //Operators = opers
                },
                new TechnologyModel()
                {
                    Config = config,
                    TechnologyListItem = g4,
                    Operators = opers
                },
            };


            config.Regions = new ObservableCollection<RegionModel>()
            {
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = br,
                    Technologies = techs
                },
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = vl,
                    Technologies = techs
                },
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = ka,
                    Technologies = techs
                },
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = ku,
                    Technologies = techs
                },
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = nn,
                    Technologies = techs
                },
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = or,
                    Technologies = techs
                },
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = ry,
                    Technologies = techs
                },
                new RegionModel()
                {
                    Config = config,
                    RegionListItem = tu,
                    Technologies = techs
                }
            };

            #endregion


            // Act
            var filteredConfig = config.GetFiltered(settings);
            var filteredRegions = filteredConfig.Regions;
            var filteredTechnologies = filteredRegions.SelectMany(o => o.Technologies);
            var filteredOperators = filteredTechnologies.SelectMany(o => o.Operators);

            var filteredRegionsList = filteredConfig.RegionsList;
            var filteredTechnologiesList = filteredConfig.TechnologiesList;
            var filteredOperatorsList = filteredConfig.OperatorsList;

            // Assert.
            Assert.AreEqual(1, filteredRegions.Count());
            Assert.IsNotNull(filteredRegions.FirstOrDefault(o => (o.ToString() == settings.SelectedRegion)));

            Assert.AreEqual(1, filteredTechnologies.Count());
            Assert.IsNotNull(filteredTechnologies.FirstOrDefault(o => (o.ToString() == "2G")));
            Assert.IsNull(filteredTechnologies.FirstOrDefault(o => (o.ToString() == "3G")));
            Assert.IsNull(filteredTechnologies.FirstOrDefault(o => (o.ToString() == "4G")));


            Assert.AreEqual(1, filteredOperators.Count());
            Assert.IsNotNull(filteredOperators.FirstOrDefault(o => (o.ToString() == "МТС")));
            Assert.IsNull(filteredOperators.FirstOrDefault(o => (o.ToString() == "Tele2")));
            Assert.IsNull(filteredOperators.FirstOrDefault(o => (o.ToString() == "МегаФон")));
            Assert.IsNull(filteredOperators.FirstOrDefault(o => (o.ToString() == "Билайн")));

            Assert.AreEqual(1, filteredRegionsList.Count());

            foreach (var filteredRegionListItem in filteredRegionsList)
            {
                Assert.AreEqual(settings.SelectedRegion, filteredRegionListItem.ToString());
                Assert.AreEqual(1, filteredRegionListItem.Regions.Count);

                foreach (var regionItem in filteredRegionListItem.Regions)
                {
                    Assert.AreEqual(filteredRegionListItem, regionItem.RegionListItem);
                }
            }

            Assert.AreEqual(1, filteredTechnologiesList.Count());

            foreach (var filteredTechnologyListItem in filteredTechnologiesList)
            {
                var correctValues = new List<string>() { "2G" };
                Assert.IsTrue(correctValues.Contains(filteredTechnologyListItem.ToString()));
                Assert.AreEqual(1, filteredTechnologyListItem.Technologies.Count);

                foreach (var technologyItem in filteredTechnologyListItem.Technologies)
                {
                    Assert.AreEqual(filteredTechnologyListItem, technologyItem.TechnologyListItem);
                }
            }

            Assert.AreEqual(1, filteredOperatorsList.Count());

            foreach (var filteredOperatorListItem in filteredOperatorsList)
            {
                var correctValues = new List<string>() { "МТС" };
                Assert.IsTrue(correctValues.Contains(filteredOperatorListItem.ToString()));
                Assert.AreEqual(1, filteredOperatorListItem.Operators.Count);

                foreach (var operatorItem in filteredOperatorListItem.Operators)
                {
                    Assert.AreEqual(filteredOperatorListItem, operatorItem.OperatorListItem);
                }
            }
        }
    }
}
