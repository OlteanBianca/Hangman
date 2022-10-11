using Hangman.Models;

namespace Hangman.ViewModels
{
    internal class OpenGameVM : BaseVM
    {
        public ObjectToSerialize Obj { get; set; }
        public User User { get; set; }
        public Game SelectedGame { get; set; }


        public OpenGameVM(User selectedUser, ObjectToSerialize obj)
        {
            User = selectedUser;
            Obj = obj;
        }
    }
}
