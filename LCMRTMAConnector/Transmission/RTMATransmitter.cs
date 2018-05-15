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
            mod.ConnectToMMM(MID.LCMRTMA_TRANSMITTER, server);
        }

        public void transmit( int msgType, object data)
        {

            Utils.rtmaTransmitterMessage("Transmitting {0} message", msgType);

            mod.SendMessage(msgType, data);
        }

        public void disconnect()
        {
            mod.DisconnectFromMMM();
        }
    }
}
