using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Extentions;
using System.Diagnostics;
using System.Threading;
using TemsParser.Extentions.Model.Config;

namespace TemsParser.Tests
{
    [TestClass]
    public class CommonExtentionsTests
    {
        [TestMethod]
        public void CommonExtentions_GetAllChildren()
        {
            // Arrange.
            // Prepare ConfigModel instance to test.

            #region arrange

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
                         new FreqModel()
                         {
                             Config = config,
                             Spectrum = "10-124,1022,1024"
                         }
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
                    Operators = opers
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


            // act
            var childrens = config.GetAllConfigItemBaseChildren();

            foreach (var item in config.Regions)
            {
                Assert.IsTrue(childrens.Contains((ConfigItemBase)item));
            }

            foreach (var item in config.Regions.SelectMany(o => o.Technologies))
            {
                Assert.IsTrue(childrens.Contains((ConfigItemBase)item));
            }

            var allOperators = config.Regions
                                   .SelectMany(o => o.Technologies)
                                   .SelectMany(o => o.Operators);

            foreach (var item in allOperators)
            {
                Assert.IsTrue(childrens.Contains((ConfigItemBase)item));
            }

            var allFreqs = config.Regions
                               .SelectMany(o => o.Technologies)
                               .SelectMany(o => o.Operators)
                               .SelectMany(o => o.Freqs);

            foreach (var item in allFreqs)
            {
                Assert.IsTrue(childrens.Contains((ConfigItemBase)item));
            }

            Assert.AreEqual(19, childrens.Count());
        }
    }
}
