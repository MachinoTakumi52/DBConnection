using createEntity;
using DBConnectionForSQLServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class INSERTテスト
    {
        [Fact]
        public void インサート()
        {
            var b = new M_TEST(1,false, "bbb");


            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Insert<M_TEST>(tran,b);
                    tran.Commit();
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
                catch (Exception e)
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

        [Fact]
        public void インサート速度検証()
        {

            /*
             * bulkcopy処理で、単体insertを500回流した時
            826
            739
            690
            662
            693

            insert文を作成して流す処理で、単体insertを500回流した時
            single
            141
            138
            178
            104
            140
            */

            //インスタンスの生成
            var sw = new Stopwatch();

            long aaa = 0;
            for (int i = 0; i < 500; i++)
            {
                //計測の開始
                sw.Start();

                using (var conn = new DataBaseConnection(Constants.connectString))
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var b = new M_TEST(i, false, "aaa");
                        conn.Insert<M_TEST>(tran, b);
                        tran.Commit();

                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                    }

                }
                sw.Stop();
                Debug.WriteLine(i + "aaa:" + sw.ElapsedMilliseconds);
                aaa += sw.ElapsedMilliseconds;
                sw.Reset();
            }

            Debug.WriteLine("total:" + aaa);
        }
    }
}
