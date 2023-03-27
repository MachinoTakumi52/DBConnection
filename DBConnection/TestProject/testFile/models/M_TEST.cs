using System;
using System.Collections.Generic;

namespace createEntity
{
    public partial class M_TEST
    {
        public int TEST_ID { get; set; }
        public bool status { get; set; }
        public string name { get; set; }

        public M_TEST(int tEST_ID, bool status,string name)
        {
            TEST_ID = tEST_ID;
            this.status = status;
            this.name = name;
        }
    }
}
