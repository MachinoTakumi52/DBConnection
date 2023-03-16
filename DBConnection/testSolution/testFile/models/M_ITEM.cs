using System;
using System.Collections.Generic;

namespace createEntity
{
    public partial class M_ITEM
    {
        public M_ITEM(int iTEM_ID, string iTEM_CD, string iTEM_NAME, int pRICE, int uNIT, byte tAX_KBN, int sEQ, byte sTATUS, DateTime? aDD_DATE, string aDD_USER_ID, string aDD_USER_NAME, DateTime? eDIT_DATE, string eDIT_USER_ID, string eDIT_USER_NAME)
        {
            ITEM_ID = iTEM_ID;
            ITEM_CD = iTEM_CD;
            ITEM_NAME = iTEM_NAME;
            PRICE = pRICE;
            UNIT = uNIT;
            TAX_KBN = tAX_KBN;
            SEQ = sEQ;
            STATUS = sTATUS;
            ADD_DATE = aDD_DATE;
            ADD_USER_ID = aDD_USER_ID;
            ADD_USER_NAME = aDD_USER_NAME;
            EDIT_DATE = eDIT_DATE;
            EDIT_USER_ID = eDIT_USER_ID;
            EDIT_USER_NAME = eDIT_USER_NAME;
        }

        public int ITEM_ID { get; set; }
        public string ITEM_CD { get; set; } = null!;
        public string ITEM_NAME { get; set; } = null!;
        public int PRICE { get; set; }
        public int UNIT { get; set; }
        public byte TAX_KBN { get; set; }
        public int SEQ { get; set; }
        public byte STATUS { get; set; }
        public DateTime? ADD_DATE { get; set; }
        public string? ADD_USER_ID { get; set; }
        public string? ADD_USER_NAME { get; set; }
        public DateTime? EDIT_DATE { get; set; }
        public string? EDIT_USER_ID { get; set; }
        public string? EDIT_USER_NAME { get; set; }
    }

   
}
