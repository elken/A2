#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace SOFT144_A2
{
    class Util
    {

        public static void swap<T>(ref T lhs, ref T rhs)
        {
            T tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }

        public static T random_member_of<T>()
        {
            Random r = new Random();
            return (T)(object)r.Next(0, (Enum.GetNames(typeof(T)).Length));
        }
    }
}
