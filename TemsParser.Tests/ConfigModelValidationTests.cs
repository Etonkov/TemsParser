using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemsParser.Models.Config;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TemsParser.Tests
{
    [TestClass]
    public class ConfigModelValidationTests
    {
        [TestMethod]
        public void ConfigModelValidation_TechnologiesList_UniqueValuesCollectionAttribute_Validation()
        {
            // Arrange.
            var config = new ConfigModel();

            // Act.
            var TechnologyListItemModel1 = new TechnologyListItemModel() { Name = "NotUniqueName" };
            var TechnologyListItemModel2 = new TechnologyListItemModel() { Name = "NotUniqueName" };
            config.TechnologiesList.Add(TechnologyListItemModel1);
            config.TechnologiesList.Add(TechnologyListItemModel2);

            // Assert.
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(config) { MemberName = "TechnologiesList" };
            bool isValid = Validator.TryValidateProperty(config.TechnologiesList, vc, vr);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ConfigModelValidation_TechnologyListItemModel_RequiredAttribute_Validation()
        {
            // Arrange.
            var TechnologyListItemModel = new TechnologyListItemModel();

            // Act.
            TechnologyListItemModel.Name = String.Empty;
            TechnologyListItemModel.FreqColumnNamePart = String.Empty;
            TechnologyListItemModel.LatitudeColumnName = String.Empty;
            TechnologyListItemModel.LevelColumnNamePart = String.Empty;
            TechnologyListItemModel.LongitudeColumnName = String.Empty;


            // Assert.
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(TechnologyListItemModel) { MemberName = "Name" };
            bool isValid1 = Validator.TryValidateProperty(TechnologyListItemModel.Name, vc, vr);


            vr = new List<ValidationResult>();
            vc = new ValidationContext(TechnologyListItemModel){ MemberName = "FreqColumnNamePart" };
            bool isValid2 = Validator.TryValidateProperty(TechnologyListItemModel.FreqColumnNamePart, vc, vr);

            vr = new List<ValidationResult>();
            vc = new ValidationContext(TechnologyListItemModel) { MemberName = "LatitudeColumnName" };
            bool isValid3 = Validator.TryValidateProperty(TechnologyListItemModel.LatitudeColumnName, vc, vr);

            vr = new List<ValidationResult>();
            vc = new ValidationContext(TechnologyListItemModel) { MemberName = "LevelColumnNamePart" };
            bool isValid4 = Validator.TryValidateProperty(TechnologyListItemModel.LevelColumnNamePart, vc, vr);

            vr = new List<ValidationResult>();
            vc = new ValidationContext(TechnologyListItemModel) { MemberName = "LongitudeColumnName" };
            bool isValid5 = Validator.TryValidateProperty(TechnologyListItemModel.LongitudeColumnName, vc, vr);

            Assert.IsFalse(isValid1);
            Assert.IsFalse(isValid2);
            Assert.IsFalse(isValid3);
            Assert.IsFalse(isValid4);
            Assert.IsFalse(isValid5);
        }

        [TestMethod]
        public void ConfigModelValidation_RegionsListValidation()
        {
            // Arrange.
            var config = new ConfigModel();

            // Act.
            var emptyRegionsListItem = new RegionListItemModel()
            {
                Name = String.Empty
            };

            config.RegionsList.Add(new RegionListItemModel() { Name = "NotUniqueName" });
            config.RegionsList.Add(new RegionListItemModel() { Name = "NotUniqueName" });

            // Assert.
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(config) { MemberName = "RegionsList" };
            bool notUniqueRegionsListIsValid = Validator.TryValidateProperty(config.RegionsList, vc, vr);
            vc = new ValidationContext(emptyRegionsListItem);
            bool emptyRegionsListItemisValid = Validator.TryValidateObject(emptyRegionsListItem, vc, vr);
            Assert.IsFalse(notUniqueRegionsListIsValid);
            Assert.IsFalse(notUniqueRegionsListIsValid);

        }
    }
}
