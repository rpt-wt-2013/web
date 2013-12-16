using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DatabaseApplication.Database
{
    class MyValue
    {
        private string value;
        private DbType type;

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                }
            }
        }

        public DbType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                }
            }
        }

    }
}
