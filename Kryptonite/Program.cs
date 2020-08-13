using DataLayer;
using DataLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

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
            //Hardcoded Encryption Key
            #region
            //var encryption = new Encrypt();
            //string key = Convert.ToBase64String(encryption.GenerateRandomNumber(32));
            //string iv = Convert.ToBase64String(encryption.GenerateRandomNumber(16));
            //using (StreamWriter sw = File.CreateText(@"C:\EncryptionKey\key.txt"))
            //{
            //    sw.WriteLine(key);
            //}
            //using (StreamWriter sw = File.CreateText(@"C:\EncryptionKey\iv.txt"))
            //{
            //    sw.WriteLine(iv);
            //}
            #endregion

            bool logintry = Login();
            if (logintry == true)
            {
                LoginSuccess();

                int pickedOption = 0;

                while (pickedOption != 3)
                {
                    Console.Clear();
                    Console.WriteLine("What do you make to do?");
                    Console.WriteLine("1. Create and encrypt a file");
                    Console.WriteLine("2. See a list of encrypted files and decrypt one.");
                    Console.WriteLine("3. Exit");

                    pickedOption = Convert.ToInt32(Console.ReadLine());

                    switch (pickedOption)
                    {
                        case 1:
                            Console.Clear();
                            EncryptFile();
                            break;
                        case 2:
                            Console.Clear();
                            DecryptFile();
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }

            }
            else
            {
                LoginFail();
            }


        }

        public static bool Login()
        {
            Console.WriteLine("Insert Username");
            string username = Console.ReadLine();

            int loginTries = 2;


            while (loginTries >= 0)
            {

                Console.Clear();
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
                    var user = context.Users
                        .Where(u => u.Username == username).FirstOrDefault();


                    byte[] usersalt = Convert.FromBase64String(user.Salt);

                    var password1 = Hash.HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), usersalt);

                    string passwordBase64 = Convert.ToBase64String(password1);




                    if (passwordBase64 == user.HashedPassword)
                    {
                        Console.WriteLine("Login Succeeded!");
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Login Failed! You have {0} attempts left.", loginTries);
                        if (loginTries > 0)
                        {
                            Console.WriteLine("Press Enter to try again.");
                        }
                        else
                        {
                            Console.WriteLine("No more attempts. Press enter.");
                        }
                        
                        Console.ReadLine();
                        loginTries--;
                    }
                }
            }
            return false;
            
        }

        public static void LoginSuccess()
        {
            Console.WriteLine("Redirecting in \n3..");
            Thread.Sleep(1000);
            Console.WriteLine("2..");
            Thread.Sleep(1000);
            Console.WriteLine("1..");
            Thread.Sleep(1000);
        }

        public static void LoginFail()
        {
            Console.WriteLine("\nToo many failed attempts. Closing in 3.. ");
            Thread.Sleep(1000);
            Console.WriteLine("2..");
            Thread.Sleep(1000);
            Console.WriteLine("1..");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public static void EncryptFile()
        {
            var encryption = new Encrypt();

            string directory = @"C:\EncryptedFiles\";

            Console.WriteLine("Type a name for your file: \n");
            string fileName = Console.ReadLine();

            Console.WriteLine("Type a message you want to be encrypted into a file: \n");
            string message = Console.ReadLine();


            byte[] key = Convert.FromBase64String(File.ReadAllText(@"C:\EncryptionKey\key.txt"));
            byte[] iv = Convert.FromBase64String(File.ReadAllText(@"C:\EncryptionKey\iv.txt"));

            string encryptedMessage = Convert.ToBase64String(encryption.EncryptText(Encoding.UTF8.GetBytes(message), key, iv));

            using (StreamWriter sw = File.CreateText(directory+fileName+".txt"))
            {
                sw.WriteLine(encryptedMessage);
            }

            Console.WriteLine("File Created.");
            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
            
            
        }

        public static void DecryptFile()
        {
            var encryption = new Encrypt();

            Console.WriteLine("Pick a file to decrypt:\n");

            List<string> files = Directory.GetFiles(@"C:\EncryptedFiles\").ToList();

            int number = 1;

            foreach (string file in files)
            {
                Console.WriteLine("{0}. {1}", number, file);
                number++;
            }
            Console.WriteLine();
            int numberpicked = Convert.ToInt32(Console.ReadLine());

            string filetext = File.ReadAllText(files[numberpicked-1]);

            byte[] filebytes = Convert.FromBase64String(filetext);

            byte[] key = Convert.FromBase64String(File.ReadAllText(@"C:\EncryptionKey\key.txt"));
            byte[] iv = Convert.FromBase64String(File.ReadAllText(@"C:\EncryptionKey\iv.txt"));

            string decryptedfile = Encoding.UTF8.GetString(encryption.DecryptText(filebytes, key, iv));

            Console.WriteLine("\nThe decrypted message: \n");
            Console.WriteLine(decryptedfile);

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();


        }

    }
}
