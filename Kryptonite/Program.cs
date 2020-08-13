using DataLayer;
using DataLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace Kryptonite
{
    class Program
    {
        static void Main(string[] args)
        {
            //Hardcoded user
            #region
            //string password = "password";
            //byte[] salt = Hash.GenerateSalt();

            //var hashedPassword = Hash.HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), salt);

            //using (var context = new EfCoreContext())
            //{
            //    User user = new User()
            //    {
            //        Username = "thomas",
            //        Salt = Convert.ToBase64String(salt),
            //        HashedPassword = Convert.ToBase64String(hashedPassword)

            //    };

            //    context.Users.Add(user);
            //    context.SaveChanges();
            //}
            #endregion
            Login();

        }

        static void Login()
        {
            Console.WriteLine("Insert Username");
            string username = Console.ReadLine();
            Console.WriteLine("Insert Password");
            StringBuilder passwordBuilder = new StringBuilder();
            bool continueReading = true;
            char newLineChar = '\r';
            while (continueReading)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                char passwordChar = consoleKeyInfo.KeyChar;

                if (passwordChar == newLineChar)
                {
                    continueReading = false;
                }
                else
                {
                    passwordBuilder.Append(passwordChar.ToString());
                }
            }
            Console.WriteLine();
            string password = passwordBuilder.ToString();

            using (var context = new EfCoreContext())
            {

            }
        }
    }
}
