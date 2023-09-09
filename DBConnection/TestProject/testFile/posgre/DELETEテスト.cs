using createEntity;
using DBConnectionForPostgreSQL;
using DBConnectionTools;
using System;
using System.Collections.Generic;
using System.Data;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class PosgreDELETEテスト
    {
        [Fact]
        public static void デリート()
        {
            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_TEST>(tran, "where TEST_ID = 9");
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
        }

        [Fact]
        public static void デリートテーブル名を引数に()
        {
            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_TEST>(tran, "where TEST_ID = 8",null,"M_TEST");
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
        }

        [Fact]
        public static void デリートパラメータ有()
        {
            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_TEST>(tran, "where TEST_ID = @testId", new List<CommandParameter>() { new CommandParameter("@testId", 7) });
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
        }

        [Fact]
        public static void デリートパラメータ有テーブル名を引数に()
        {
            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_TEST>(tran, "where TEST_ID = @testId", new List<CommandParameter>() { new CommandParameter("@testId", 6) },"M_TEST");
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
        }

        [Fact]
        public static void デリートパラメータ複数有テーブル名を引数に()
        {
            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_TEST>(tran, "where TEST_ID IN ( @testId0,@testId1,@testId2)", 
                        new List<CommandParameter>() { 
                            new CommandParameter("@testId0", 5),
                            new CommandParameter("@testId1", 4),
                            new CommandParameter("@testId2", 3)
                        } );
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
        }

        [Fact]
        public static void 全部()
        {
            デリート();
            デリートテーブル名を引数に();
            デリートパラメータ有();
            デリートパラメータ有テーブル名を引数に();
            デリートパラメータ複数有テーブル名を引数に();
        }

            [Fact]
        public static void デリートオール()
        {
            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Delete<M_TEST>(tran);
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
