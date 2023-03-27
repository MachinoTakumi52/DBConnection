using createEntity;
using DBConnectionForSQLServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class INSERTテスト
    {
        [Fact]
        public void インサート()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var b = new M_ITEM(14, "14", "ぐぐぐぐ", 111111, 1, 1, 8, 1,DateTime.Now,"aaa", "aaa", DateTime.Now,"bbb", "bbb");


            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Insert<M_ITEM>(tran,b);
                    tran.Commit();
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    Debug.WriteLine($"　{ts}");

                }
                catch (Exception)
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

        [Fact]
        public void ビットインサート()
        {
            var b = new M_TEST(1,true,null);


            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Insert<M_TEST>(tran, b);
                    tran.Commit();

                }
                catch (Exception)
                {
                    tran.Rollback();
                }

            }
        }

        [Fact]
        public void バルクインサートnull許容プロパティあり()
        {
            List<M_TEST> a = new List<M_TEST>() { new M_TEST(2,true,null), new M_TEST(3, true, "aaaa"), new M_TEST(4, false, null), new M_TEST(5, true, "gefdbdsb"), new M_TEST(6, false, null) };
            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.BulkInsert<M_TEST>(tran, a);
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
