using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.ViewModels.Config
{
    public class RenamingData
    {
        public RenamingData(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }

        public string OldName { get; private set; }
        public string NewName { get; private set; }
    }
}
