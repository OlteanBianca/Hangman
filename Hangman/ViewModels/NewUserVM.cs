using Hangman.Commands;
using Hangman.Models;
using Hangman.Services;
using System.Windows.Input;

namespace Hangman.ViewModels
{
    internal class NewUserVM : BaseVM
    {
        private readonly UpdateUsers updateUsers;

        private User newUser;
        public User NewUser
        {
            get => newUser;
            set
            {
                newUser = value;
                OnPropertyChanged("NewUser");
            }
        }

        private ICommand addUserCommand;
        public ICommand AddUserCommand
        {
            get
            {
                if (addUserCommand == null)
                {
                    addUserCommand = new RelayCommand(updateUsers.AddUser);
                }
                return addUserCommand;
            }
        }

        private ICommand nextAvatarCommand;
        public ICommand NextAvatarCommand
        {
            get
            {
                if (nextAvatarCommand == null)
                {
                    nextAvatarCommand = new RelayCommand(updateUsers.NextAvatar);
                }
                return nextAvatarCommand;
            }
        }

        private ICommand previousAvatarCommand;
        public ICommand PreviousAvatarCommand
        {
            get
            {
                if (previousAvatarCommand == null)
                {
                    previousAvatarCommand = new RelayCommand(updateUsers.PreviousAvatar);
                }
                return previousAvatarCommand;
            }
        }


        public NewUserVM()
        {
            updateUsers = new UpdateUsers();
            NewUser = new User
            {
                UserName = "",
                Avatar = updateUsers.GetAvatar(0)
            };
        }
    }
}
