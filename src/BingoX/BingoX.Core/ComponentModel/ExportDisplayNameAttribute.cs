using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BingoX
{
    public class ExportDisplayNameAttribute : DisplayNameAttribute
    {
        public ExportDisplayNameAttribute(string displayName) : base(displayName)
        {

        }


        public int Order { get; set; }
    }
}
