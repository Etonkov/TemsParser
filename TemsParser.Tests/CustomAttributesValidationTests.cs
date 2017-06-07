using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.CustomAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace TemsParser.Tests
{
    [TestClass]
    public class CustomValidationAttributesTests
    {
        [TestMethod]
        public void CustomValidationAttributes_CoordinateAttribute()
        {
            // Arrange.
            var latitudeCoordinateAttribute = new CoordinateAttribute(CoordinateType.Latitude);
            var longitudeCoordinateAttribute = new CoordinateAttribute(CoordinateType.Longitude);

            // Act.
            var validLatitudeCoordinateValues = new List<string>()
            {
                "-90",
                "  0  ",
                "90",
                "51.6895179748535",
                "51,6895179748535",
                "37.8588829040527",
                "37,8588829040527"
            };

            var invalidLatitudeCoordinateValues = new List<string>()
            {
                "-91",
                "91",
                "p51.6895179748535",
                "100000",
                "string"
            };

            var validLongitudeCoordinateValues = new List<string>()
            {
                "-180",
                "0",
                "  90  ",
                "180",
                "51.6895179748535",
                "51,6895179748535",
                "37.8588829040527567567567567567",
                "37,8588829040527"
            };

            var invalidLongitudeCoordinateValues = new List<string>()
            {
                "-181",
                "181",
                "p51.6895179748535",
                "100000",
                "string"
            };

            
            // Assert.
            foreach (var item in validLatitudeCoordinateValues)
            {
                Assert.IsTrue(latitudeCoordinateAttribute.IsValid(item));
            }

            foreach (var item in invalidLatitudeCoordinateValues)
            {
                Assert.IsFalse(latitudeCoordinateAttribute.IsValid(item));
            }

            foreach (var item in validLongitudeCoordinateValues)
            {
                Assert.IsTrue(longitudeCoordinateAttribute.IsValid(item));
            }

            foreach (var item in invalidLongitudeCoordinateValues)
            {
                Assert.IsFalse(longitudeCoordinateAttribute.IsValid(item));
            }
        }

        [TestMethod]
        public void CustomValidationAttributes_EqualsTrueAttribute()
        {
            // Arrange.
            var equalsTrueAttribute = new EqualsTrueAttribute();

            // Act + assert.
            Assert.IsTrue(equalsTrueAttribute.IsValid(true));
            Assert.IsFalse(equalsTrueAttribute.IsValid(false));

            try
            {
                equalsTrueAttribute.IsValid("string");
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException)
            {
                // In this case an exception must be thrown. Othewise, assert fail.
            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void CustomValidationAttributes_FreqSpectrumAttribute()
        {
            // Arrange.
            var freqSpectrumAttribute = new FreqSpectrumAttribute();


            // Act.
            var validFreqSpectrumValues = new List<string>()
            {
                "0",
                "  0  ",
                "\t\n0",
                " 1, 2, 132 ,3-16 ",
                "65535,213,132132",
                "65535;213,132132;123-124",
            };

            var invalidFreqSpectrumValues = new List<string>()
            {
                "-9",
                "4--5",
                "1,3,-4",
                "1;234;-235",
                "string",
                "1;234;235pp",
                "1;132-23"
            };

            // Assert.
            foreach (var item in validFreqSpectrumValues)
            {
                Assert.IsTrue(freqSpectrumAttribute.IsValid(item));
            }

            foreach (var item in invalidFreqSpectrumValues)
            {
                Assert.IsFalse(freqSpectrumAttribute.IsValid(item));
            }
        }

        [TestMethod]
        public void CustomValidationAttributes_UniqueValuesCollectionAttribute()
        {
            // Arrange.
            var uniqueValuesCollectionAttribute = new UniqueValuesCollectionAttribute();


            // Act.
            var validUiqueValuesCollectionValues = new List<IEnumerable<object>>()
            {
                new List<object>()
                {
                    "one",
                    "two",
                    "three",
                    "four",
                    "five"
                },
                new List<object>()
                {
                    1,
                    2,
                    3,
                    4,
                    5
                }
            };

            var invalidUiqueValuesCollectionValues = new List<IEnumerable<object>>()
            {
                new List<object>()
                {
                    "one",
                    " one ",
                    "one  ",
                },
                new List<object>()
                {
                    "one",
                    "two",
                    "three",
                    "three"
                },
                new List<object>()
                {
                    1,
                    1
                },
                new List<object>()
                {
                    1,
                    2,
                    3,
                    3
                },
            };

            // Assert.
            foreach (var item in validUiqueValuesCollectionValues)
            {
                Assert.IsTrue(uniqueValuesCollectionAttribute.IsValid(item));
            }

            foreach (var item in invalidUiqueValuesCollectionValues)
            {
                Assert.IsFalse(uniqueValuesCollectionAttribute.IsValid(item));
            }
        }
    }
}
