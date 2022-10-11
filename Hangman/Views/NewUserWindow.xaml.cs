using System.Windows;

namespace Hangman.Views
{
    public partial class NewUserWindow : Window
    {
        private SignInWindow signInWindow;

        public NewUserWindow()
        {
            InitializeComponent();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            signInWindow = new SignInWindow();
            signInWindow.Show();
            Close();
        }
    }
}
