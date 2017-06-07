using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Common
{
    public delegate void BooleanPropertyChangedEventHandler(object sender, BooleanPropertyChangedEventArgs e);

    /// <summary>
    /// Provides data for the BoolPropertyChanged event.
    /// </summary>
    public class BooleanPropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the BooleanPropertyChangedEventArgs class
        /// using the old and the new value of observable collection property.
        /// </summary>
        /// <param name="oldValue">The old value of boolean property.</param>
        /// <param name="newValue">The new value of boolean property.</param>
        public BooleanPropertyChangedEventArgs(bool oldValue, bool newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }


        /// <summary>
        /// The old value of boolean property.
        /// </summary>
        public bool OldValue { get; private set; }

        /// <summary>
        /// The new value of boolean property.
        /// </summary>
        public bool NewValue { get; private set; }
    }
}
