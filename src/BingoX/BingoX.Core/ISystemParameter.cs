using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX
{
    public interface ISystemParameter
    {
        /// <summary>
        /// 參數分類
        /// </summary>
        string ParameterGroup { get; set; }

        /// <summary>
        /// 參數代碼
        /// </summary>
        string ParameterKey { get; set; }

        /// <summary>
        /// 參數數值
        /// </summary>
        string ParameterValue { get; set; }

        string GetValue();
    }
}
