using createEntity;
using DBConnectionForSQLServer;
using DBConnectionTools;
using System;
using System.Collections.Generic;
using System.Data;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class DELETEテスト
    {
        [Fact]
        public static void デリートパラメータ単体()
        {
            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_ITEM>(tran, "where ITEM_ID = @itemId", new List<CommandParameter>() { new CommandParameter("@itemId", 11, DbType.Int32) });
                    tran.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    tran.Rollback();
                }
            }
        }

        [Fact]
        public static void デリートパラメータ複数()
        {
            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_ITEM>(tran, "where ITEM_ID IN ( @itemId0,@itemId1,@itemId2)", 
                        new List<CommandParameter>() { 
                            new CommandParameter("@itemId0", 8, DbType.Int32),
                            new CommandParameter("@itemId1", 9, DbType.Int32),
                            new CommandParameter("@itemId2", 10, DbType.Int32)
                        } );
                    tran.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    tran.Rollback();
                }
            }
        }
    }
}
