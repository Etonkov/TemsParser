using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using TemsParser.Messages;
using System.ComponentModel.DataAnnotations;
using TemsParser.Models.Config;
using TemsParser.Models.Settings;
using TemsParser.Extentions;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using TemsParser.ViewModels;
using TemsParser.Models.Parsing;
using TemsParser.Processing;
using System.Windows;
using System.Threading.Tasks;
using System.Diagnostics;
using TemsParser.Models.Parsing.Point;

namespace TemsParser.IO
{
    public class FileReader
    {
        private FileReader() { }


        private static T Deserialise<T>(string filePath)
        {
            var result = default(T);

            var binFormatter = new BinaryFormatter();

            using (var fStream = File.OpenRead(filePath))
            {
                result = (T)binFormatter.Deserialize(fStream);
            }

            return result;
        }

        public static ConfigModel DeserialiseConfig()
        {
            return Deserialise<ConfigModel>(Global.ConfigFilePath);
        }

        public static SettingsModel DeserialiseSettings()
        {
            return Deserialise<SettingsModel>(Global.SettingsFilePath);
        }

        public static T[,] DeserialisePointArray<T>(string filePath)
            where T: struct, IPoint
        {
            lock (Global.LockFileOperations)
            {
                var result = Deserialise<T[,]>(filePath);
                return result;
            }
        }
    }
}
