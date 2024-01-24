using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlanner
{
    internal class Constants
    {
        public const string DBFile = "myApp.db";

        public const SQLite.SQLiteOpenFlags Flags = //All of this is from microsoft docs directly
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DBFile);
    }
}
