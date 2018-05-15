using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Xunit.Abstractions;
using LCMRTMAConnector.Receipt;
using LCMRTMAConnector.Properties;

namespace LCMRTMAConnector
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            ApplicationProperties props;
            Console.WriteLine("LCMRTMA Connector Started");

            using (var input = File.OpenText("Properties/props.yaml"))
            {
                var deserializer = new DeserializerBuilder()
                    .Build();

                props = deserializer.Deserialize<Properties.ApplicationProperties>(input);
            }

            var appLCM = new LCM.LCM.LCM();

            //LCMReceiver receiver = new LCMReceiver();
            //receiver.receive(props.LcmParseRate);

            LCMTransmitter transmitter = new LCMTransmitter();
            transmitter.transmit();
        }
    }
}
