using createEntity;
using DBConnectionForSQLServer;
using System;
using System.Collections.Generic;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class INSERTテスト
    {
        [Fact]
        public void インサート()
        {
           var b = new M_ITEM(8, "8", "ぐぐぐぐ", 111111, 1, 1, 8, 1,DateTime.Now,"aaa", "aaa", DateTime.Now,"bbb", "bbb");


            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Insert<M_ITEM>(tran,b);
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }

        [Fact]
        public void バルクインサート()
        {
            List<M_ITEM> a = new List<M_ITEM>() { new M_ITEM(8, "8", "ぐぐぐぐ", 111111, 1, 1, 8, 1,DateTime.Now,"aaa", "aaa", DateTime.Now,"bbb", "bbb"),
                                                              new M_ITEM(9, "9", "grtgtrgrt", 111111, 1, 1, 8, 1,DateTime.Now,"aaa", "aaa", DateTime.Now,"bbb", "bbb"),
                                                              new M_ITEM(10, "10", "gtrgtrr", 111111, 1, 1, 8, 1,DateTime.Now,"aaa", "aaa", DateTime.Now,"bbb", "bbb") };
            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.BulkInsert<M_ITEM>(tran,a);
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }
    }
}
