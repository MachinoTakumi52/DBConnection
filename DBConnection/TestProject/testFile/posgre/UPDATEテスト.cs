using createEntity;
using DBConnectionForPostgreSQL;
using DBConnectionTools;
using System;
using System.Collections.Generic;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class PosgreUPDATEテスト
    {
        [Fact]
        public void アップデート()
        {

            var builder = new EntityModifyBuilder<M_TEST>();



            builder.Add<string>(x => x.name, "ああああ");
            builder.Add<bool>(x => x.status, true);

            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {

                    conn.Update<M_TEST>(tran, builder, "WHERE TEST_ID = 2");
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }

        [Fact]
        public void アップデートテーブル名引数に()
        {

            var builder = new EntityModifyBuilder<M_TEST>();



            builder.Add<string>(x => x.name, "いいいい");
            builder.Add<bool>(x => x.status, true);

            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {

                    conn.Update<M_TEST>(tran, builder, "WHERE TEST_ID = 4", null, "M_TEST"); 
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }


        [Fact]
        public void 　アップデートパラメータ有()
        {

            var builder = new EntityModifyBuilder<M_TEST>();



            builder.Add<string>(x => x.name,"うううう");
            builder.Add<bool>(x => x.status ,true);

            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {

                    conn.Update<M_TEST>(tran, builder, "WHERE TEST_ID = @testId", new List<CommandParameter>() { { new CommandParameter("@testId", 6)} });
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }

        [Fact]
        public void アップデートパラメータ有テーブル名引数に()
        {

            var builder = new EntityModifyBuilder<M_TEST>();



            builder.Add<string>(x => x.name, "ええええ");
            builder.Add<bool>(x => x.status, true);

            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {

                    conn.Update<M_TEST>(tran, builder, "WHERE TEST_ID = @testId", new List<CommandParameter>() { { new CommandParameter("@testId", 8) } },"M_TEST");
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }

        [Fact]
        public void アップデートパラメータ複数有テーブル名引数に()
        {

            var builder = new EntityModifyBuilder<M_TEST>();



            builder.Add<string>(x => x.name, "おおおお");
            builder.Add<bool>(x => x.status, false);

            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {

                    conn.Update<M_TEST>(tran, builder, "WHERE TEST_ID IN (@testId1,@testId2,@testId3,@testId4,@testId5)", new List<CommandParameter>() { { new CommandParameter("@testId1", 1) }, { new CommandParameter("@testId2", 3) }, { new CommandParameter("@testId3", 5) }, { new CommandParameter("@testId4", 7) }, { new CommandParameter("@testId5", 9) } }, "M_TEST");
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
            アップデート();
            アップデートテーブル名引数に();
            アップデートパラメータ有();
            アップデートパラメータ有テーブル名引数に();
            アップデートパラメータ複数有テーブル名引数に();
        }

        [Fact]
        public void アップデートオール()
        {

            var builder = new EntityModifyBuilder<M_TEST>();



            builder.Add<string>(x => x.name, "あいう");
            builder.Add<bool>(x => x.status, true);

            using (var conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {

                    conn.Update<M_TEST>(tran, builder, null, null, "M_TEST");
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
