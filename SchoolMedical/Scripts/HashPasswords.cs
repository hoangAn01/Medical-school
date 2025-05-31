using BCrypt.Net;

namespace SchoolMedical.Scripts
{
    public class HashPasswords{
        public static void Main()
        {
            // Hash passwords cho các user mẫu
            var passwords = new Dictionary<string, string>
            {
                { "admin01", "Admin123!" },
                { "nurse01", "Nurse123!" },
                { "parent01", "Parent123!" },
                { "admin02", "Admin123!" },
                { "nurse02", "Nurse123!" },
                { "parent02", "Parent123!" },
                { "parent03", "Parent123!" },
                { "nurse03", "Nurse123!" }
            };

            Console.WriteLine("-- Update passwords with BCrypt hash --");
            Console.WriteLine("-- Copy and run these SQL commands in your database --");
            Console.WriteLine();

            foreach (var user in passwords)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Value);
                Console.WriteLine($"UPDATE Account SET PasswordHash = '{hashedPassword}' WHERE Username = '{user.Key}';");
            }

            Console.WriteLine();
            Console.WriteLine("-- Password mapping for reference --");
            foreach (var user in passwords)
            {
                Console.WriteLine($"Username: {user.Key} | Password: {user.Value}");
            }
        }
    }
}
