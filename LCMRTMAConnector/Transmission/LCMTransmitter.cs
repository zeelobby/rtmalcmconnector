using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCM;
using LCM.LCM;

namespace LCMRTMAConnector
{
    public class LCMTransmitter
    {
        private LCM.LCM.LCM appLCM;

        public LCMTransmitter()
        {
            this.appLCM = new LCM.LCM.LCM();
        }

        public void transmit()
        {
            try
            {
                Messages.bmi2ec_t msg = new Messages.bmi2ec_t();
                TimeSpan span = DateTime.Now - new DateTime(1970, 1, 1);
                msg.timestamp = span.Ticks * 100;
                msg.feature_values = new float[] { 1232.1232f, 1232.1111f, 12.2423f, 123123f };

                System.Threading.Thread.Sleep(10000);

                Console.WriteLine("Sending Message");

                appLCM.Publish("EXAMPLE", msg);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Ex: " + e);
            }
        }
    }
}
