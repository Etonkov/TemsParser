using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.Parsing.Feedback
{
    /// <summary>
    /// Provides data for the FeedbackCreated event.
    /// </summary>
    public class FeedbackCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ReportCreatedEventArgs class using the log item.
        /// </summary>
        /// <param name="feedbackItem"></param>
        public FeedbackCreatedEventArgs(FeedbackItemModel feedbackItem)
        {
            FeedbackItem = feedbackItem;
        }

        /// <summary>
        /// Data to display.
        /// </summary>
        public FeedbackItemModel FeedbackItem { get; private set; }
    }
}
