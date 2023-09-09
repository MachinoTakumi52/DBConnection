using createEntity;
using DBConnectionForMySQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class mySQLINSERTテスト
    {
        [Fact]
        public void インサート()
        {
            var b = new M_TEST(1, false, "aaa");


            using (var conn = new DataBaseConnection(Constants.mySQLConnectString))
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
        public void インサートテーブル名引数に()
        {
            var b = new M_TEST(2, true, "bbb");


            using (var conn = new DataBaseConnection(Constants.mySQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Insert<M_TEST>(tran, b, "M_TEST");
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
            List<M_TEST> a = new List<M_TEST>() { new M_TEST(3, false, "ccc"), new M_TEST(4, false, "ddd"), new M_TEST(5, false, "eee"), new M_TEST(6, false, "fff") };
            using (var conn = new DataBaseConnection(Constants.mySQLConnectString))
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
        public void バルクインサートテーブル名引数に()
        {
            List<M_TEST> a = new List<M_TEST>() { new M_TEST(7, false, "ggg"), new M_TEST(8, false, "hhh"), new M_TEST(9, false, "iii") };
            using (var conn = new DataBaseConnection(Constants.mySQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.BulkInsert<M_TEST>(tran, a, "M_TEST");
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }

        [Fact]
        public void 全部()
        {
            インサート();
            インサートテーブル名引数に();
            バルクインサート();
            バルクインサートテーブル名引数に();
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

                using (var conn = new DataBaseConnection(Constants.mySQLConnectString))
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
