using System.Linq;
using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;

namespace GitServer.Services
{
    public class UserService
    {
        private readonly IRepository<User> _user;

        public UserService(IRepository<User> user)
        {
            _user = user;
        }

        public User GetUserById(long id)
        {
            return _user.List(user => user.ID == id).FirstOrDefault();
        }

        public User GetUserByName(string name)
            => _user.List(u => u.Name == name).FirstOrDefault();

        public void Save(User newUser)
        {
            _user.Edit(newUser);
        }
    }
}