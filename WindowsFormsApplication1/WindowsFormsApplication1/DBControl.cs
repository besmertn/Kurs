using System;
using MySql.Data.MySqlClient;
using NLog;

namespace WindowsFormsApplication1
{
    class DbControl
    {
        private MySqlConnection _connection;
        private MySqlCommand _command;
        private MySqlDataReader _reader;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private void Connect() {
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
            _command = new MySqlCommand(query,_connection);
            _command.ExecuteNonQuery();           
            return _command;
            
        }

        public MySqlDataReader ReadFrom(string tableName)
        {
            _reader = Query("SELECT * FROM `" + tableName + "`").ExecuteReader();
            return _reader;
        }


    }
}
