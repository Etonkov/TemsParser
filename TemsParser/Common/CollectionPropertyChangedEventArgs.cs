using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Common
{
    /// <summary>
    /// Provides data for the CollectionPropertyChanged event.
    /// </summary>
    public class CollectionPropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the CollectionPropertyChangedEventArgs class
        /// using the old and the new value of observable collection property.
        /// </summary>
        /// <param name="oldValue">The old value of observable collection property.</param>
        /// <param name="newValue">The new value of observable collection property.</param>
        public CollectionPropertyChangedEventArgs(IEnumerable<object> oldValue, IEnumerable<object> newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }


        /// <summary>
        /// The old value of observable collection property.
        /// </summary>
        public IEnumerable<object> OldValue { get; private set; }

        /// <summary>
        /// The new value of observable collection property.
        /// </summary>
        public IEnumerable<object> NewValue { get; private set; }
    }
}
