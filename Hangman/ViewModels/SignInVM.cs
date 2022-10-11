using Hangman.Commands;
using Hangman.Models;
using Hangman.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Hangman.ViewModels
{
    internal class SignInVM : BaseVM
    {
        private readonly UpdateUsers updateUsers;
        private Serialization serialization;

        public ObjectToSerialize Obj { get; set; }
        public ObservableCollection<User> Users { get; set; }

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

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;

                if (SelectedUser != null)
                {
                    if (File.Exists(@"..\..\Users\" + SelectedUser.UserName + @"\games.xml"))
                    {
                        serialization = new Serialization(SelectedUser.UserName);
                        Obj = new ObjectToSerialize()
                        {
                            Games = serialization.DeserializeGames()
                        };
                    }
                    else
                    {
                        Obj = null;
                    }
                    IsEnabled = true;
                }
                else
                {
                    IsEnabled = false;
                }
                OnPropertyChanged("SelectedUser");
            }
        }

        private ICommand deleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null)
                {
                    deleteUserCommand = new RelayCommand(updateUsers.DeleteUser);
                }
                return deleteUserCommand;
            }
        }


        public SignInVM()
        {
            isEnabled = false;
            updateUsers = new UpdateUsers();
            Users = new ObservableCollection<User>();

            DirectoryInfo directory = new DirectoryInfo(@"..\..\Users\");
            foreach (DirectoryInfo it in directory.GetDirectories())
            {
                string avatar = Directory.GetFiles(@"..\..\Users\" + it.Name, "*.png")[0];
                
                User newUser = new User
                {
                    UserName = it.Name,
                    Avatar = "/Hangman;component/Resources/Avatars/" + Path.GetFileName(avatar)
                };
                Users.Add(newUser);
            }
            updateUsers.Users = Users;
        }
    }
}
