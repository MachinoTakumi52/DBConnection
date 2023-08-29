using createEntity;

using System;
using System.Collections.Generic;
using Xunit;
using testSolution.testFile.Constant;
using DBConnectionTools;
using DBConnectionForSQLServer;

namespace testSolution.testFile
{
    public class SELECTテスト
    {
   
        [Fact]
        public void 全件()
        {
            //SQL定義
            string sql = "SELECT * FROM M_ITEM";
            //パラメータ指定

            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            {

                //リストで返却
                var r =  conn.Select<M_ITEM>(sql);

                Console.WriteLine("");
            }
        }

        [Fact]
        public void 条件検索パラメータ単体()
        {
            //SQL定義
            string sql = "SELECT * FROM M_ITEM WHERE ITEM_ID = @ITEM_ID";
            //パラメータ指定

            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            {
                //リストで返却 
                var r = conn.Select<M_ITEM>(sql, new List<CommandParameter>() { { new CommandParameter("@ITEM_ID", 1, System.Data.DbType.Int64) } });
                var a = conn.Select<M_ITEM>(sql, new List<CommandParameter>() { { new CommandParameter("@ITEM_ID", 1) } });

                Console.WriteLine("");
            }
        }

        [Fact]
        public void 匿名セレクト()
        {
   
            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                //SQL定義
                var sql2 = "SELECT ITEM_ID,ITEM_CD FROM M_ITEM";

                //タプル型
                var bbb = conn.Select<(int ITEM_ID, string ITEM_CD)>(sql2,null,tran);

                //匿名クラス
                var ccc = conn.Select(new { ITEM_ID = default(int), ITEM_CD = default(string) }, sql2,null,tran);


                Console.WriteLine("");
            }

        }

        [Fact]
        public void 条件検索パラメータ複数()
        {
            //SQL定義
            string sql = "SELECT * FROM M_ITEM WHERE ITEM_ID = @ITEM_ID OR ITEM_CD = @ITEM_CD";
            //パラメータ指定

            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            {

                //リストで返却
                var r = conn.Select<M_ITEM>(sql, new List<CommandParameter>() { { new CommandParameter("@ITEM_ID", 1, System.Data.DbType.Int32) }, { new CommandParameter("@ITEM_CD", "2") } });

                Console.WriteLine("");
            }
        }

    }
}
