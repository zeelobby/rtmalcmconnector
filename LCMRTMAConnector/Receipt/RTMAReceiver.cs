using Dragonfly;
using LCM.LCM;
using LCMRTMAConnector.Util;
using System;
namespace LCMRTMAConnector.Receipt
{
    public class RTMAReceiver
    {
        private readonly Module mod;
        private readonly string server;
        private readonly LCMTransmitter lCMTransmitter;
        private Message m;

        public RTMAReceiver(string server, LCMTransmitter lCMTransmitter)
        {
            this.mod = new Module();
            this.server = server;
            this.lCMTransmitter = lCMTransmitter;
            mod.ConnectToMMM(MID.LCMRTMA_CONNECTOR, server);
            mod.Subscribe(MT.NEURAL_DECODER_OUTPUT);

            Utils.rtmaReceiverMessage("RTMAReceiver Listening...");
        }

        public void receive()
        {

            try
            {
                m = mod.ReadMessage(Module.ReadType.NonBlocking);

                if (m.msg_type > 100)
                {

                    Utils.rtmaReceiverMessage("Received message {0}", m.msg_type);

                    object o;
                    if (m.msg_type == MT.NEURAL_DECODER_OUTPUT)
                    {
                        MDF.NEURAL_DECODER_OUTPUT rtmaIn = new MDF.NEURAL_DECODER_OUTPUT();
                        o = rtmaIn;
                        m.GetData(ref o);

                        mavlcm.MDF_NEURAL_DECODER_OUTPUT lcmOut = new mavlcm.MDF_NEURAL_DECODER_OUTPUT();
                        lcmOut.decoderoutput = rtmaIn.decoderoutput;
                        lcmOut.timestamp = rtmaIn.timestamp;
                        lcmOut.header = "test";

                        lCMTransmitter.transmit("NEURAL_DECODER_OUTPUT", lcmOut);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Ex: " + e);
                Environment.Exit(1);
            }
        }

        public void disconnect()
        {
            mod.DisconnectFromMMM();
        }



    }
}
