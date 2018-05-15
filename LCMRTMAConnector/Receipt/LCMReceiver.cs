using System;
using LCM;
using mavlcm;
namespace LCMRTMAConnector.Receipt
{
    public class LCMReceiver
    {
        private readonly LCM.LCM.LCM lCM;

        public LCMReceiver()
        {
            this.lCM = new LCM.LCM.LCM();
        }

        public void receive(int lCMReceiveRate)
        {

            try
            {
                lCM.SubscribeAll(new SimpleSubscriber());


            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Ex: " + e);
                Environment.Exit(1);
            }
        }

        internal class SimpleSubscriber : LCM.LCM.LCMSubscriber
        {
            public void MessageReceived(LCM.LCM.LCM lcm, string channel, LCM.LCM.LCMDataInputStream dins)
            {
                Console.WriteLine("RECV: " + channel);

                if (channel == "EXAMPLE")
                {
                    mavlcm.MDF_NEURAL_DECODER_OUTPUT msg = new mavlcm.MDF_NEURAL_DECODER_OUTPUT(dins);

                    Console.WriteLine("Received message of the type example_t:");
                    Console.WriteLine("  header   = {0}", msg.header);
                    Console.WriteLine("  timestamp   = {0:D}", msg.timestamp);
                    Console.WriteLine("  position    = ({0:N}, {1:N}, {2:N}, {3:N})",
                                      msg.decoderoutput[0], msg.decoderoutput[1], msg.decoderoutput[2], msg.decoderoutput[3]);
                }
            }
        }
    }
}
