using EntitiesLayer.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services {
    /// <summary>
    /// <remarks>Josip Mojzeš</remarks>
    /// </summary>
    public class AuthenticationService {

        public static User LoggedUser { get; set; }

        private string ApplicationFolder {
            get {
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\RPP2324_T15\\BidBuddy";
            }
        }

        private string SavedUserFilePath {
            get => $"{ApplicationFolder}\\SavedUser.txt";
        }

        public User LoadSavedUser() {

            try {

                if (File.Exists(SavedUserFilePath) == false) {
                    return null;
                }

                string data = File.ReadAllText(SavedUserFilePath).Trim();
                
                string[] parts = data.Split(';');
                if (parts.Length != 2) {
                    DeleteSavedUser();
                    return null;
                }

                UserService userService = new UserService();
                User user = userService.GetUserByUsername(parts[0]);

                if (user == null) {
                    DeleteSavedUser();
                    return null;
                }

                if (user.Password != parts[1]) {
                    DeleteSavedUser();
                    return null;
                }

                return user;

            } catch(Exception) {
                return null;
            }

        }

        public Task<User> LoadSavedUserAsync() {
            return Task.Run(() => {
                return LoadSavedUser();
            });
        }

        public void DeleteSavedUser() {
            if (File.Exists(SavedUserFilePath)) {
                File.Delete(SavedUserFilePath);
            }
        }

        public void SaveLoggedUser(User user) {
            Directory.CreateDirectory(ApplicationFolder);
            
            string data = $"{user.Username};{user.Password}";
            File.WriteAllText(SavedUserFilePath, data);
        }

        public void Logout()
        {
            LoggedUser = null;
            DeleteSavedUser();
        }

    }

}
