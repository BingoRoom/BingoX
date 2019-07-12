using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Generator
{
    public interface IGenerator<TId>
    {
        TId New();
    }
}
