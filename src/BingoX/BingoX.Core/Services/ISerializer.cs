using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Services
{
    public interface ISerializer
    {
        T Deserialize<T>(string str);
        string SerializeString(object obj);
        byte[] Serialize(object obj);
        T Deserialize<T>(byte[] buffer);
    }
}
