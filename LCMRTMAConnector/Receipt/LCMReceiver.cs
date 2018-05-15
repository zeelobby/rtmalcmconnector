using System;
using LCM;
using mavlcm;
using LCMRTMAConnector.Util;
using LCMRTMAConnector.Properties;
using LCMRTMAConnector.Transmission;
using Dragonfly;

namespace LCMRTMAConnector.Receipt
{
    public class LCMReceiver
    {
        private readonly LCM.LCM.LCM lCM;

        public LCMReceiver()
        {
            this.lCM = new LCM.LCM.LCM();
        }

        public void receive(int lCMReceiveRate, ApplicationProperties props, RTMATransmitter rtmaTransmitter)
        {

            try
            {
                lCM.SubscribeAll(new SimpleSubscriber(props, rtmaTransmitter));


            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Ex: " + e);
                Environment.Exit(1);
            }
        }

        internal class SimpleSubscriber : LCM.LCM.LCMSubscriber
        {
            ApplicationProperties props;
            RTMATransmitter rtmaTransmitter;

            public SimpleSubscriber(ApplicationProperties props, RTMATransmitter rtmaTransmitter) : base()
            {
                this.props = props;
                this.rtmaTransmitter = rtmaTransmitter;
            }

            public void MessageReceived(LCM.LCM.LCM lcm, string channel, LCM.LCM.LCMDataInputStream dins)
            {
                Utils.lcmReceiverMessage("RECV: " + channel);

                if (channel == props.Lcm.Channels.neuralDecoder)
                {
                    mavlcm.MDF_NEURAL_DECODER_OUTPUT lcmIn = new mavlcm.MDF_NEURAL_DECODER_OUTPUT(dins);

                    Utils.lcmReceiverMessage("Received message of the type MDF_NEURAL_DECODER_OUTPUT:");
                    Utils.lcmReceiverMessage("  header   = {0}", lcmIn.header);
                    Utils.lcmReceiverMessage("  timestamp   = {0:D}", lcmIn.timestamp);
                    Utils.lcmReceiverMessage("  position    = ({0:N}, {1:N}, {2:N}, {3:N})",
                                      lcmIn.decoderoutput[0], lcmIn.decoderoutput[1], lcmIn.decoderoutput[2], lcmIn.decoderoutput[3]);
                    MDF.NEURAL_DECODER_OUTPUT rtmaOut = new MDF.NEURAL_DECODER_OUTPUT();
                    rtmaOut.decoderoutput = lcmIn.decoderoutput;
                    rtmaOut.timestamp = (int)lcmIn.timestamp;
                    rtmaTransmitter.transmit( MT.NEURAL_DECODER_OUTPUT, rtmaOut);
                }
            }
        }
    }
}
