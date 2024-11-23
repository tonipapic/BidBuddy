using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class UserRoleService
    {
        public UserRoleService() { }
        /// <summary>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <returns>List of <see cref="UserRole"/></returns>
        public List<UserRole> GetUserRoles()
        {
            using(var repo = new UserRoleRepository())
            {
                return repo.GetAll().ToList();
            }
        }
    }
}
