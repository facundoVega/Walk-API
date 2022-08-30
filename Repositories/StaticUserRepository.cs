using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            //new User()
            //{
            //    FirstName = "Read only", LastName = "User", EmailAdress = "readonly@user.com",
            //    Id = Guid.NewGuid(), UserName = "readonly@user.com", Password = "Readonly@user",
            //    Roles = new List<Role> { "reader"}
            //},
            //new User()
            //{
            //    FirstName = "Read Write", LastName = "user", EmailAdress = "readwrite@user.com",
            //    Id = Guid.NewGuid(), UserName = "readwirter@user.com", Password = "ReadWrite@user",
            //    Roles = new List<string>{"reader", "writer"}
            //}
        };
        public async Task<User> AuthenticateAsync(string username, string password) =>
            Users.Find(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase)
            && x.Password == password);

         
    }
}
