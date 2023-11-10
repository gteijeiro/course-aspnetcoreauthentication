namespace SSOServer.Models
{
    public class Repository
    {
        private static List<User> Users = new List<User>
        {
            new User(1, "Maria", "Sanders", "msanders@northwind.com", "12345", new string[]{ "Admin" }),
            new User(2, "Pedro", "Flores", "pflores@northwind.com", "12345", new string[]{ "Accountant" }),
            new User(3, "Estela", "Castillo", "ecastillos@northwind.com", "12345", new string[]{ "Seller" }),
            new User(4, "Gloria", "Ruiz", "gruiz@northwind.com", "12345", new string[]{ "Seller", "Accountant" })
        };

        public static User GetUser(string email, string password)
        {
            return Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public static User GetUser(int id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }







    }
}
