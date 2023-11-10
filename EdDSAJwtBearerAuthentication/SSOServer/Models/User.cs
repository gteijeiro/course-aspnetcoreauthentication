namespace SSOServer.Models
{
    public class User
    {
        public User()
        {

        }

        public User(int id, string firstName, string lastName, string email, string password, string[] roles)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Roles = roles;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}
