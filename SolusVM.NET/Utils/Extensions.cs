using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolusVM
{
    internal static class Extensions
    {
        internal static bool IsNotNull(this object value) { return (null != value); }
        internal static bool IsNull(this object value) { return (null == value); }
        internal static bool IsTrue(this bool value) { return value; }
        internal static bool IsFalse(this bool value) { return !value; }
    }
}
