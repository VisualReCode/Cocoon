using System.Reflection;
using MessagePack;

namespace ReCode.Cocoon.Legacy.Tests
{
    internal static class AttributeHelper
    {
        public static bool AttributeCorrect(PropertyInfo propInfo, int positionValue)
        {
            var attrs = propInfo.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                if (attr is KeyAttribute msgPackKey)
                {
                    if (msgPackKey.IntKey == positionValue)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}