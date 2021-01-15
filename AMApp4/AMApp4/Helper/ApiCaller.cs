using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AMApp4.Helper
{
    public class ApiCaller
    {
        public static async Task<ApiResponse> Get(string url, string apikey = null, string host = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(apikey) && !string.IsNullOrWhiteSpace(host))
                {
                    client.DefaultRequestHeaders.Add("x-rapidapi-key", apikey);
                    client.DefaultRequestHeaders.Add("x-rapidapi-host", host);
                }

                var request = await client.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    return new ApiResponse { Response = await request.Content.ReadAsStringAsync() };
                }
                else
                    return new ApiResponse { ErrorMessage = request.ReasonPhrase };
            }
        }
    }

    public class ApiResponse
    {
        public bool Successful => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
    }
}
