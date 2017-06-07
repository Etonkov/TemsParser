using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.Config
{
    /// <summary>
    /// Represents named instances.
    /// Instances with a string property called "Name".
    /// </summary>
    public interface IName
    {
        string Name { get; set; }
    }
}
