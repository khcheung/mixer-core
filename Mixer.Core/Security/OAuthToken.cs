using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mixer.Core.Security
{
    public class OAuthToken
    {
        [JsonProperty("access_token")]
        public String AccessToken { get; set; }

        [JsonProperty("token_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OAuthTokenType TokenType { get; set; }

        [JsonProperty("expires_in")]
        public Int32 ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public String RefreshToken { get; set; }

    }


    public enum OAuthTokenType
    {
        Bearer
    }
}
