using EntitiesLayer.Entities;
using System.Linq;

namespace DataAccessLayer.Repositories {
    /// <summary>
    /// Repository for accessing user data.
    /// <remarks>Dorian Rušak</remarks>
    /// </summary>
    public class UserRepository : Repository<User>
    {
        public UserRepository() : base() { }

        /// <summary>
        /// Retrieves all users in the database along with their roles and reviews.
        /// </summary>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>A query that returns all users.</returns>
        public override IQueryable<User> GetAll()
        {
            var query = from e in Entities.Include("UserRole").Include("Reviews")
                        select e;
            return query;
        }

        /// <summary>
        /// Retrieves a user based on their username
        /// </summary>
        /// <param name="username"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>The user with the corresponding username or null if not found.</returns>
        public User GetUserByUsername(string username)
        {
            return Entities.FirstOrDefault(u => u.Username == username);
        }

        /// <summary>
        /// Retrieves a user based on their email address.
        /// </summary>
        /// <param name="email"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>The user with the corresponding email address or null if not found.</returns>
        public User GetUserByEmail(string email)
        {
            return Entities.FirstOrDefault(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a user based on their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns>The user with the corresponding ID or null if not found.</returns>
        public User GetById(int userId)
        {
            return Entities.FirstOrDefault(u => u.UserId == userId);
        }

        /// <summary>
        /// Updates user information in the database.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="save"></param>
        /// <remarks>Dorian Rušak</remarks>
        /// <returns></returns>
        public override int Update(User entity, bool save = true)
        {
            var user = Entities.SingleOrDefault(u => u.UserId == entity.UserId);

            if (user != null)
            {
                user.Username = entity.Username;
                user.Email = entity.Email;
                user.Password = entity.Password;
                user.Salt = entity.Salt;


                if (save) return SaveChanges();
            }

            return 0;
        }
    }
}
