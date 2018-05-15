using System;
using YamlDotNet.Serialization;

namespace LCMRTMAConnector.Properties
{
    public class ApplicationProperties
    {
        [YamlMember(Alias = "lcm-parse-rate", ApplyNamingConventions = false)]
        public int LcmParseRate { get; set; }
    }
}
