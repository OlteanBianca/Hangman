using Hangman.Models;
using Hangman.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Hangman.Views
{
    public partial class HangmanWindow : Window
    {
        private MenuItem categoryChecked;
        private SignInWindow signInWindow;
        private HelpWindow helpWindow;
        private OpenGameWindow openGameWindow;
        private StatisticsWindow statistics;

        public HangmanWindow(Game selectedGame, ObjectToSerialize obj, User selectedUser)
        {
            InitializeComponent();
            DataContext = new HangmanVM(selectedGame, obj, selectedUser);

            all.IsChecked = true;
            categoryChecked = all;
        }

        private void AboutClicked(object sender, RoutedEventArgs e)
        {
            helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void OpenGameClicked(object sender, RoutedEventArgs e)
        {
            openGameWindow = new OpenGameWindow((DataContext as HangmanVM).Obj, (DataContext as HangmanVM).User);
            openGameWindow.Show();
            Close();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            signInWindow = new SignInWindow();
            signInWindow.Show();
            Close();
        }

        private void StatisticsClicked(object sender, RoutedEventArgs e)
        {
            statistics = new StatisticsWindow();
            statistics.Show();
        }

        private void CategoryChecked(object sender, RoutedEventArgs e)
        {
            if (categoryChecked != null)
            {
                categoryChecked.IsChecked = false;
            }
            categoryChecked = e.Source as MenuItem;
            (DataContext as HangmanVM).SelectedCategory = categoryChecked.Header.ToString();
        }
    }
}
