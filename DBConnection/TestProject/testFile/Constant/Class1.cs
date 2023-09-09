using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testSolution.testFile.Constant
{
    public static class Constants
    {
        /// <summary>
        /// SQLSErver
        /// </summary>
        public const string sqlServerConnectString = "Data Source=TMACHINO-PC;Initial Catalog=27training;User ID=sa;Password=1234;Encrypt=False";
        /// <summary>
        /// PostgreSQL
        /// </summary>
        public const string posgreSQLConnectString = "Server=localhost;Database=postgres;Port=16001;UserName=postgres;Password=p@ssw0rd;";
        /// <summary>
        /// MySQL
        /// </summary>
        public const string mySQLConnectString = "Server=localhost;Port=3001;Database=hoge-db;User ID=root;Password=root;";
    }
}

