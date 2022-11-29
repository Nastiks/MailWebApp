using MailTask.Server.Data;
using MailTask.Shared.Models;

namespace MailTask.Server.Services
{
    public class AccountService
    {
        private readonly AppDbContext dbContext;
        public AccountService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<User> SearchUsers(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Array.Empty<User>();
            }
            return dbContext.Users.Where(x => x.Username.ToLower().Contains(username.ToLower()));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return dbContext.Users;
        }

        public User TryFindUser(string userId)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == userId);
        }

        public User GetUser(string username)
        {
            var foundUser = dbContext.Users.FirstOrDefault(x => x.Username == username);
            if (foundUser != null)
            {
                return foundUser;
            }

            User newUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                Username = username
            };
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            return newUser;
        }
    }
}