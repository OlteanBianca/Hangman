using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Hangman.Models
{
    [Serializable]
    public class Statistics : INotifyPropertyChanged
    {
        private int won;
        private int lost;
        private string category;

        [XmlAttribute]
        public string Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        [XmlElement]
        public int Won
        {
            get => won;
            set
            {
                won = value;
                OnPropertyChanged("Won");
            }
        }

        [XmlElement]
        public int Lost
        {
            get => lost;
            set
            {
                lost = value;
                OnPropertyChanged("Lost");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                if (propertyName == "Update")
                {
                    //using (StreamWriter file = File.CreateText(@"..\..\Users\" + User.UserName + @"\" + idGame))
                    //{
                    //    file.WriteLine(category);
                    //    file.WriteLine(lvl);
                    //    file.WriteLine(livesLeft);
                    //    file.WriteLine(timeLeft);
                    //    file.WriteLine(currentWord);
                    //    file.WriteLine(originalWord);
                    //    file.WriteLine(lettersUsed);
                    //}
                }
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
