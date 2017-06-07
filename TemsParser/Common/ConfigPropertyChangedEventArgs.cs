using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.Common
{
    /// <summary>
    /// Provides data for the ConfigPropertyChanged event.
    /// </summary>
    public class ConfigPropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ConfigPropertyChangedEventArgs class
        /// using the old and the new value of Config property.
        /// </summary>
        /// <param name="oldValue">The old value of Config property.</param>
        /// <param name="newValue">The new value of Config property.</param>
        public ConfigPropertyChangedEventArgs(ConfigModel oldValue, ConfigModel newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }


        /// <summary>
        /// The old value of observable collection property.
        /// </summary>
        public ConfigModel OldValue { get; private set; }

        /// <summary>
        /// The new value of observable collection property.
        /// </summary>
        public ConfigModel NewValue { get; private set; }
    }
}
