using System;
using System.Reflection;

namespace Atomic.Core
{
    public class Utilities
    {
        public static void CopyValues<T>(T @base, T copy)
        {
            var type = @base.GetType();
            foreach(var field in type.GetFields())
            {
                field.SetValue(copy, field.GetValue(@base));
            }
        }
    }
}