using BusinessLogicLayer.Exceptions;
using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// Service class for managing user-related operations.
    /// <remarks>Dorian Rušak</remarks>
    /// </summary>
    public class UserService
    {

        public UserService() { }

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>List of User objects.</returns>
        public List<User> GetAllUsers()
        {
            using(var repo = new UserRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        /// <summary>
        /// Retrieves user profile information by user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>User object containing profile information.</returns>
        public User GetUserProfile(int userId)
        {
            using (var repo = new UserRepository())
            {
                return repo.GetById(userId);
            }
        }

        /// <summary>
        /// Updates the user profile with the provided information.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="iban"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <exception cref="UserNotFoundException"></exception>
        public void UpdateUserProfile(int userId, string firstName, string lastName, string email, string phoneNumber, string iban)
        {
            using (var repo = new UserRepository())
            {
                var user = repo.GetById(userId);

                if (user == null)
                {
                    throw new UserNotFoundException("Korisnik nije pronađen");
                }

                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.PhoneNumber = phoneNumber;
                user.IBAN = iban;

                repo.Update(user);
            }
        }

        public void UpdateUserProfile(int userId, bool verified, int roleId, string banMessage)
        {
            using (var repo = new UserRepository())
            {
                var user = repo.GetById(userId);

                if (user == null)
                {
                    throw new UserNotFoundException("Korisnik nije pronađen");
                }

                user.IsVerified = verified;
                user.UserRoleId = roleId;
                user.BanMessage = banMessage;
                repo.Update(user);
            }
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>User object with the corresponding username or null if not found./returns>
        public User GetUserByUsername(string username)
        {
            using(var repo = new UserRepository())
            {
                return repo.GetUserByUsername(username);            
            }
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>User object with the corresponding ID or null if not found</returns>
        public User GetUserById(int userId) {
            using (var repo = new UserRepository()) {
                return repo.GetById(userId);
            }
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>User object with the corresponding email address or null if not found.</returns>
        public User GetUserByEmail(string email)
        {
            using( var repo = new UserRepository())
            {
                return repo.GetUserByEmail(email);
            }
        }

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="birthDate"></param>
        /// <param name="iban"></param>
        /// <exception cref="UsernameTakenException"></exception>
        /// <exception cref="EmailTakenException"></exception>
        /// <remarks>Dorian Rušak</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void RegisterUser(string username, string email, string password, string phoneNumber, string firstName, string lastName, DateTime birthDate, string iban)
        {
            if (IsUsernameTaken(username))
            {
                throw new UsernameTakenException("Korisničko ime već postoji");
            }

            if (IsEmailTaken(email))
            {
                throw new EmailTakenException("Email već postoji");
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Obavezna polja ne smiju bit prazna!");
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                phoneNumber = "0000";
            }

            using (var repo = new UserRepository())
            {
                string salt = GenerateSalt();
                string saltedPassword = password + salt;
                string hashedPassword = HashPassword(saltedPassword);
                User newUser = new User
                {
                    Username = username,
                    Email = email,
                    Password = hashedPassword,
                    Salt = salt,
                    PhoneNumber = phoneNumber,
                    UserRoleId = UserRole.Basic,
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate,
                    IBAN = iban,
                };

                repo.Add(newUser);
            }
        }

        /// <summary>
        /// Logs in a user with the provided username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>User object if login is successful, throws exceptions otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="InvalidPasswordException"></exception>
        /// <remarks>Dorian Rušak</remarks>
        public User LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Korisničko ime i lozinka moraju biti uneseni");  
            }

            User existingUser = GetUserByUsername(username);

            if(existingUser == null)
            {
                throw new UserNotFoundException("Krivo korisničko ime");
            }

            if (existingUser.UserRoleId == UserRole.Banned)
            {
                throw new ApplicationException("Banovani ste! " + existingUser.BanMessage);
            }

            if (existingUser.UserRoleId == UserRole.Unactivated)
            {
                throw new ApplicationException("Vaš račun nije još aktiviran!");
            }

            string saltedPassword = password + existingUser.Salt;
            string hashedPassword = HashPassword(saltedPassword);

            if (hashedPassword != existingUser.Password)
            {
                throw new InvalidPasswordException("Netočna lozinka!");
            }

            return existingUser;
        }

        /// <summary>
        /// Hashes the provided password using SHA-256 algorithm.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hashed password as a base64-encoded string.</returns>
        /// <remarks>Dorian Rušak</remarks>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Generates a random salt for password hashing.
        /// </summary>
        /// <returns>Random salt as a base64-encoded string.</returns>
        /// <remarks>Dorian Rušak</remarks>
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            return salt;
        }

        /// <summary>
        /// Checks if a username is already taken by another user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True if the username is already taken; otherwise, false.</returns>
        /// <remarks>Dorian Rušak</remarks>
        public bool IsUsernameTaken(string username)
        {
            using(var repo = new UserRepository())
            {
                return repo.GetUserByUsername(username) != null;
            }
        }

        /// <summary>
        /// Checks if an email address is already registered by another user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if the email address is already taken; otherwise, false.</returns>
        /// <remarks>Dorian Rušak</remarks>
        public bool IsEmailTaken(string email)
        {
            using (var repo = new UserRepository())
            {
                return repo.GetUserByEmail(email) != null;
            }
        }
    }
}
