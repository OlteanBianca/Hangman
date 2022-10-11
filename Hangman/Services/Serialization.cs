using Hangman.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Hangman.Services
{
    internal class Serialization
    {
        private readonly string user;

        public Serialization(string user)
        {
            this.user = user;
        }

        // afisare
        public void SerializeObject(object entity)
        {
            XmlSerializer xmlser = new XmlSerializer(entity.GetType());
            FileStream fileStr = entity is ObservableCollection<Game>
                ? new FileStream(@"..\..\Users\" + user + @"\games.xml", FileMode.Create)
                : new FileStream(@"..\..\Users\" + user + @"\statistics.xml", FileMode.Create);

            xmlser.Serialize(fileStr, entity);
            fileStr.Dispose();
        }

        //citire
        public ObservableCollection<Game> DeserializeGames()
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(ObservableCollection<Game>));
            FileStream file = new FileStream(@"..\..\Users\" + user + @"\games.xml", FileMode.Open);
            ObservableCollection<Game> Games = xmlser.Deserialize(file) as ObservableCollection<Game>;

            file.Dispose();
            return Games;
        }

        public List<Statistics> DeserializeStatistics()
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(List<Statistics>));
            FileStream file = new FileStream(@"..\..\Users\" + user + @"\statistics.xml", FileMode.Open);

            List<Statistics> Statistics = xmlser.Deserialize(file) as List<Statistics>;

            file.Dispose();
            return Statistics;
        }
    }
}
