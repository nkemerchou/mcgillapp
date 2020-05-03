using Google.Cloud.AutoML.V1;
using Google.Cloud.BigQuery.V2;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using SIMFireRiskPredictor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SIMFireRiskPredictor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            try
            {
                string projectId = "thermal-creek-272919";
                var client = BigQueryClient.Create(projectId);
                string query = $"SELECT * " +
                    $" FROM `dw.Montreal_DB_Poly_compact`" +
                    $" Where ST_CONTAINS(geo, ST_GEOGPOINT(-73.573260, 45.504566)) ";




                //string query = @"SELECT * except (Borough_id, DBuid, DAuid) FROM `thermal-creek-272919.dw.All_By_DB2` LIMIT 10";
                var result = client.ExecuteQuery(query, parameters: null);
                Console.Write("\nQuery Results:\n------------\n");
                foreach (var row in result)
                {
                    var dbuid = row["DBUID"];
                    Console.WriteLine($"{row["DBUID"]}: {row["geo"]}");
                }
            }
            catch (Exception ex)
            {

            }



            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            //https://googleapis.github.io/google-cloud-dotnet/docs/Google.Cloud.AutoML.V1/api/Google.Cloud.AutoML.V1.PredictionServiceClient.html

            PythonPredict();
            //prediction();
            //test();
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        static void PythonPredict()
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Program Files\Python38\python.exe";

            // 2) Provide script and arguments
            //var script = @"C:\Users\nkadmin\source\repos\SIMFireRiskPredictor\SIMFireRiskPredictor\predict.py";
            var script = @"predict.py";
            var payloadArg = @"{""""bld_avg_Above_ground_floors"""":1.92857142857142,	""""bld_avg_building_Area"""":167.571428571428,	""""bld_avg_lot_Area"""":354.9375,	""""bld_avg_property_count"""":1.64285714285714,	""""bld_median_building_type"""":1000,	""""bld_median_catagorie_building"""":0,	""""bp_Average_Age"""":44,	""""bp_Average_Income"""":67517,	""""bp_Buildings_with_5_or_more_floors"""":4,	""""bp_Buildings_with_less_than_5_floors"""":34,	""""bp_College"""":19,	""""bp_construction_after_2000"""":6,	""""bp_construction_between_1981_and_2000"""":16,	""""bp_Couple_without_children"""":36,	""""bp_firestation_count"""":1,	""""bp_Immigrant_population"""":16,	""""bp_Language_French_Spoken"""":33,	""""bp_Mobile_homes"""":0,	""""bp_Population"""":4958,	""""bp_Population_density"""":473,	""""bp_Secondaryhighschool"""":24,	""""bp_Semi_detached_or_row_houses"""":18,	""""bp_Unemployment_rate"""":8,	""""crime_db_count"""":1,	""""da_area"""":0.1341,	""""da_Firestation_count"""":0,	""""da_pop_density"""":3795.67486950037,	""""da_population"""":509,	""""db_area"""":0.0228,	""""db_density"""":1315.78947368421,	""""db_population"""":30,	""""iw_weather_str"""":""""6"""",	""""Month"""":""""4"""",	""""nbr_comment_311"""":0,	""""nbr_complain_311"""":0,	""""nbr_request_311"""":0,	""""Part_of_Day"""":""""2"""",	""""season"""":""""1"""",	""""Weekday"""":""""0""""}"; 
                //"//,	\"bld_avg_building_Area\":167.571428571428,	\"bld_avg_lot_Area\":354.9375,	\"bld_avg_property_count\":1.64285714285714,	\"bld_median_building_type\":1000,	\"bld_median_catagorie_building\":0,	\"bp_Average_Age\":44,	\"bp_Average_Income\":67517,	\"bp_Buildings_with_5_or_more_floors\":4,	\"bp_Buildings_with_less_than_5_floors\":34,	\"bp_College\":19,	\"bp_construction_after_2000\":6,	\"bp_construction_between_1981_and_2000\":16,	\"bp_Couple_without_children\":36,	\"bp_firestation_count\":1,	\"bp_Immigrant_population\":16,	\"bp_Language_French_Spoken\":33,	\"bp_Mobile_homes\":0,	\"bp_Population\":4958,	\"bp_Population_density\":473,	\"bp_Secondaryhighschool\":24,	\"bp_Semi_detached_or_row_houses\":18,	\"bp_Unemployment_rate\":8,	\"crime_db_count\":1,	\"da_area\":0.1341,	\"da_Firestation_count\":0,	\"da_pop_density\":3795.67486950037,	\"da_population\":509,	\"db_area\":0.0228,	\"db_density\":1315.78947368421,	\"db_population\":30,	\"iw_weather_str\":\"6\",	\"Month\":\"4\",	\"nbr_comment_311\":0,	\"nbr_complain_311\":0,	\"nbr_request_311\":0,	\"Part_of_Day\":\"2\",	\"season\":\"1\",	\"Weekday\":\"0\"}";

            //psi.Arguments = $"\"{script}\"";
            psi.Arguments = $"\"{script}\" \"{payloadArg}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            // 5) Display output
            Console.WriteLine("ERRORS:");
            Console.WriteLine(errors);
            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine(results);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private async Task test()
        {
            PredictClient client = new PredictClient();

            string project = "thermal-creek-272919";
            string model = "TBL5828849788421931008";
            string location = "us-central1";

            Sample x = new Sample
            {
                MyProperty = 25

            };
            var instances = new List<Sample> { x };

            await client.Predict(project, location, model);
            //Console.WriteLine(String.Join("\n", predictions));
        }
        private void prediction()
        {
            try
            {
                ExamplePayload payload = new ExamplePayload()
                {
                    //TextSnippet = new TextSnippet()
                    //{
                    //    Content = "{\"payload\":{\"row\":{\"values\":[\"3795.67486950037\",\"0.0\",\"30.0\"]}}}"
                    //}
                };


                PredictionServiceClient predictionServiceClient = PredictionServiceClient.Create();

                PredictRequest request = new PredictRequest
                {
                    ModelName = ModelName.FromProjectLocationModel("thermal-creek-272919", "us-central1", "TBL5828849788421931008"),
                    Payload = payload,
                    //model_path = client.model_path('thermal-creek-272919', 'us-central1', 'TBL5828849788421931008')

                    Params = { { "feature_importance", "false" } },
                };

                var response = predictionServiceClient .Predict(request);
            }
            catch (Exception ex)
            {

                throw;
            }
            //mockGrpcClient.Setup(x => x.Predict(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);

            //PredictionServiceClient client = new PredictionServiceClientImpl(mockGrpcClient.Object, null);

            //PredictResponse response = client.Predict(request);
        }
        private void predict(string projectId, string locationId, string modelId, List<Value> values)
        {
            // Initialize client that will be used to send requests. This client only needs to be created
            // once, and can be reused for multiple requests. After completing all of your requests, call
            // the "close" method on the client to safely clean up any remaining background resources.
            PredictionServiceClient client = PredictionServiceClient.Create();

            //// Get the full path of the model.
            //ModelName name = new ModelName(projectId, locationId, modelId);
            ////.of(projectId, "us-central1", modelId);

            //Google.Cloud.AutoML.V1.ro Row row = Row.newBuilder().addAllValues(values).build();
            //ExamplePayload payload = ExamplePayload.newBuilder().setRow(row).build();

            // Feature importance gives you visibility into how the features in a specific prediction
            // request informed the resulting prediction. For more info, see:
            // https://cloud.google.com/automl-tables/docs/features#local
            //PredictRequest request = PredictRequest. newBuilder()
            //        .setName(name.toString())
            //        .setPayload(payload)
            //        .putParams("feature_importance", "true")
            //        .build();

            //PredictResponse response = client.Predict(request);

            //System.out.println("Prediction results:");
            //foreach (AnnotationPayload annotationPayload in response.getPayloadList()) {
            //TablesAnnotation tablesAnnotation = annotationPayload.getTables();
            //System.out.format("Classification label: %s%n", tablesAnnotation.getValue().getStringValue());
            //System.out.format("Classification score: %.3f%n", tablesAnnotation.getScore());
            // Get features of top importance
            //tablesAnnotation
            //    .getTablesModelColumnInfoList()
            //    .forEach(
            //        info ->
            //            System.out.format(
            //                "\tColumn: %s - Importance: %.2f%n",
            //                info.getColumnDisplayName(), info.getFeatureImportance()));
            //}
        }
    }
}
