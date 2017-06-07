using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TemsParser.Models.Config
{
    /// <summary>
    /// Represent the exceptions that is thrown when the configuration contains more
    /// elements than the maximum possible items.
    /// </summary>
    [Serializable]
    public class TooManyItemsInConfigurationException : Exception
    {
        /// <summary>
        /// The exceptions that is thrown when the configuration contains more
        /// elements than the maximum possible items.
        /// </summary>
        public TooManyItemsInConfigurationException() { }

        /// <summary>
        /// The exceptions that is thrown when the configuration contains more
        /// elements than the maximum possible items.
        /// </summary>
        public TooManyItemsInConfigurationException(string message) : base(message) { }

        /// <summary>
        /// The exceptions that is thrown when the configuration contains more
        /// elements than the maximum possible items.
        /// </summary>
        public TooManyItemsInConfigurationException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// The exceptions that is thrown when the configuration contains more
        /// elements than the maximum possible items.
        /// </summary>
        protected TooManyItemsInConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
