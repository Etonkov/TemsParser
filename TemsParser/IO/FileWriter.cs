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
    public class FileWriter
    {
        private FileWriter() { }


        private static void SerializeObject(dynamic data, string filePath)
        {
            var binFormatter = new BinaryFormatter();

            using (var fStream = new FileStream(filePath, FileMode.Create))
            {
                binFormatter.Serialize(fStream, data);
            }
        }

        public static void SerializeConfig(ConfigModel configModel)
        {
            lock (Global.LockFileOperations)
            {
                SerializeObject(configModel, Global.ConfigFilePath);
            }
        }

        public static async Task SerializeConfigAsync(ConfigModel configModel)
        {
            await Task.Run(() =>
            {
                try
                {
                    SerializeConfig(configModel);
                }
                catch (Exception e)
                {
                    Alarms.ShowError("Ошибка сохранения файла конфигурации", e.ToString());
                }
            });
        }

        public static void SerializeSettings(SettingsModel settingsModel)
        {
            lock (Global.LockFileOperations)
            {
                SerializeObject(settingsModel, Global.SettingsFilePath);
            }
        }

        public static void SerializeBinArray<T>(T[,] binArray, string filePath)
            where T: struct, IPoint
        {
            lock (Global.LockFileOperations)
            {
                SerializeObject(binArray, filePath);
            }
        }
    }
}
