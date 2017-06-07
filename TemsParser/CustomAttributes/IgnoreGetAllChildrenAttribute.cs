using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.CustomAttributes
{
    /// <summary>
    /// Specifies that a property will be ignored in GetAllChildren() method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreGetAllChildrenAttribute : Attribute
    { }
}
