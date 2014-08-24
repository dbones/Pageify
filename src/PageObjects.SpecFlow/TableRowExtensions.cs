using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TechTalk.SpecFlow;

namespace PageObjects.SpecFlow
{
    public static class TableRowExtensions
    {
        public static T GetEnumValue<T>(this TableRow row, string columnName, T defaultValue = default(T))
        {
            if (typeof(T).BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be of type System.Enum");
            }

            var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            var names = Enum.GetNames(typeof(T)).ToArray();

            if (!row.ContainsKey(columnName))
            {
                return defaultValue;
            }

            var stringValue = row[columnName];
            var index = Array.IndexOf(names, stringValue);

            if (index == -1)
            {
                throw new InvalidEnumArgumentException(string.Format("{0} does not exist as a value for {1}", stringValue, typeof(T).FullName));
            }

            return values[index];
        }

        public static T GetValue<T>(this TableRow row, string columnName, T defaultValue = default(T))
        {
            if (!row.ContainsKey(columnName))
            {
                return defaultValue;
            }

            var stringValue = row[columnName];
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var value = (T)converter.ConvertFromInvariantString(stringValue);
            return value;
        }

        public static IEnumerable<T> GetValues<T>(this TableRow row, string columnName)
        {
            if (!row.ContainsKey(columnName))
            {
                yield break;
            }


            var stringValues = row[columnName].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var stringValue in stringValues)
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                var value = (T)converter.ConvertFromInvariantString(stringValue);
                yield return value;
            }
        }
    }
}