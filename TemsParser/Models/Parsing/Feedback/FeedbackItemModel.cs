using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using TemsParser.Common;
using TemsParser.Models.Config;

namespace TemsParser.Models.Parsing.Feedback
{
    public class FeedbackItemModel
    {
        public FeedbackItemModel(FeedbackStatus status, string fileName, string message)
        {
            Status = status;
            FileName = fileName;
            Message = message;
            Time = DateTime.Now;
        }

        public FeedbackStatus Status { get; private set; }

        public string FileName { get; private set; }

        public string Message { get; private set; }

        public DateTime Time { get; private set; }


        public override string ToString()
        {
            return String.Join("\t", Status, FileName, Message, Time);
        }
    }
}
