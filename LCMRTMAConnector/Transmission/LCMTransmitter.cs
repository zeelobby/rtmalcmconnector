using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCM;
using LCM.LCM;
using mavlcm;
using LCMRTMAConnector.Util;

namespace LCMRTMAConnector
{
    public class LCMTransmitter
    {
        private LCM.LCM.LCM appLCM;

        public LCMTransmitter()
        {
            this.appLCM = new LCM.LCM.LCM();
        }

        public void transmit(int msgType, LCMEncodable lcmOut)
        {
            try
            {
                Console.WriteLine("Transmitting {0} message", msgType);

                appLCM.Publish("EXAMPLE", lcmOut);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Ex: " + e);
            }
        }
    }
}
