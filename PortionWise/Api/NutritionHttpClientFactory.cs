using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PortionWise.Api
{

    public interface INutritionHttpClientFactory : IHttpClientFactory {}

    public class NutritionHttpClientFactory : INutritionHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            var apiKey = ApiKeyStore.NutritionApiKey;
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://api.calorieninjas.com/v1/")
            };
            
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            return client;
        }
    }
}
