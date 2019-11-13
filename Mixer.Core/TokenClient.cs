using Mixer.Core.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mixer.Core
{
    public class TokenClient
    {
        private OAuthToken mOAuthToken { get; set; }
        private String mClientId { get; set; }
        private String mClientSecret { get; set; }
        private String mTokenUrl = "https://mixer.com/api/v1/oauth/token";

        public TokenClient(String clientId, String clientSecret)
        {
            this.mClientId = clientId;
            this.mClientSecret = clientSecret;
        }

        public TokenClient(OAuthToken token, String clientId, String clientSecret)
        {
            this.mOAuthToken = token;
            this.mClientId = clientId;
            this.mClientSecret = clientSecret;
        }

        public static TokenClient FromJsonString(String tokenString, String clientId, String clientSecret)
        {
            var oauthToken = JsonConvert.DeserializeObject<OAuthToken>(tokenString);
            return new TokenClient(oauthToken, clientId, clientSecret);
        }

        public async Task RefreshToken()
        {
            var httpClient = new HttpClient();            
            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("refresh_token", mOAuthToken.RefreshToken);
            parameters.Add("client_id", mClientId);
            parameters.Add("client_secret", mClientSecret);
            var formContent = new FormUrlEncodedContent(parameters);
            formContent.Headers.Add("Client-ID", mClientId);
            var response = await httpClient.PostAsync(mTokenUrl, formContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                this.mOAuthToken = JsonConvert.DeserializeObject<OAuthToken>(responseBody);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<OAuthError>(responseBody);
                throw new Exception($"{error.Error} - {error.ErrorDesciption}");
            }
        }

        public async Task GetTokenFromCode(String code, String redirectUri)
        {
            var httpClient = new HttpClient();
            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("code", code);
            parameters.Add("redirect_uri", redirectUri);
            parameters.Add("client_id", mClientId);
            parameters.Add("client_secret", mClientSecret);
            var response = await httpClient.PostAsync(mTokenUrl, new FormUrlEncodedContent(parameters));
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                this.mOAuthToken = JsonConvert.DeserializeObject<OAuthToken>(responseBody);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<OAuthError>(responseBody);
                throw new Exception($"{error.Error} - {error.ErrorDesciption}");
            }
        }

        public String GetTokenString()
        {
            return JsonConvert.SerializeObject(mOAuthToken, Formatting.Indented);
        }
    }
}
