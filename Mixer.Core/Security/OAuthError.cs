using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mixer.Core.Security
{
    public class OAuthError
    {
        [JsonProperty("error")]
        public String Error { get; set; }
        [JsonProperty("error_description")]
        public String ErrorDesciption { get; set; }
    }
}
