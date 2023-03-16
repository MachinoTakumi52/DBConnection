
using CommonLibrary;
using createEntity;
using DBConnectionForSQLServer;
using System;
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

       
    }
}
