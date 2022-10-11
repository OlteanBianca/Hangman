using Hangman.Models;
using Hangman.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Hangman.ViewModels
{
    internal class StatisticsVM
    {
        private readonly Serialization serialization;

        public ObservableCollection<Tuple<string, List<Statistics>>> UsersStatistics { get; set; }

        public StatisticsVM()
        {
            UsersStatistics = new ObservableCollection<Tuple<string, List<Statistics>>>();

            DirectoryInfo directory = new DirectoryInfo(@"..\..\Users\");
            foreach (DirectoryInfo it in directory.GetDirectories())
            {
                serialization = new Serialization(it.Name);
                List<Statistics> list = serialization.DeserializeStatistics();

                UsersStatistics.Add(new Tuple<string, List<Statistics>>(it.Name, list));
            }
        }
    }
}
