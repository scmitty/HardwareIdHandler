using static HwidHandler.Database.DatabaseHandling;

namespace HwidHandler.Database
{
    internal class Database
    {
        public string DatabaseName { get; set; }    

        public string ConnectionString { get; set; }

        public SupportedDB DatabaseType {get; set; } 


        public bool Connection()
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SaveData(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> LoadData<T>(string sqlQuery)
        {
            throw new NotImplementedException();
        }
    }
}
