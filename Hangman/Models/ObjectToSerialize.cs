using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Hangman.Models
{
    [Serializable]
    public class ObjectToSerialize
    {
        [XmlArray]
        public ObservableCollection<Game> Games { get; set; }

        [XmlArray]
        public List<Statistics> Statistics { get; set; }
    }
}
