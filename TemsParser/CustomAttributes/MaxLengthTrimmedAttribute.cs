using System;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace TemsParser.CustomAttributes
{
    public class MaxLengthTrimmedAttribute : MaxLengthAttribute
    {
        public MaxLengthTrimmedAttribute(int length) : base(length)
        {
            
        }



        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value.ToString().Trim(), validationContext);
        }
    }
}
