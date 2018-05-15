using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Xunit.Abstractions;
using LCMRTMAConnector.Receipt;
using LCMRTMAConnector.Properties;
using LCMRTMAConnector.Transmission;

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
                 .WithNamingConvention(new CamelCaseNamingConvention())
                 .Build();

                props = deserializer.Deserialize<ApplicationProperties>(input);
            }

            var appLCM = new LCM.LCM.LCM();
            string rtmaServer = props.Rtma.Host + ":" + props.Rtma.Port;

            LCMReceiver lcmReceiver = new LCMReceiver();
            lcmReceiver.receive(props.Lcm.ParseRate, props);

            LCMTransmitter lcmTransmitter = new LCMTransmitter();
            RTMATransmitter rtmaTransmitter = new RTMATransmitter(rtmaServer);

            RTMAReceiver rtmaReceiver = new RTMAReceiver(rtmaServer, lcmTransmitter);
            
            while (true) {
                rtmaReceiver.receive();
            };

            rtmaReceiver.disconnect();
            rtmaTransmitter.disconnect();
        }
    }
}
