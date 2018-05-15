using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace LCMRTMAConnector.Properties
{
    public class ApplicationProperties
    {
        public Lcm Lcm { get; set; }
        public Rtma Rtma { get; set;  }
       
    }

    public class Lcm
    {
        public int ParseRate { get; set; }
        public Channels Channels { get; set; }
    }

    public class Rtma
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }

    public class Channels
    {
        public string neuralDecoder { get; set; }
    }
}
