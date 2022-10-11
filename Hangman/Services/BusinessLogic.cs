using Hangman.Models;
using Hangman.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace Hangman.Services
{
    internal class BusinessLogic
    {
        private readonly DispatcherTimer dispatcherTimer;
        private readonly HangmanVM VM;
        private readonly Serialization serialization;
        private DateTime deadline;


        public BusinessLogic(HangmanVM hangman)
        {
            VM = hangman;

            serialization = new Serialization(VM.User.UserName);
            VM.Obj.Statistics = serialization.DeserializeStatistics();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimerTick);
        }

        private string GenerateNewWord(string category)
        {
            List<string> words = new List<string>();

            if (category == "All categories")
            {
                DirectoryInfo directory = new DirectoryInfo(@"..\..\Categories\");
                foreach (FileInfo it in directory.GetFiles())
                {
                    words.AddRange(File.ReadAllLines(@"..\..\Categories\" + it.Name).ToList());
                }
            }
            else
            {
                words = File.ReadAllLines(@"..\..\Categories\" + category + ".txt").ToList();
            }

            Random rnd = new Random();
            int index = rnd.Next(0, words.Count);
            return words[index];
        }

        private string UpdateWord(char letter)
        {
            string word = VM.CurrentGame.CurrentWord.Replace(" ", "");

            if (VM.CurrentGame.OriginalWord.Contains(" "))
            {
                int index = 0;
                while (VM.CurrentGame.OriginalWord.IndexOf(' ', index) != -1)
                {
                    index = VM.CurrentGame.OriginalWord.IndexOf(' ', index) + 1;
                    word = word.Insert(index - 1, " ");
                }
            }

            for (int i = 0; i < VM.CurrentGame.OriginalWord.Length; i++)
            {
                if (VM.CurrentGame.OriginalWord[i] == letter)
                {
                    word = word.Remove(i, 1).Insert(i, letter.ToString());
                }
                if (VM.CurrentGame.OriginalWord[i] == char.ToUpper(letter))
                {
                    word = word.Remove(i, 1).Insert(i, char.ToUpper(letter).ToString());
                }
            }
            return word;
        }

        private void UpdateSavedGames(bool result)
        {
            StopTimer();
            UpdateStatistics(result);

            VM.IsEnabled = false;
            VM.KeyboardKeys = GenerateKeyboard(false);
            VM.Show(VM.CurrentGame.OriginalWord, result);

            if (VM.CurrentGame.IDGame != -1)
            {
                foreach (Game it in VM.Obj.Games)
                {
                    if (it.IDGame == VM.CurrentGame.IDGame)
                    {
                        _ = VM.Obj.Games.Remove(it);
                        break;
                    }
                }
                if (VM.Obj.Games.Count == 0)
                {
                    File.Delete(@"..\..\Users\" + VM.User.UserName + @"\games.xml");
                }
                else
                {
                    serialization.SerializeObject(VM.Obj.Games);
                }
            }
        }

        private void UpdateStatistics(bool result)
        {
            foreach (Statistics it in VM.Obj.Statistics)
            {
                if (it.Category == VM.CurrentGame.Category)
                {
                    if (result)
                    {
                        it.Won++;
                    }
                    else
                    {
                        it.Lost++;
                    }
                }
            }
            serialization.SerializeObject(VM.Obj.Statistics);
        }

        public void SaveGame()
        {
            if (VM.CurrentGame != null)
            {
                if (VM.CurrentGame.IDGame == -1)
                {
                    int id = 0;
                    if (VM.Obj != null && VM.Obj.Games != null)
                    {
                        bool ok = true;
                        while (true)
                        {
                            foreach (Game it in VM.Obj.Games)
                            {
                                if (it.IDGame == id)
                                {
                                    id++;
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok)
                            {
                                break;
                            }
                            ok = true;
                        }
                    }
                    else
                    {
                        VM.Obj.Games = new ObservableCollection<Game>();
                    }
                    VM.CurrentGame.IDGame = id;
                    VM.Obj.Games.Add(VM.CurrentGame);
                }
                else
                {
                    foreach (Game it in VM.Obj.Games)
                    {
                        if (it.IDGame == VM.CurrentGame.IDGame)
                        {
                            _ = VM.Obj.Games.Remove(it);
                            VM.Obj.Games.Add(VM.CurrentGame);
                            break;
                        }
                    }
                }
                serialization.SerializeObject(VM.Obj.Games);
            }
        }

        public void NewGame(object selectedCategory)
        {
            string originalWord = GenerateNewWord(selectedCategory as string);
            string word = "";

            foreach (char i in originalWord)
            {
                if (i == ' ')
                {
                    word += i;
                }
                else
                {
                    word += '_';
                }
            }

            int id = -1;
            if(VM.CurrentGame != null && VM.CurrentGame.Lvl != 5)
            {
                id = VM.CurrentGame.IDGame;
            }

            VM.CurrentGame = new Game
            {
                User = VM.User,
                Category = selectedCategory as string,
                Lvl = 1,
                LettersUsed = "",
                LivesLeft = 6,
                TimeLeft = 60,
                OriginalWord = originalWord,
                CurrentWord = word,
                IDGame = id
            };

            VM.IsEnabled = true;
            VM.KeyboardKeys = GenerateKeyboard(false);
            VM.Lives = GenerateLivesImages();
            VM.CurrentImage = "/Hangman;component/Resources/Hangman/6.jpg";
        }

        public void CheckLetter(object character)
        {
            char letter = Convert.ToChar(character);

            VM.CurrentGame.LettersUsed += letter;
            VM.KeyboardKeys[Convert.ToInt32(character) - 97] = new Tuple<char, bool>(letter, false);

            if (VM.CurrentGame.OriginalWord.Contains(letter) || VM.CurrentGame.OriginalWord.Contains(char.ToUpper(letter)))
            {
                string word = UpdateWord(letter);
                VM.CurrentGame.CurrentWord = word;

                if (word == VM.CurrentGame.OriginalWord)
                {
                    if (VM.CurrentGame.Lvl < 5)
                    {
                        StopTimer();
                        VM.Show(VM.CurrentGame.OriginalWord, true);

                        int lvl = VM.CurrentGame.Lvl + 1;
                        NewGame(VM.CurrentGame.Category);
                        VM.CurrentGame.Lvl = lvl;
                    }
                    else if (VM.CurrentGame.Lvl == 5)
                    {
                        UpdateSavedGames(true);
                    }
                }
            }
            else
            {
                VM.CurrentGame.LivesLeft -= 1;
                VM.Lives[VM.CurrentGame.LivesLeft] = "/Hangman;component/Resources/Icons/life_used.png";
                VM.CurrentImage = "/Hangman;component/Resources/Hangman/" + VM.CurrentGame.LivesLeft.ToString() + ".jpg";

                if (VM.CurrentGame.LivesLeft == 0)
                {
                    UpdateSavedGames(false);
                }
            }
        }

        public ObservableCollection<Tuple<char, bool>> GenerateKeyboard(bool check)
        {
            ObservableCollection<Tuple<char, bool>> list = new ObservableCollection<Tuple<char, bool>>();

            if (check)
            {
                for (int i = 97; i < 123; i++)
                {
                    if (VM.CurrentGame != null && VM.CurrentGame.LettersUsed.Contains((char)i))
                    {
                        list.Add(new Tuple<char, bool>((char)i, false));
                    }
                    else
                    {
                        list.Add(new Tuple<char, bool>((char)i, true));
                    }
                }
            }
            else
            {
                for (int i = 97; i < 123; i++)
                {
                    list.Add(new Tuple<char, bool>((char)i, false));
                }
            }
            return list;
        }

        public ObservableCollection<string> GenerateLivesImages()
        {
            ObservableCollection<string> Lives = new ObservableCollection<string>();

            for (int i = 0; i < 6; i++)
            {
                Lives.Add("/Hangman;component/Resources/Icons/life_left.png");
            }

            if (VM.CurrentGame != null)
            {
                for (int i = 5; i >= VM.CurrentGame.LivesLeft; i--)
                {
                    Lives[i] = "/Hangman;component/Resources/Icons/life_used.png";
                }
            }
            return Lives;
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            int secondsRemaining = (deadline - DateTime.Now).Seconds;
            VM.CurrentGame.TimeLeft = secondsRemaining;

            if (secondsRemaining == 0)
            {
                dispatcherTimer.Stop();
                dispatcherTimer.IsEnabled = false;
                UpdateSavedGames(false);
            }
        }

        public void StartTimer()
        {
            if (VM.CurrentGame != null)
            {
                deadline = DateTime.Now.AddSeconds(VM.CurrentGame.TimeLeft);
                dispatcherTimer.Start();
                VM.KeyboardKeys = GenerateKeyboard(true);
            }
        }

        public void StopTimer()
        {
            dispatcherTimer.Stop();
            VM.KeyboardKeys = GenerateKeyboard(false);
        }
    }
}
