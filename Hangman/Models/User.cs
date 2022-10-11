using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Hangman.Models
{
    [Serializable]
    public class User : INotifyPropertyChanged
    {
        private string username;
        private string avatar;

        [XmlAttribute]
        public string UserName
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged("UserName");
            }
        }

        [XmlAttribute]
        public string Avatar
        {
            get => avatar;
            set
            {
                avatar = value;
                OnPropertyChanged("Avatar");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
