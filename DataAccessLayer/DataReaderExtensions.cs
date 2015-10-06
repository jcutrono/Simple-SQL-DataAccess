using System;
using System.Data;

namespace DataAccessLayer
{
    internal static class DataReaderExtensions
    {
        internal static T GetNullSafeValue<T>(this IDataReader reader, int i)
        {
            object value = reader.GetValue(i);
            return value == DBNull.Value ? default(T) : (T)value;
        }

        internal static string GetNullSafeString(this IDataReader reader, int i)
        {
            object value = reader.GetValue(i);
            return value == DBNull.Value ? string.Empty : value.ToString();
        }
    }
}
