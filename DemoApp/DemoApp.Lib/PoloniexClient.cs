/*
 * MIT License
 * Copyright (c) 2017 Kristijan Burnik
 * Please refer to the LICENSE file in project root.
 */
namespace DemoApp.Lib
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using DemoApp.Lib.Model;
    using Newtonsoft.Json;

    public class PoloniexClient
    {
        private static readonly string PublicEndpointUrl = "https://poloniex.com/public?command=";
        private static readonly string PublicCommandReturnTicker = "returnTicker";

        /// <summary>
        /// Returns the current ticker status for all Poloniex crypto currencies.
        /// </summary>
        /// <returns></returns>
        public async Task<TickerSnapshot> GetTickerAsync()
        {
            return await this.GetPublicAsync<TickerSnapshot>(PublicCommandReturnTicker);
        }

        /// <summary>
        /// Asynchronously loads a public API page and deserializes to given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task<T> GetPublicAsync<T>(string command)
        {
            return await this.GetAsync<T>(PublicEndpointUrl + command);
        }

        /// <summary>
        /// Asynchronously loads a page and deserializes to given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<T> GetAsync<T>(string url)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var rawBody = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(rawBody);
        }
    }
}
