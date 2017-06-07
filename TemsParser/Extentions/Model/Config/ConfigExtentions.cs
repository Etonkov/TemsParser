using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Xml.Serialization;

using TemsParser.Models.Config;
using TemsParser.Models.Settings;
using TemsParser.ViewModels;
using System.Collections.Specialized;



namespace TemsParser.Extentions.Model.Config
{
    public static class ConfigExtentions
    {
        /// <summary>
        /// Determines whether the ConfigModel and all of his elements is valid.
        /// Outputs ErrorMessages whith objectName in the validationResults collections.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool Validate(this ConfigModel config, out ICollection<ValidationResult> result)
        {
            result = new List<ValidationResult>();

            if (config == null)
            {
                result.Add(new ValidationResult("Конфигурация не найдена.)"));
                return false;
            }
            else
            {
                bool returns = true;

                var validationItems = config.GetAllChildren<IValidationItem>();

                foreach (var regionsListItem in validationItems)
                {
                    ValidateItem(regionsListItem, ref returns, ref result);
                }

                return returns;
            }
        }

        /// <summary>
        /// Determines whether the validationItem is valid.
        /// If the validationItem is not valid change the itemIsValid to false
        /// and add the ErrorMessage whith the object name to the validationResults collections.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationItem"></param>
        /// <param name="itemsIsValid"></param>
        /// <param name="result"></param>
        /// <param name="itemName"></param>
        private static void ValidateItem<T>(T validationItem,
                                            ref bool itemsIsValid,
                                            ref ICollection<ValidationResult> result)
        {
            Dictionary<Type, string> classDisplayNames = 
                new Dictionary<Type, string>()
                {
                    {typeof(ConfigModel), "config"},
                    {typeof(RegionListItemModel), "regionsList"},
                    {typeof(TechnologyListItemModel), "technologiesList"},
                    {typeof(OperatorListItemModel), "operatorsList"},
                    {typeof(RegionModel), "region"},
                    {typeof(OperatorModel), "operator"},
                    {typeof(TechnologyModel), "technology"},
                    {typeof(FreqModel), "freq"}
                };

            List<ValidationResult> thisValidationResults = new List<ValidationResult>();

            bool currentIsValid = Validator.TryValidateObject(validationItem,
                                                              new ValidationContext(validationItem),
                                                              thisValidationResults,
                                                              true);

            if (currentIsValid == false)
            {
                string className = String.Empty;

                Type itemType = validationItem.GetType();



                if (classDisplayNames.TryGetValue(itemType, out className) == false)
                {
                    className = itemType.ToString();
                }

                string itemName = validationItem.ToString();

                if (itemName != String.Empty)
                {
                    itemName = ": \"" + itemName + "\"";
                }

                var totalErrorMessage = className + itemName;

                foreach (var validationResult in thisValidationResults)
                {
                    totalErrorMessage += "\n- " + validationResult.ErrorMessage;
                }

                result.Add(new ValidationResult(totalErrorMessage)); 
            }

            itemsIsValid &= currentIsValid;
        }

        public static IEnumerable<ConfigItemBase> GetAllConfigItemBaseChildren(this ConfigModel config)
        {
            var returns = new List<ConfigItemBase>();

            foreach (var reg in config.Regions)
            {
                returns.Add((ConfigItemBase)reg);

                foreach (var tech in reg.Technologies)
                {
                    returns.Add((ConfigItemBase)tech);

                    foreach (var oper in tech.Operators)
                    {
                        returns.Add((ConfigItemBase)oper);

                        foreach (var freq in oper.Freqs)
                        {
                            returns.Add((ConfigItemBase)freq);
                        }
                    }
                }
            }

            return returns.Distinct();
        }

        public static ConfigItemBase GetItem(this ConfigModel config, int itemId)
        {
            return config.GetAllConfigItemBaseChildren().First(o => (o.Id == itemId));
        }

        public static ConfigModel GetFiltered(this ConfigModel config, SettingsModel setting)
        {
            var returns = new ConfigModel(config);

            var filteredRegion = returns.Regions.FirstOrDefault(r => (r.ToString() == setting.SelectedRegion));

            returns.Regions = new ObservableCollection<RegionModel>() { filteredRegion };

            var filteredTechnologies = from t in filteredRegion.Technologies
                                       where setting.SelectedTechnologies.Contains(t.ToString())
                                       where t.Operators.SelectMany(o => o.Freqs).SelectMany(f => f.Spectrum).Count() != 0
                                       select t;

            filteredRegion.Technologies = new ObservableCollection<TechnologyModel>(filteredTechnologies);

            foreach (var technologyItem in filteredTechnologies)
            {
                var filteredOperators = from o in technologyItem.Operators
                                        where setting.SelectedOperators.Contains(o.ToString())
                                        where o.Freqs.SelectMany(f => f.Spectrum).Count() != 0
                                        select o;

                technologyItem.Operators = new ObservableCollection<OperatorModel>(filteredOperators);
            }

            var filteredRegionsList = from r in returns.RegionsList
                                      where r.Regions.Count > 0
                                      select r;

            returns.RegionsList = new ObservableCollection<RegionListItemModel>(filteredRegionsList);

            var filteredTechnologiesList = from t in returns.TechnologiesList
                                           where t.Technologies.Count > 0
                                           select t;

            returns.TechnologiesList = new ObservableCollection<TechnologyListItemModel>(filteredTechnologiesList);

            var filteredOperatorsList = from o in returns.OperatorsList
                                        where o.Operators.Count > 0
                                        select o;

            returns.OperatorsList = new ObservableCollection<OperatorListItemModel>(filteredOperatorsList);

            return returns;
        }
    }
}
