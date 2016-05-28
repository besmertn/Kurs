using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using NLog;

namespace WindowsFormsApplication1.Controller
{
    class UsersDbControl : DbControl
    {
        protected MD5 Md5Hash = MD5.Create();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public int CashRegisterNumber { set; get; }

        public void UpdateUsersDb()
        {
            const string path = @"usersDB.txt";

            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    int inx = s.IndexOf("*", StringComparison.Ordinal);
                    int inx2 = s.IndexOf(";", StringComparison.Ordinal);
                    int len = s.Length;
                    string login = s.Substring(0, inx);
                    string hash = s.Substring(inx + 1, inx2 - inx - 1);
                    string cashregisternumber = s.Substring(inx2 + 1, len - inx2 - 1);
                    try
                    {
                        Query(
                            "INSERT INTO `Kurs`.`users` (`user_id`, `login`, `password`,`cashregisternumber`) VALUES (NULL,'" +
                            login + "', '" + hash + "', " + cashregisternumber + ")");
                        Query("INSERT INTO `checkcounter`(`cashregisternumber`, `checkcounter`) VALUES(" +
                              cashregisternumber + ", 0)");
                    }
                    catch
                    {
                        Query("UPDATE `Kurs`.`users` SET `password`='" + hash + "' WHERE `login` = '" + login + "';");
                    }
                }
            }
        }


        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        protected static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            return false;
        }

        public void AddUser(User user)
        {
            string path = @"usersDB.txt";

            string hash;
            using (Md5Hash)
            {
                hash = GetMd5Hash(Md5Hash, user.Password);
            }
            string result = user.Login + "*" + hash + ";" + user.CashRegisterNumber;
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(result);
            }
        }

        public Boolean AuthorizeCheck(string login, string password)
        {
            _logger.Info(login);
            _logger.Info(password);
            MySqlDataReader reader = ReadFrom("users");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetValue(1).ToString() == login &&
                        VerifyMd5Hash(Md5Hash, password, reader.GetString("password")))
                    {
                        CashRegisterNumber = reader.GetInt32("cashregisternumber");
                        _logger.Info(true);
                        return true;
                    }
                }
            }
            reader.Close();
            return false;
        }
    }
}
