using System;
using System.Collections.Generic;

namespace createEntity
{
    public partial class M_TOKUI
    {
        public int TOKUI_ID { get; set; }
        public string TOKUI_CD { get; set; } = null!;
        public string TOKUI_NAME { get; set; } = null!;
        public string? POST_NO { get; set; }
        public string? ADDRESS { get; set; }
        public string? TEL { get; set; }
        public string? FAX { get; set; }
        public byte SIME_DATE { get; set; }
        public string? TANTOU_NAME { get; set; }
        public int? SEQ { get; set; }
        public byte STATUS { get; set; }
        public DateTime? ADD_DATE { get; set; }
        public string? ADD_USER_ID { get; set; }
        public string? ADD_USER_NAME { get; set; }
        public DateTime? EDIT_DATE { get; set; }
        public string? EDIT_USER_ID { get; set; }
        public string? EDIT_USER_NAME { get; set; }
    }
}
