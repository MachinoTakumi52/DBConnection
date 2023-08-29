using createEntity;
using DBConnectionForSQLServer;
using DBConnectionTools;
using System;
using System.Collections.Generic;
using testSolution.testFile.Constant;
using Xunit;

namespace testSolution.testFile
{
    public class UPDATEテスト
    {
        [Fact]
        public void アップデート()
        {
      
            EntityModifyBuilder<M_ITEM> builder = new EntityModifyBuilder<M_ITEM>();
            


            builder.Add<string>(x => x.ITEM_NAME, "ああああ");

            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    
                    conn.Update<M_ITEM>(tran,builder,"WHERE ITEM_ID = 9");
                    tran.Commit();

                }
                catch (Exception e)
                {
                    tran.Rollback();
                }

            }
        }


        [Fact]
        public void 　ビットアップデート()
        {

            EntityModifyBuilder<M_TEST> builder = new EntityModifyBuilder<M_TEST>();



            builder.Add<bool>(x => x.status ,true);

            using (var conn = new DataBaseConnection(Constants.connectString))
            using (var tran = conn.BeginTransaction())
            {
                try
                {

                    conn.Update<M_TEST>(tran, builder, "WHERE TEST_ID = @aaa", new List<CommandParameter>() { { new CommandParameter("@aaa", 1)} });
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
