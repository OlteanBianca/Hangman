using Hangman.Models;
using Hangman.ViewModels;
using System.Windows;

namespace Hangman.Views
{
    public partial class OpenGameWindow : Window
    {
        private SignInWindow signInWindow;
        private HangmanWindow hangmanWindow;

        public OpenGameWindow(ObjectToSerialize obj, User selectedUser)
        {
            InitializeComponent();
            DataContext = new OpenGameVM(selectedUser, obj);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            signInWindow = new SignInWindow();
            signInWindow.Show();
            Close();
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            OpenGameVM openGameVM = DataContext as OpenGameVM;
            hangmanWindow = new HangmanWindow(openGameVM.SelectedGame, openGameVM.Obj, openGameVM.User);
            hangmanWindow.Show();
            Close();
        }
    }
}
