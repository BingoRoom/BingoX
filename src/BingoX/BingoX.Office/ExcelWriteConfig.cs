using System.Collections.Generic;

namespace BingoX.Office
{
    public class ExcelWriteConfig
    {
        public ExcelType ExcelType { get; set; }
        public string SheetName { get; set; }
        public IDictionary<string, string> MapperColumns { get; set; }

       

    }
}
