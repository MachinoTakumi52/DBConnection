using createEntity;
using System;
using System.Collections.Generic;
using Xunit;
using testSolution.testFile.Constant;
using DBConnectionTools;
using DBConnectionForPostgreSQL;

namespace testSolution.testFile
{
    public class PosgreSELECT�e�X�g
    {
   
        [Fact]
        public void �S��()
        {
            //SQL��`
            string sql = "SELECT * FROM M_TEST";
            //�p�����[�^�w��

            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            {

                //���X�g�ŕԋp
                var r =  conn.Select<M_TEST>(sql);

                Console.WriteLine("");
            }
        }

        [Fact]
        public void ���������p�����[�^�P��()
        {
            //SQL��`
            string sql = "SELECT * FROM M_TEST WHERE TEST_ID = @TEST_ID";
            //�p�����[�^�w��

            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            {
                //���X�g�ŕԋp 
                var a = conn.Select<M_TEST>(sql, new List<CommandParameter>() { { new CommandParameter("@TEST_ID", 1) } });

                Console.WriteLine("");
            }
        }

        [Fact]
        public void �����Z���N�g()
        {
   
            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            using (var tran = conn.BeginTransaction())
            {
                //SQL��`
                var sql2 = "SELECT TEST_ID,name FROM M_TEST";

                //�^�v���^
                var bbb = conn.Select<(int TEST_ID, string name)>(sql2,null,tran);

                //�����N���X
                var ccc = conn.Select(new { TEST_ID = default(int), name = default(string) }, sql2,null,tran);


                Console.WriteLine("");
            }

        }

        [Fact]
        public void ���������p�����[�^����()
        {
            //SQL��`
            string sql = "SELECT * FROM M_TEST WHERE status = @status and name = @name";
            //�p�����[�^�w��

            using (DataBaseConnection conn = new DataBaseConnection(Constants.posgreSQLConnectString))
            {

                //���X�g�ŕԋp
                var r = conn.Select<M_TEST>(sql, new List<CommandParameter>() { { new CommandParameter("@status", true) }, { new CommandParameter("@name", "bbb") } });

                Console.WriteLine("");
            }
        }

        [Fact]
        public void �S��()
        {
            �S��();
            ���������p�����[�^�P��();
            �����Z���N�g();
            ���������p�����[�^����();
        }
    }
}
