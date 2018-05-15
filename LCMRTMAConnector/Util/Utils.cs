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

        public static void rtmaTransmitterMessage(string message, params object[] arg0)
        {
            Console.WriteLine("RTMATransmitter: " + message, arg0);
        }

        public static void rtmaReceiverMessage(string message, params object[] arg0)
        {
            Console.WriteLine("RTMAReceiver: " + message, arg0);
        }

        public static void lcmTransmitterMessage(string message, params object[] arg0)
        {
            Console.WriteLine("LCMTransmitter: "+ message, arg0);
        }

        public static void lcmReceiverMessage(string message, params object[] arg0)
        {
            Console.WriteLine("LCMReceiver: " + message, arg0);
        }
    }
}
