using System;
using System.Collections.Generic;

namespace createEntity
{
    public partial class T_SEIKYU_M
    {
        public int SM_ID { get; set; }
        public int S_ID { get; set; }
        public int ITEM_ID { get; set; }
        public DateTime U_DATE { get; set; }
        public int U_PRICE { get; set; }
        public int U_SU { get; set; }
        public int U_KIN { get; set; }
        public int U_TAX { get; set; }
        public decimal U_TAX_RATE { get; set; }
        public string? BIKOU { get; set; }
        public byte STATUS { get; set; }
        public DateTime? ADD_DATE { get; set; }
        public string? ADD_USER_ID { get; set; }
        public string? ADD_USER_NAME { get; set; }
        public DateTime? EDIT_DATE { get; set; }
        public string? EDIT_USER_ID { get; set; }
        public string? EDIT_USER_NAME { get; set; }
    }
}
