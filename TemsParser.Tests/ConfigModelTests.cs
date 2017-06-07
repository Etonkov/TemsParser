
using TemsParser.Models.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;
using System;


namespace TemsParser.Tests
{
    // All ConfigItemBase objects in ConfigModel must automaticaly sets Config property
    // when added and replaced in collection. And, after that, assigned the unique available Id.
    // At first: Id = ++ConfigModel.CurrentId. At second: when ConfigModel.CurrentId = Int32.MaxValue
    // (overflow CurrentId) programm must found and set first free Id of ConfigItemBase object(reuse Ids).
    // For removed ConfigItemBase elements(from ConfigModel) Config property
    // must be set to null and its Id must set to 0.
    [TestClass]
    public class ConfigModelTests
    {
        [TestMethod]
        public void ConfigModel_ObservableCollectionChanged_NormalMode()
        {
            // Arrange.
            ConfigModel config = new ConfigModel() { };


            // Act + assert.
            #region RegionModel

            for (int i = 1; i <= 10; i++)
            {
                var addedRegion = new RegionModel()
                {
                    Name = String.Format("addedRegion-{0}", i)
                };

                config.Regions.Add(addedRegion);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedRegion.Config);
                Assert.AreEqual(i, addedRegion.Id);

                config.Regions.Remove(addedRegion);

                // Testing removed RegionModel.
                Assert.IsNull(addedRegion.Config);
                Assert.AreEqual(0, addedRegion.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedRegion = new RegionModel()
                {
                    Name = String.Format("addedRegion-{0}", i)
                };

                config.Regions.Add(addedRegion);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedRegion.Config);
                Assert.AreEqual((i*2 + 9), addedRegion.Id);

                var replacedRegion = new RegionModel()
                {
                    Name = String.Format("replacedRegion-{0}", i)
                };

                config.Regions[i - 1] = replacedRegion;

                // Testing replaced RegionModel.
                Assert.IsNull(addedRegion.Config);
                Assert.AreEqual(0, addedRegion.Id);
                Assert.AreEqual(config, replacedRegion.Config);
                Assert.AreEqual((i * 2 + 10), replacedRegion.Id);
            }

            #endregion


            #region TechnologyModel

            for (int i = 1; i <= 10; i++)
            {
                var addedTechnology = new TechnologyModel()
                {
                    Name = String.Format("addedTechnology-{0}", i)
                };

                config.Regions[0].Technologies.Add(addedTechnology);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedTechnology.Config);
                Assert.AreEqual(i + 30, addedTechnology.Id);

                config.Regions[0].Technologies.Remove(addedTechnology);

                // Testing removed RegionModel.
                Assert.IsNull(addedTechnology.Config);
                Assert.AreEqual(0, addedTechnology.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedTechnology = new TechnologyModel()
                {
                    Name = String.Format("addedTechnology-{0}", i)
                };

                config.Regions[0].Technologies.Add(addedTechnology);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedTechnology.Config);
                Assert.AreEqual((i * 2 + 39), addedTechnology.Id);

                var replacedTechnology = new TechnologyModel()
                {
                    Name = String.Format("replacedTechnology-{0}", i)
                };

                config.Regions[0].Technologies[i - 1] = replacedTechnology;

                // Testing replaced RegionModel.
                Assert.IsNull(addedTechnology.Config);
                Assert.AreEqual(0, addedTechnology.Id);
                Assert.AreEqual(config, replacedTechnology.Config);
                Assert.AreEqual((i * 2 + 40), replacedTechnology.Id);
            }

            #endregion


            #region OperatorModel

            for (int i = 1; i <= 10; i++)
            {
                var addedOperator = new OperatorModel()
                {
                    Name = String.Format("addedOperator-{0}", i)
                };

                config.Regions[0].Technologies[0].Operators.Add(addedOperator);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedOperator.Config);
                Assert.AreEqual(i + 60, addedOperator.Id);

                config.Regions[0].Technologies[0].Operators.Remove(addedOperator);

                // Testing removed RegionModel.
                Assert.IsNull(addedOperator.Config);
                Assert.AreEqual(0, addedOperator.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedOperator = new OperatorModel()
                {
                    Name = String.Format("addedOperator-{0}", i)
                };

                config.Regions[0].Technologies[0].Operators.Add(addedOperator);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedOperator.Config);
                Assert.AreEqual((i * 2 + 69), addedOperator.Id);

                var replacedOperator = new OperatorModel()
                {
                    Name = String.Format("replacedOperator-{0}", i)
                };

                config.Regions[0].Technologies[0].Operators[i - 1] = replacedOperator;

                // Testing replaced RegionModel.
                Assert.IsNull(addedOperator.Config);
                Assert.AreEqual(0, addedOperator.Id);
                Assert.AreEqual(config, replacedOperator.Config);
                Assert.AreEqual((i * 2 + 70), replacedOperator.Id);
            }

            #endregion


            #region FreqModel

            for (int i = 1; i <= 10; i++)
            {
                var addedFreq = new FreqModel()
                {
                    Spectrum = i.ToString()
                };

                config.Regions[0].Technologies[0].Operators[0].Freqs.Add(addedFreq);

                Assert.AreEqual(config, addedFreq.Config);
                Assert.AreEqual(i + 90, addedFreq.Id);

                config.Regions[0].Technologies[0].Operators[0].Freqs.Remove(addedFreq);

                Assert.IsNull(addedFreq.Config);
                Assert.AreEqual(0, addedFreq.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedFreq = new FreqModel()
                {
                    Spectrum = i.ToString()
                };

                config.Regions[0].Technologies[0].Operators[0].Freqs.Add(addedFreq);

                Assert.AreEqual(config, addedFreq.Config);
                Assert.AreEqual((i * 2 + 99), addedFreq.Id);

                var replacedFreq = new FreqModel()
                {
                    Spectrum = i.ToString()
                };

                config.Regions[0].Technologies[0].Operators[0].Freqs[i - 1] = replacedFreq;

                Assert.IsNull(addedFreq.Config);
                Assert.AreEqual(0, addedFreq.Id);
                Assert.AreEqual(config, replacedFreq.Config);
                Assert.AreEqual((i * 2 + 100), replacedFreq.Id);
            }

            Assert.AreEqual(120, config.CurrentId);

            #endregion 
        }

        [TestMethod]
        public void ConfigModel_ObservableCollectionChanged_OverflowMode()
        { 
            // Arrange.
            ConfigModel config = new ConfigModel();
            var privateObject = new PrivateObject(config);
            privateObject.SetField("_currentId", Int32.MaxValue);

            // Act + assert.
            #region RegionModel

            for (int i = 1; i <= 10; i++)
            {
                var addedRegion = new RegionModel()
                {
                    Name = String.Format("addedRegion-{0}", i)
                };

                config.Regions.Add(addedRegion);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedRegion.Config);
                Assert.AreEqual(i, addedRegion.Id);

                config.Regions.Remove(addedRegion);

                // Testing removed RegionModel.
                Assert.IsNull(addedRegion.Config);
                Assert.AreEqual(0, addedRegion.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedRegion = new RegionModel()
                {
                    Name = String.Format("addedRegion-{0}", i)
                };

                config.Regions.Add(addedRegion);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedRegion.Config);
                Assert.AreEqual((i * 2 + 9), addedRegion.Id);

                var replacedRegion = new RegionModel()
                {
                    Name = String.Format("replacedRegion-{0}", i)
                };

                config.Regions[i - 1] = replacedRegion;

                // Testing replaced RegionModel.
                Assert.IsNull(addedRegion.Config);
                Assert.AreEqual(0, addedRegion.Id);
                Assert.AreEqual(config, replacedRegion.Config);
                Assert.AreEqual((i * 2 + 10), replacedRegion.Id);
            }

            #endregion


            #region TechnologyModel

            for (int i = 1; i <= 10; i++)
            {
                var addedTechnology = new TechnologyModel()
                {
                    Name = String.Format("addedTechnology-{0}", i)
                };

                config.Regions[0].Technologies.Add(addedTechnology);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedTechnology.Config);
                Assert.AreEqual(i + 30, addedTechnology.Id);

                config.Regions[0].Technologies.Remove(addedTechnology);

                // Testing removed RegionModel.
                Assert.IsNull(addedTechnology.Config);
                Assert.AreEqual(0, addedTechnology.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedTechnology = new TechnologyModel()
                {
                    Name = String.Format("addedTechnology-{0}", i)
                };

                config.Regions[0].Technologies.Add(addedTechnology);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedTechnology.Config);
                Assert.AreEqual((i * 2 + 39), addedTechnology.Id);

                var replacedTechnology = new TechnologyModel()
                {
                    Name = String.Format("replacedTechnology-{0}", i)
                };

                config.Regions[0].Technologies[i - 1] = replacedTechnology;

                // Testing replaced RegionModel.
                Assert.IsNull(addedTechnology.Config);
                Assert.AreEqual(0, addedTechnology.Id);
                Assert.AreEqual(config, replacedTechnology.Config);
                Assert.AreEqual((i * 2 + 40), replacedTechnology.Id);
            }

            #endregion


            #region OperatorModel

            for (int i = 1; i <= 10; i++)
            {
                var addedOperator = new OperatorModel()
                {
                    Name = String.Format("addedOperator-{0}", i)
                };

                config.Regions[0].Technologies[0].Operators.Add(addedOperator);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedOperator.Config);
                Assert.AreEqual(i + 60, addedOperator.Id);

                config.Regions[0].Technologies[0].Operators.Remove(addedOperator);

                // Testing removed RegionModel.
                Assert.IsNull(addedOperator.Config);
                Assert.AreEqual(0, addedOperator.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedOperator = new OperatorModel()
                {
                    Name = String.Format("addedOperator-{0}", i)
                };

                config.Regions[0].Technologies[0].Operators.Add(addedOperator);

                // Testing added RegionModel.
                Assert.AreEqual(config, addedOperator.Config);
                Assert.AreEqual((i * 2 + 69), addedOperator.Id);

                var replacedOperator = new OperatorModel()
                {
                    Name = String.Format("replacedOperator-{0}", i)
                };

                config.Regions[0].Technologies[0].Operators[i - 1] = replacedOperator;

                // Testing replaced RegionModel.
                Assert.IsNull(addedOperator.Config);
                Assert.AreEqual(0, addedOperator.Id);
                Assert.AreEqual(config, replacedOperator.Config);
                Assert.AreEqual((i * 2 + 70), replacedOperator.Id);
            }

            #endregion


            #region FreqModel

            for (int i = 1; i <= 10; i++)
            {
                var addedFreq = new FreqModel()
                {
                    Spectrum = i.ToString()
                };

                config.Regions[0].Technologies[0].Operators[0].Freqs.Add(addedFreq);

                Assert.AreEqual(config, addedFreq.Config);
                Assert.AreEqual(i + 90, addedFreq.Id);

                config.Regions[0].Technologies[0].Operators[0].Freqs.Remove(addedFreq);

                Assert.IsNull(addedFreq.Config);
                Assert.AreEqual(0, addedFreq.Id);
            }


            for (int i = 1; i <= 10; i++)
            {
                var addedFreq = new FreqModel()
                {
                    Spectrum = i.ToString()
                };

                config.Regions[0].Technologies[0].Operators[0].Freqs.Add(addedFreq);

                Assert.AreEqual(config, addedFreq.Config);
                Assert.AreEqual((i * 2 + 99), addedFreq.Id);

                var replacedFreq = new FreqModel()
                {
                    Spectrum = i.ToString()
                };

                config.Regions[0].Technologies[0].Operators[0].Freqs[i - 1] = replacedFreq;

                Assert.IsNull(addedFreq.Config);
                Assert.AreEqual(0, addedFreq.Id);
                Assert.AreEqual(config, replacedFreq.Config);
                Assert.AreEqual((i * 2 + 100), replacedFreq.Id);
            }

            Assert.AreEqual(120, config.CurrentId);

            #endregion 
        }

        [TestMethod]
        public void ConfigModel_CollectionPropertyChanged_NormalMode()
        {
            // Arrange.
            ConfigModel config = new ConfigModel() { };

            // Act + assert:
            #region RegionModel

            var reg1 = new RegionModel()
            {
                Name = "Region-1"
            };

            var reg2 = new RegionModel()
            {
                Name = "Region-2"
            };

            var reg3 = new RegionModel()
            {
                Name = "Region-3"
            };

            var regs = new ObservableCollection<RegionModel>();
            regs.Add(reg1);
            regs.Add(reg2);
            regs.Add(reg3);

            config.Regions = regs;

            Assert.AreEqual(config, reg1.Config);
            Assert.AreEqual(1, reg1.Id);
            Assert.AreEqual(config, reg2.Config);
            Assert.AreEqual(2, reg2.Id);
            Assert.AreEqual(config, reg3.Config);
            Assert.AreEqual(3, reg3.Id);

            var reg4 = new RegionModel()
            {
                Name = "Region-4"
            };

            var reg5 = new RegionModel()
            {
                Name = "Region-5"
            };

            var reg6 = new RegionModel()
            {
                Name = "Region-6"
            };

            var regs2 = new ObservableCollection<RegionModel>();
            regs2.Add(reg4);
            regs2.Add(reg5);
            regs2.Add(reg6);

            config.Regions = regs2;

            Assert.IsNull(reg1.Config);
            Assert.AreEqual(0, reg1.Id);
            Assert.IsNull(reg2.Config);
            Assert.AreEqual(0, reg2.Id);
            Assert.IsNull(reg3.Config);
            Assert.AreEqual(0, reg3.Id);

            Assert.AreEqual(config, reg4.Config);
            Assert.AreEqual(4, reg4.Id);
            Assert.AreEqual(config, reg5.Config);
            Assert.AreEqual(5, reg5.Id);
            Assert.AreEqual(config, reg6.Config);
            Assert.AreEqual(6, reg6.Id);

            #endregion


            #region TechnologyModel

            var tech1 = new TechnologyModel()
            {
                Name = "Technology-1"
            };

            var tech2 = new TechnologyModel()
            {
                Name = "Technology-2"
            };

            var tech3 = new TechnologyModel()
            {
                Name = "Technology-3"
            };

            var techs = new ObservableCollection<TechnologyModel>();
            techs.Add(tech1);
            techs.Add(tech2);
            techs.Add(tech3);

            config.Regions[0].Technologies = techs;

            Assert.AreEqual(config, tech1.Config);
            Assert.AreEqual(7, tech1.Id);
            Assert.AreEqual(config, tech2.Config);
            Assert.AreEqual(8, tech2.Id);
            Assert.AreEqual(config, tech3.Config);
            Assert.AreEqual(9, tech3.Id);

            var tech4 = new TechnologyModel()
            {
                Name = "Technology-4"
            };

            var tech5 = new TechnologyModel()
            {
                Name = "Technology-5"
            };

            var tech6 = new TechnologyModel()
            {
                Name = "Technology-6"
            };

            var techs2 = new ObservableCollection<TechnologyModel>();
            techs2.Add(tech4);
            techs2.Add(tech5);
            techs2.Add(tech6);

            config.Regions[0].Technologies = techs2;

            Assert.IsNull(tech1.Config);
            Assert.AreEqual(0, tech1.Id);
            Assert.IsNull(tech2.Config);
            Assert.AreEqual(0, tech2.Id);
            Assert.IsNull(tech3.Config);
            Assert.AreEqual(0, tech3.Id);

            Assert.AreEqual(config, tech4.Config);
            Assert.AreEqual(10, tech4.Id);
            Assert.AreEqual(config, tech5.Config);
            Assert.AreEqual(11, tech5.Id);
            Assert.AreEqual(config, tech6.Config);
            Assert.AreEqual(12, tech6.Id);

            #endregion


            #region OperatorModel

            var oper1 = new OperatorModel()
            {
                Name = "OperatorModel-1"
            };

            var oper2 = new OperatorModel()
            {
                Name = "OperatorModel-2"
            };

            var oper3 = new OperatorModel()
            {
                Name = "OperatorModel-3"
            };

            var opers = new ObservableCollection<OperatorModel>();
            opers.Add(oper1);
            opers.Add(oper2);
            opers.Add(oper3);

            config.Regions[0].Technologies[0].Operators = opers;

            Assert.AreEqual(config, oper1.Config);
            Assert.AreEqual(13, oper1.Id);
            Assert.AreEqual(config, oper2.Config);
            Assert.AreEqual(14, oper2.Id);
            Assert.AreEqual(config, oper3.Config);
            Assert.AreEqual(15, oper3.Id);

            var oper4 = new OperatorModel()
            {
                Name = "OperatorModel-4"
            };

            var oper5 = new OperatorModel()
            {
                Name = "OperatorModel-5"
            };

            var oper6 = new OperatorModel()
            {
                Name = "OperatorModel-6"
            };

            var opers2 = new ObservableCollection<OperatorModel>();
            opers2.Add(oper4);
            opers2.Add(oper5);
            opers2.Add(oper6);

            config.Regions[0].Technologies[0].Operators = opers2;

            Assert.IsNull(oper1.Config);
            Assert.AreEqual(0, oper1.Id);
            Assert.IsNull(oper2.Config);
            Assert.AreEqual(0, oper2.Id);
            Assert.IsNull(oper3.Config);
            Assert.AreEqual(0, oper3.Id);

            Assert.AreEqual(config, oper4.Config);
            Assert.AreEqual(16, oper4.Id);
            Assert.AreEqual(config, oper5.Config);
            Assert.AreEqual(17, oper5.Id);
            Assert.AreEqual(config, oper6.Config);
            Assert.AreEqual(18, oper6.Id);

            #endregion


            #region FreqModel

            var freq1 = new FreqModel()
            {
                Spectrum = "FreqModel-1"
            };

            var freq2 = new FreqModel()
            {
                Spectrum = "FreqModel-2"
            };

            var freq3 = new FreqModel()
            {
                Spectrum = "FreqModel-3"
            };

            var freqs = new ObservableCollection<FreqModel>();
            freqs.Add(freq1);
            freqs.Add(freq2);
            freqs.Add(freq3);

            config.Regions[0].Technologies[0].Operators[0].Freqs = freqs;

            Assert.AreEqual(config, freq1.Config);
            Assert.AreEqual(19, freq1.Id);
            Assert.AreEqual(config, freq2.Config);
            Assert.AreEqual(20, freq2.Id);
            Assert.AreEqual(config, freq3.Config);
            Assert.AreEqual(21, freq3.Id);

            var freq4 = new FreqModel()
            {
                Spectrum = "FreqModel-4"
            };

            var freq5 = new FreqModel()
            {
                Spectrum = "FreqModel-5"
            };

            var freq6 = new FreqModel()
            {
                Spectrum = "FreqModel-6"
            };

            var freqs2 = new ObservableCollection<FreqModel>();
            freqs2.Add(freq4);
            freqs2.Add(freq5);
            freqs2.Add(freq6);

            config.Regions[0].Technologies[0].Operators[0].Freqs = freqs2;

            Assert.IsNull(freq1.Config);
            Assert.AreEqual(0, freq1.Id);
            Assert.IsNull(freq2.Config);
            Assert.AreEqual(0, freq2.Id);
            Assert.IsNull(freq3.Config);
            Assert.AreEqual(0, freq3.Id);

            Assert.AreEqual(config, freq4.Config);
            Assert.AreEqual(22, freq4.Id);
            Assert.AreEqual(config, freq5.Config);
            Assert.AreEqual(23, freq5.Id);
            Assert.AreEqual(config, freq6.Config);
            Assert.AreEqual(24, freq6.Id);

            #endregion
        }

        [TestMethod]
        public void ConfigModel_CollectionPropertyChanged_OverflowMode()
        {
            // Arrange.
            ConfigModel config = new ConfigModel() { };
            var privateObject = new PrivateObject(config);
            privateObject.SetField("_currentId", Int32.MaxValue);

            // Act + assert:
            #region RegionModel

            var reg1 = new RegionModel()
            {
                Name = "Region-1"
            };

            var reg2 = new RegionModel()
            {
                Name = "Region-2"
            };

            var reg3 = new RegionModel()
            {
                Name = "Region-3"
            };

            var regs = new ObservableCollection<RegionModel>();
            regs.Add(reg1);
            regs.Add(reg2);
            regs.Add(reg3);

            config.Regions = regs;

            Assert.AreEqual(config, reg1.Config);
            Assert.AreEqual(1, reg1.Id);
            Assert.AreEqual(config, reg2.Config);
            Assert.AreEqual(2, reg2.Id);
            Assert.AreEqual(config, reg3.Config);
            Assert.AreEqual(3, reg3.Id);

            var reg4 = new RegionModel()
            {
                Name = "Region-4"
            };

            var reg5 = new RegionModel()
            {
                Name = "Region-5"
            };

            var reg6 = new RegionModel()
            {
                Name = "Region-6"
            };

            var regs2 = new ObservableCollection<RegionModel>();
            regs2.Add(reg4);
            regs2.Add(reg5);
            regs2.Add(reg6);

            config.Regions = regs2;

            Assert.IsNull(reg1.Config);
            Assert.AreEqual(0, reg1.Id);
            Assert.IsNull(reg2.Config);
            Assert.AreEqual(0, reg2.Id);
            Assert.IsNull(reg3.Config);
            Assert.AreEqual(0, reg3.Id);

            Assert.AreEqual(config, reg4.Config);
            Assert.AreEqual(4, reg4.Id);
            Assert.AreEqual(config, reg5.Config);
            Assert.AreEqual(5, reg5.Id);
            Assert.AreEqual(config, reg6.Config);
            Assert.AreEqual(6, reg6.Id);

            #endregion


            #region TechnologyModel

            var tech1 = new TechnologyModel()
            {
                Name = "Technology-1"
            };

            var tech2 = new TechnologyModel()
            {
                Name = "Technology-2"
            };

            var tech3 = new TechnologyModel()
            {
                Name = "Technology-3"
            };

            var techs = new ObservableCollection<TechnologyModel>();
            techs.Add(tech1);
            techs.Add(tech2);
            techs.Add(tech3);

            config.Regions[0].Technologies = techs;

            Assert.AreEqual(config, tech1.Config);
            Assert.AreEqual(7, tech1.Id);
            Assert.AreEqual(config, tech2.Config);
            Assert.AreEqual(8, tech2.Id);
            Assert.AreEqual(config, tech3.Config);
            Assert.AreEqual(9, tech3.Id);

            var tech4 = new TechnologyModel()
            {
                Name = "Technology-4"
            };

            var tech5 = new TechnologyModel()
            {
                Name = "Technology-5"
            };

            var tech6 = new TechnologyModel()
            {
                Name = "Technology-6"
            };

            var techs2 = new ObservableCollection<TechnologyModel>();
            techs2.Add(tech4);
            techs2.Add(tech5);
            techs2.Add(tech6);

            config.Regions[0].Technologies = techs2;

            Assert.IsNull(tech1.Config);
            Assert.AreEqual(0, tech1.Id);
            Assert.IsNull(tech2.Config);
            Assert.AreEqual(0, tech2.Id);
            Assert.IsNull(tech3.Config);
            Assert.AreEqual(0, tech3.Id);

            Assert.AreEqual(config, tech4.Config);
            Assert.AreEqual(10, tech4.Id);
            Assert.AreEqual(config, tech5.Config);
            Assert.AreEqual(11, tech5.Id);
            Assert.AreEqual(config, tech6.Config);
            Assert.AreEqual(12, tech6.Id);

            #endregion


            #region OperatorModel

            var oper1 = new OperatorModel()
            {
                Name = "OperatorModel-1"
            };

            var oper2 = new OperatorModel()
            {
                Name = "OperatorModel-2"
            };

            var oper3 = new OperatorModel()
            {
                Name = "OperatorModel-3"
            };

            var opers = new ObservableCollection<OperatorModel>();
            opers.Add(oper1);
            opers.Add(oper2);
            opers.Add(oper3);

            config.Regions[0].Technologies[0].Operators = opers;

            Assert.AreEqual(config, oper1.Config);
            Assert.AreEqual(13, oper1.Id);
            Assert.AreEqual(config, oper2.Config);
            Assert.AreEqual(14, oper2.Id);
            Assert.AreEqual(config, oper3.Config);
            Assert.AreEqual(15, oper3.Id);

            var oper4 = new OperatorModel()
            {
                Name = "OperatorModel-4"
            };

            var oper5 = new OperatorModel()
            {
                Name = "OperatorModel-5"
            };

            var oper6 = new OperatorModel()
            {
                Name = "OperatorModel-6"
            };

            var opers2 = new ObservableCollection<OperatorModel>();
            opers2.Add(oper4);
            opers2.Add(oper5);
            opers2.Add(oper6);

            config.Regions[0].Technologies[0].Operators = opers2;

            Assert.IsNull(oper1.Config);
            Assert.AreEqual(0, oper1.Id);
            Assert.IsNull(oper2.Config);
            Assert.AreEqual(0, oper2.Id);
            Assert.IsNull(oper3.Config);
            Assert.AreEqual(0, oper3.Id);

            Assert.AreEqual(config, oper4.Config);
            Assert.AreEqual(16, oper4.Id);
            Assert.AreEqual(config, oper5.Config);
            Assert.AreEqual(17, oper5.Id);
            Assert.AreEqual(config, oper6.Config);
            Assert.AreEqual(18, oper6.Id);

            #endregion


            #region FreqModel

            var freq1 = new FreqModel()
            {
                Spectrum = "FreqModel-1"
            };

            var freq2 = new FreqModel()
            {
                Spectrum = "FreqModel-2"
            };

            var freq3 = new FreqModel()
            {
                Spectrum = "FreqModel-3"
            };

            var freqs = new ObservableCollection<FreqModel>();
            freqs.Add(freq1);
            freqs.Add(freq2);
            freqs.Add(freq3);

            config.Regions[0].Technologies[0].Operators[0].Freqs = freqs;

            Assert.AreEqual(config, freq1.Config);
            Assert.AreEqual(19, freq1.Id);
            Assert.AreEqual(config, freq2.Config);
            Assert.AreEqual(20, freq2.Id);
            Assert.AreEqual(config, freq3.Config);
            Assert.AreEqual(21, freq3.Id);

            var freq4 = new FreqModel()
            {
                Spectrum = "FreqModel-4"
            };

            var freq5 = new FreqModel()
            {
                Spectrum = "FreqModel-5"
            };

            var freq6 = new FreqModel()
            {
                Spectrum = "FreqModel-6"
            };

            var freqs2 = new ObservableCollection<FreqModel>();
            freqs2.Add(freq4);
            freqs2.Add(freq5);
            freqs2.Add(freq6);

            config.Regions[0].Technologies[0].Operators[0].Freqs = freqs2;

            Assert.IsNull(freq1.Config);
            Assert.AreEqual(0, freq1.Id);
            Assert.IsNull(freq2.Config);
            Assert.AreEqual(0, freq2.Id);
            Assert.IsNull(freq3.Config);
            Assert.AreEqual(0, freq3.Id);

            Assert.AreEqual(config, freq4.Config);
            Assert.AreEqual(22, freq4.Id);
            Assert.AreEqual(config, freq5.Config);
            Assert.AreEqual(23, freq5.Id);
            Assert.AreEqual(config, freq6.Config);
            Assert.AreEqual(24, freq6.Id);

            #endregion
        }
    }
}
