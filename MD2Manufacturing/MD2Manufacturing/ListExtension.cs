using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD2
{
    public static class ListExtension
    {
        public static bool ContainsAll<T>(this List<T> baseList, List<T> checkingList)
        {
            foreach(var v in checkingList)
            {
                if (!baseList.Contains(v))
                    return false;
            }
            return true;
        }
    }
}
