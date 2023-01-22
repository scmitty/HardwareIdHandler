namespace HwidHandler.Database
{
    public class DatabaseHandling
    {

        private Database database;

        public enum SupportedDB
        {
            MySql,
            SqlServer,
        }

        public DatabaseHandling ()
        {
            this.database = new Database();
        }

        public DatabaseHandling (SupportedDB dbType, string connectionString)
        {
            this.database = new Database();
            this.database.DatabaseType = dbType;
            this.database.ConnectionString= connectionString;
        }

        public void SetDbSetting (SupportedDB dbType,string connectionString)
        {
            this.database.DatabaseType = dbType;
            this.database.ConnectionString= connectionString;
        }

        public void ConnectToDb()
        {
            this.database.Connection();
        }

        public void DisconnectFromDb()
        {
            this.database.Disconnect();
        }

        public void SetHwIdWhitelist(string sqlQuery)
        {
            var whitelist =this.database.LoadData<HardwareId>(sqlQuery);
        }

    }
}
