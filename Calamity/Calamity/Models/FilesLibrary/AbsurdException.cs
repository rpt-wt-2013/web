using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamity.Models.FilesLibrary
{
    [Serializable]
    public class AbsurdException : Exception
    {
        public AbsurdException() : base() { }
        public AbsurdException(String msg) : base(msg) { }
        public AbsurdException(String msg, Exception cause) : base(msg, cause) { }
    }
}
