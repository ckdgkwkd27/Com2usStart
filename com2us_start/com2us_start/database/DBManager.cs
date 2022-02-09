using Dapper;
using MySqlConnector;
using System;
using System.Linq;

namespace com2us_start
{
    public class DBManager
    {
        public MySqlConnection? _connection { get; set; }

        public DBManager()
        {
            _connection = null;
        }

        public MySqlConnection? connectionFactory(string _id, string _pwd, string _db, string _port = "3306",
            string _host = "127.0.0.1")
        {
            string host = "host=" + _host + "; ";
            string port = "port=" + _port + "; ";
            string id = "user id=" + _id + "; ";
            string pwd = "password=" + _pwd + "; ";
            string db = "database=" + _db + "; ";

            string ConnString = host + port + id + pwd + db;
            var connection = new MySqlConnection(ConnString);

            try
            {
                connection.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            _connection = connection;
            return connection;
        }

        public List<Member> GetAllMember()
        {
            List<Member> list = new List<Member>();

            if (_connection == null)
            {
                Console.WriteLine("ERROR: Call ConnectionFactory First()");
                return list;
            }

            var _memberList = _connection.Query<Member>("SELECT ID,Password FROM account");
            list = _memberList.ToList();

            return list;
        }

        public void InsertSingleMember(string _id, string _pwd)
        {
            if (_connection == null)
            {
                Console.WriteLine("ERROR: Call ConnectionFactory First()");
                return;
            }
            if (MemberCheck(_id, _pwd))
                return;

            var affect_rows = _connection.Execute("INSERT account(ID, Password) VALUES (@id, @pwd)",
                new { id = _id, pwd = _pwd });
        }

        public bool MemberCheck(string _id, string _pwd)
        {
            if (_connection == null)
            {
                Console.WriteLine("ERROR: Call ConnectionFactory First()");
                return false;
            }

            var _memberList = _connection.Query<Member>(
                "SELECT ID, Password FROM account WHERE ID=@id and Password=@pwd",
                new { id = _id, pwd = _pwd });

            if (_memberList.ToList().Count > 0)
                return true;
            return false;
        }
    }
}
