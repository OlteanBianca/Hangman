using Hangman.Commands;
using Hangman.Models;
using Hangman.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Hangman.ViewModels
{
    internal class HangmanVM : BaseVM
    {
        private readonly BusinessLogic businessLogic;

        public User User { get; set; }
        public ObjectToSerialize Obj { get; set; }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        private Game currentGame;
        public Game CurrentGame
        {
            get => currentGame;
            set
            {
                currentGame = value;
                OnPropertyChanged("CurrentGame");
            }
        }

        private string currentImage;
        public string CurrentImage
        {
            get => currentImage ?? "/Hangman;component/Resources/Hangman/6.jpg";
            set
            {
                currentImage = value;
                OnPropertyChanged("CurrentImage");
            }
        }

        private string selectedCategory;
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        private ObservableCollection<string> lives;
        public ObservableCollection<string> Lives
        {
            get => lives;
            set
            {
                lives = value;
                OnPropertyChanged("lives");
            }
        }

        private ObservableCollection<Tuple<char, bool>> keyboardKeys;
        public ObservableCollection<Tuple<char, bool>> KeyboardKeys
        {
            get => keyboardKeys;
            set
            {
                keyboardKeys = value;
                OnPropertyChanged("KeyboardKeys");
            }
        }

        private ICommand keyboardCommand;
        public ICommand KeyboardCommand
        {
            get
            {
                if (keyboardCommand == null)
                {
                    keyboardCommand = new RelayCommand(businessLogic.CheckLetter);
                }
                return keyboardCommand;
            }
        }

        private ICommand newGameCommand;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                {
                    newGameCommand = new RelayCommand(businessLogic.NewGame);
                }
                return newGameCommand;
            }
        }

        private ICommand saveGameCommand;
        public ICommand SaveGameCommand
        {
            get
            {
                if (saveGameCommand == null)
                {
                    saveGameCommand = new RelayCommand(businessLogic.SaveGame);
                }
                return saveGameCommand;
            }
        }

        private ICommand startTimerCommand;
        public ICommand StartTimerCommand
        {
            get
            {
                if (startTimerCommand == null)
                {
                    startTimerCommand = new RelayCommand(businessLogic.StartTimer);
                }
                return startTimerCommand;
            }
        }

        private ICommand stopTimerCommand;
        public ICommand StopTimerCommand
        {
            get
            {
                if (stopTimerCommand == null)
                {
                    stopTimerCommand = new RelayCommand(businessLogic.StopTimer);
                }
                return stopTimerCommand;
            }
        }


        public HangmanVM(Game selectedGame, ObjectToSerialize obj, User selectedUser)
        {
            Obj = obj ?? new ObjectToSerialize();

            User = selectedUser;
            CurrentGame = selectedGame;
            businessLogic = new BusinessLogic(this);

            Lives = businessLogic.GenerateLivesImages();
            KeyboardKeys = businessLogic.GenerateKeyboard(false);

            if (CurrentGame != null)
            {
                if (CurrentGame.LivesLeft >= 0)
                {
                    CurrentImage = "/Hangman;component/Resources/Hangman/" + currentGame.LivesLeft.ToString() + ".jpg";
                }
                IsEnabled = true;
            }
        }

        public void Show(string word, bool result)
        {
            _ = result
                ? CurrentGame.Lvl == 5
                    ? MessageBox.Show("Congratulations you won the game!\nThe last word was: " + word, "Result", MessageBoxButton.OK, MessageBoxImage.Information)
                    : MessageBox.Show("Congratulations you guessed the word correctly!\nThe word was: " + word, "Result", MessageBoxButton.OK, MessageBoxImage.Information)
                : MessageBox.Show("Unfortunately you lost the game! The word was!\n" + word, "Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
