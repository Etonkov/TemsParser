using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;

namespace TemsParser.IO
{
    public class ImportConfigResult
    {
        public ImportConfigResult(bool result, ConfigModel config)
        {
            Result = result;
            Config = config;
        }


        public bool Result { get; private set; }

        public ConfigModel Config { get; private set; }
    }
}
