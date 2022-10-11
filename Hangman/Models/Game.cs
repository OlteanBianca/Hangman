using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Hangman.Models
{
    [Serializable]
    public class Game : INotifyPropertyChanged
    {
        private User user;
        private int idGame;
        private string category;
        private string originalWord;
        private string currentWord;
        private string lettersUsed;
        private int lvl;
        private int livesLeft;
        private int timeLeft;

        [XmlElement]
        public User User
        {
            get => user;
            set => user = value;
        }

        [XmlAttribute]
        public int IDGame
        {
            get => idGame;
            set => idGame = value;
        }

        [XmlElement]
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
        public string OriginalWord
        {
            get => originalWord;
            set
            {
                originalWord = value;
                OnPropertyChanged("OriginalWord");
            }
        }

        [XmlElement]
        public string CurrentWord
        {
            get
            {
                currentWord = currentWord.Replace(" ", "");
                string word = "";
                foreach (char i in currentWord)
                {
                    word += i + " ";
                }
                return word;
            }
            set
            {
                currentWord = value;
                OnPropertyChanged("currentWord");
            }
        }

        [XmlElement]
        public string LettersUsed
        {
            get => lettersUsed;
            set
            {
                lettersUsed = value;
                OnPropertyChanged("LettersUsed");
            }
        }

        [XmlElement]
        public int Lvl
        {
            get => lvl;
            set
            {
                lvl = value;
                OnPropertyChanged("Lvl");
            }
        }

        [XmlElement]
        public int LivesLeft
        {
            get => livesLeft;
            set
            {
                livesLeft = value;
                OnPropertyChanged("LivesLeft");
            }
        }

        [XmlElement]
        public int TimeLeft
        {
            get => timeLeft;
            set
            {
                timeLeft = value;
                OnPropertyChanged("TimeLeft");
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
