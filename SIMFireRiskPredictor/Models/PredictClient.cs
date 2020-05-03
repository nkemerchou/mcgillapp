using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SIMFireRiskPredictor.Models
{
    public class PredictClient
    {

        private HttpClient client;

        public PredictClient()
        {
            this.client = new HttpClient();
            client.BaseAddress = new Uri("https://automl.googleapis.com/v1beta1/"); // projects/${PROJECT_ID}/locations/{location}/models/model-id:predict");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task Predict(String project, string location, String model, String version = null)
        {
            try
            {
                //var version_suffix = version == null ? "" : $"/version/{version}";
                var model_uri = $"projects/{project}/locations/{location}/models/{model}";
                var predict_uri = $"{model_uri}:predict";

                GoogleCredential credential = await GoogleCredential.GetApplicationDefaultAsync();
                credential.CreateScoped(new string[]
                    {
                        "automl.googleapis.com",
                        "https://www.googleapis.com/auth/cloud-platform",
                        
                   });

                //var credential = GoogleCredential.GetApplicationDefault();

                var bearer_token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
                //var bearer_token = "ya29.a0Ae4lvC3VritdeVz9cCQoA8qfk0g9mRfg3qlbpRV8tbLr7GnMIIskZ3PxVoiQpN1XPdblD40rI7CLonr1VEyQ6rCuvyHCsAa_vFI37oUq5iYhWpORVJPVcWjwmEBv3t9GoB5QIPrBL_FDeqj1ObAYRocD4uaMwUO04nPmu9UziksH6FY-n2bVBfowlM_-KTv-2bHDynGM6hrOkLIsVKUXO6GId9EPqer4PdLCqQqln43UElA2SeO-6IrChHwI-7vOZu0H2C-OgAEk";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer_token);

                //var request = new { instances = instances };
                var content = new StringContent("{\"payload\":{\"row\":{\"values\":[\"3795.67486950037\",\"0.0\",\"30.0\",\"0.0228\",\"1315.78947368421\",\"1.0\",\"1000.0\",\"0.0\",\"1.9285714285714284\",\"1.6428571428571426\",\"354.9375\",\"167.57142857142858\",\"4.0\",\"0.0\",\"2.0\",\"1.0\",\"6.0\",\"0.0\",\"0.0\",\"0.0\",\"1.0\",\"473.0\",\"4958.0\",\"8.0\",\"44.0\",\"67517.0\",\"36.0\",\"16.0\",\"6.0\",\"18.0\",\"34.0\",\"4.0\",\"0.0\",\"19.0\",\"24.0\",\"16.0\",\"33.0\",\"509.0\",\"0.1341\"]}}}", Encoding.UTF8, "application/json");

                var responseMessage = await client.PostAsync(predict_uri, content);
                responseMessage.EnsureSuccessStatusCode();

                var responseBody = await responseMessage.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(responseBody);

                //return response.predictions.ToObject<List<O>>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    public class Sample
    {
        public int MyProperty { get; set; }
    }
}
