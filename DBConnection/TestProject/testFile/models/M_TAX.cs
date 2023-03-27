using System;
using System.Collections.Generic;

namespace createEntity
{
    public partial class M_TAX
    {
        public int TAX_ID { get; set; }
        public byte TAX_KBN { get; set; }
        public decimal TAX_RATE { get; set; }
        public DateTime TAX_FROM { get; set; }
        public DateTime TAX_TO { get; set; }
        public byte STATUS { get; set; }
        public DateTime? ADD_DATE { get; set; }
        public string? ADD_USER_ID { get; set; }
        public string? ADD_USER_NAME { get; set; }
        public DateTime? EDIT_DATE { get; set; }
        public string? EDIT_USER_ID { get; set; }
        public string? EDIT_USER_NAME { get; set; }
    }
}
