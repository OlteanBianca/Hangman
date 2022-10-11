using Hangman.ViewModels;
using System.Windows;

namespace Hangman.Views
{
    public partial class SignInWindow : Window
    {
        private OpenGameWindow openGameWindow;
        private NewUserWindow newUserWindow;
        private HangmanWindow hangmanWindow;

        public SignInWindow()
        {
            InitializeComponent();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            SignInVM signInVM = DataContext as SignInVM;
            if (signInVM.Obj != null && signInVM.Obj.Games.Count != 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to continue a saved game?", "Play", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    openGameWindow = new OpenGameWindow(signInVM.Obj, signInVM.SelectedUser);
                    openGameWindow.Show();
                    Close();
                    return;
                }
            }
            hangmanWindow = new HangmanWindow(null, signInVM.Obj, signInVM.SelectedUser);
            hangmanWindow.Show();
            Close();
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
            newUserWindow = new NewUserWindow();
            newUserWindow.Show();
            Close();
        }
    }
}
