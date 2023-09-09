using createEntity;
using System;
using System.Collections.Generic;
using Xunit;
using testSolution.testFile.Constant;
using DBConnectionTools;
using DBConnectionForPostgreSQL;

namespace testSolution.testFile
{
    public class PosgreSELECTテスト
    {
   
        [Fact]
        public void 全件()
        {
            //SQL定義
            string sql = "SELECT * FROM M_TEST";
            //パラメータ指定

            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            {

                //リストで返却
                var r =  conn.Select<M_TEST>(sql);

                Console.WriteLine("");
            }
        }

        [Fact]
        public void 条件検索パラメータ単体()
        {
            //SQL定義
            string sql = "SELECT * FROM M_TEST WHERE TEST_ID = @TEST_ID";
            //パラメータ指定

            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            {
                //リストで返却 
                var a = conn.Select<M_TEST>(sql, new List<CommandParameter>() { { new CommandParameter("@TEST_ID", 1) } });

                Console.WriteLine("");
            }
        }

        [Fact]
        public void 匿名セレクト()
        {
   
            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                //SQL定義
                var sql2 = "SELECT TEST_ID,name FROM M_TEST";

                //タプル型
                var bbb = conn.Select<(int TEST_ID, string name)>(sql2,null,tran);

                //匿名クラス
                var ccc = conn.Select(new { TEST_ID = default(int), name = default(string) }, sql2,null,tran);


                Console.WriteLine("");
            }

        }

        [Fact]
        public void 条件検索パラメータ複数()
        {
            //SQL定義
            string sql = "SELECT * FROM M_TEST WHERE status = @status and name = @name";
            //パラメータ指定

            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            {

                //リストで返却
                var r = conn.Select<M_TEST>(sql, new List<CommandParameter>() { { new CommandParameter("@status", true) }, { new CommandParameter("@name", "bbb") } });

                Console.WriteLine("");
            }
        }

        [Fact]
        public void 全部()
        {
            全件();
            条件検索パラメータ単体();
            匿名セレクト();
            条件検索パラメータ複数();
        }
    }
}
