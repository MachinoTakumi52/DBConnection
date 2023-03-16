using System;
using System.Collections.Generic;

namespace createEntity
{
    public partial class T_URIAGE
    {
        public int U_ID { get; set; }
        public int TOKUI_ID { get; set; }
        public DateTime U_DATE { get; set; }
        public int U_KIN_KEI { get; set; }
        public int U_TAX_KEI { get; set; }
        public string? BIKOU { get; set; }
        public int? S_ID { get; set; }
        public byte STATUS { get; set; }
        public DateTime? ADD_DATE { get; set; }
        public string? ADD_USER_ID { get; set; }
        public string? ADD_USER_NAME { get; set; }
        public DateTime? EDIT_DATE { get; set; }
        public string? EDIT_USER_ID { get; set; }
        public string? EDIT_USER_NAME { get; set; }
    }
}
