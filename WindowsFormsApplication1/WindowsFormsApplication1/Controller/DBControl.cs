using System;
using MySql.Data.MySqlClient;
using NLog;

namespace WindowsFormsApplication1.Controller
{
    class DbControl
    {
        private MySqlConnection _connection;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private void Connect()
        {
            try
            {
                _logger.Debug("Trying to connect to the database");
                _connection = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=Kurs");
                _connection.Open();
                _logger.Debug("The connection is successfully");
            }
            catch (Exception e)
            {
                _logger.Error("Connection error " + e);
            }
        }

        protected MySqlCommand Query(string query)
        {
            Connect();
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            return command;
        }

        public MySqlDataReader ReadFrom(string tableName)
        {
            return  Query("SELECT * FROM `" + tableName + "`").ExecuteReader();;
        }
    }
}
