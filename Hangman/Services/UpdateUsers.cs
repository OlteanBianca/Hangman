using Hangman.Models;
using Hangman.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Hangman.Services
{
    internal class UpdateUsers
    {
        private int currentAvatar = 0;
        private SignInWindow signInWindow;
        private readonly List<string> Avatars;
        public ObservableCollection<User> Users { get; set; }


        public UpdateUsers()
        {
            Avatars = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(@"..\..\Resources\Avatars\");

            foreach (FileInfo it in directory.GetFiles())
            {
                Avatars.Add("/Hangman;component/Resources/Avatars/"+ it.Name);
            }
        }

        public void AddUser(object User)
        {
            object[] values = (object[])User;
            User user = (User)values[0];

            if (user.UserName != "")
            {
                _ = Directory.CreateDirectory(@"..\..\Users\" + user.UserName);

                File.Copy(@"..\..\Resources\Avatars\" + Path.GetFileName(user.Avatar), @"..\..\Users\" + user.UserName + @"\" + Path.GetFileName(user.Avatar));

                signInWindow = new SignInWindow();
                signInWindow.Show();
                (values[1] as NewUserWindow).Close();

                List<Statistics> list = new List<Statistics>()
                {
                    new Statistics(){Category="Cities", Won=0, Lost=0},
                    new Statistics(){Category="Countries", Won=0, Lost=0},
                    new Statistics(){Category="Mountains", Won=0, Lost=0},
                    new Statistics(){Category="Singers", Won=0, Lost=0},
                    new Statistics(){Category="Songs", Won=0, Lost=0},
                    new Statistics(){Category="All categories", Won=0, Lost=0},
                };

                Serialization ser = new Serialization(user.UserName);
                ser.SerializeObject(list);
            }
        }

        public void DeleteUser(object user)
        {
            Directory.Delete(@"..\..\Users\" + (user as User).UserName, true);
            _ = Users.Remove(user as User);
        }

        public string GetAvatar(int option)
        {
            if (option == 1)
            {
                if (currentAvatar == Avatars.Count - 1)
                {
                    currentAvatar = 0;
                }
                else
                {
                    currentAvatar++;
                }
            }
            else if (option == -1)
            {
                if (currentAvatar == 0)
                {
                    currentAvatar = Avatars.Count - 1;
                }
                else
                {
                    currentAvatar--;
                }
            }
            return Avatars[currentAvatar];
        }

        public void NextAvatar(object user)
        {
            (user as User).Avatar = GetAvatar(1);
        }

        public void PreviousAvatar(object user)
        {
            (user as User).Avatar = GetAvatar(-1);
        }
    }
}
