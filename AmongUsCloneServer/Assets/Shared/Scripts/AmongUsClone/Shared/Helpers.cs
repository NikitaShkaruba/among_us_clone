using System;

namespace AmongUsClone.Shared
{
    public static class Helpers
    {
        public static string GetEnumName(Enum enumObject)
        {
            return Enum.GetName(enumObject.GetType(), enumObject);
        }
    }
}
