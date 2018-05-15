using System;
using LCM;
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
                    Messages.bmi2ec_t msg = new Messages.bmi2ec_t(dins);

                    Console.WriteLine("Received message of the type example_t:");
                    Console.WriteLine("  timestamp   = {0:D}", msg.timestamp);
                    Console.WriteLine("  position    = ({0:N}, {1:N}, {2:N}, {3:N})",
                                      msg.feature_values[0], msg.feature_values[1], msg.feature_values[2], msg.feature_values[4]);
                }
            }
        }
    }
}
