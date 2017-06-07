using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.IO
{
    public class Global
    {
        public const string ConfigFilePath = "config.dat";
        public const string SettingsFilePath = "settings.dat";
        public static readonly object LockFileOperations = new Object();

        private Global()
        {

        }
    }
}
