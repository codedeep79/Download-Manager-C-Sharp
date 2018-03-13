namespace Download_Manager_C_Sharp
{
    static class App
    {
        static Database db;
        public static Database DB
        {
            get
            {
                if (db == null)
                    db = new Database();
                return db;
            }
        }
    }
}
