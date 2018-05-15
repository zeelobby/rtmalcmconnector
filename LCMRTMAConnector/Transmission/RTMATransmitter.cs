using Dragonfly;
using LCMRTMAConnector.Util;
using System;
namespace LCMRTMAConnector.Transmission
{
    public class RTMATransmitter
    {

        private readonly string server;
        private readonly Module mod;


        public RTMATransmitter(string server)
        {
            this.server = server;
            this.mod = new Module();
        }

        public void transmit(short msgTarget, int msgType, object data)
        {
            mod.ConnectToMMM(msgTarget, server);

            Utils.lcmTransmitterMessage("Transmitting {0} message to {1}", msgType, msgTarget);

            mod.SendMessage(msgType, data);
        }

        public void disconnect()
        {
            mod.DisconnectFromMMM();
        }
    }
}
