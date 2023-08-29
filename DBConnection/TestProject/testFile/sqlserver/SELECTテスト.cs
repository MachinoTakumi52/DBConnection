using createEntity;

using System;
using System.Collections.Generic;
using Xunit;
using testSolution.testFile.Constant;
using DBConnectionTools;
using DBConnectionForSQLServer;

namespace testSolution.testFile
{
    public class SELECT�e�X�g
    {
   
        [Fact]
        public void �S��()
        {
            //SQL��`
            string sql = "SELECT * FROM M_ITEM";
            //�p�����[�^�w��

            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            {

                //���X�g�ŕԋp
                var r =  conn.Select<M_ITEM>(sql);

                Console.WriteLine("");
            }
        }

        [Fact]
        public void ���������p�����[�^�P��()
        {
            //SQL��`
            string sql = "SELECT * FROM M_ITEM WHERE ITEM_ID = @ITEM_ID";
            //�p�����[�^�w��

            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            {
                //���X�g�ŕԋp 
                var r = conn.Select<M_ITEM>(sql, new List<CommandParameter>() { { new CommandParameter("@ITEM_ID", 1, System.Data.DbType.Int64) } });
                var a = conn.Select<M_ITEM>(sql, new List<CommandParameter>() { { new CommandParameter("@ITEM_ID", 1) } });

                Console.WriteLine("");
            }
        }

        [Fact]
        public void �����Z���N�g()
        {
   
            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                //SQL��`
                var sql2 = "SELECT ITEM_ID,ITEM_CD FROM M_ITEM";

                //�^�v���^
                var bbb = conn.Select<(int ITEM_ID, string ITEM_CD)>(sql2,null,tran);

                //�����N���X
                var ccc = conn.Select(new { ITEM_ID = default(int), ITEM_CD = default(string) }, sql2,null,tran);


                Console.WriteLine("");
            }

        }

        [Fact]
        public void ���������p�����[�^����()
        {
            //SQL��`
            string sql = "SELECT * FROM M_ITEM WHERE ITEM_ID = @ITEM_ID OR ITEM_CD = @ITEM_CD";
            //�p�����[�^�w��

            using (DataBaseConnection conn = new DataBaseConnection(Constants.connectString))
            {

                //���X�g�ŕԋp
                var r = conn.Select<M_ITEM>(sql, new List<CommandParameter>() { { new CommandParameter("@ITEM_ID", 1, System.Data.DbType.Int32) }, { new CommandParameter("@ITEM_CD", "2") } });

                Console.WriteLine("");
            }
        }

    }
}
