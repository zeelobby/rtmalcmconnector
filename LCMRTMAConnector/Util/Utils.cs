using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LCMRTMAConnector.Util
{
    class Utils
    {

        public static string FindConstantName<T>(Type containingType, T value)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            foreach (FieldInfo field in containingType.GetFields
                     (BindingFlags.Static | BindingFlags.Public))
            {
                if (field.FieldType == typeof(T) &&
                    comparer.Equals(value, (T)field.GetValue(null)))
                {
                    return field.Name; // There could be others, of course...
                }
            }
            return null; // Or throw an exception
        }


        public static void rtmaReceiverMessage(string message, params object[] arg0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message, arg0);
            Console.ResetColor();
        }

        public static void errorMessage(string message, params object[] arg0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, arg0);
            Console.ResetColor();
        }
    }
}
