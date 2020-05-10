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

        [HttpPost]
        public JsonResult GetDBuid(string lat, string lng)
        {
            string dbuid = "";
            try
            {
                string projectId = "firebird-276623";
                var client = BigQueryClient.Create(projectId);
                string query = $"SELECT * " +
                    $" FROM `dw.Montreal_DB_Poly_compact`" +
                    $" Where ST_CONTAINS(geo, ST_GEOGPOINT({lng}, {lat})) ";

                //string query = @"SELECT * except (Borough_id, DBuid, DAuid) FROM `thermal-creek-272919.dw.All_By_DB2` LIMIT 10";
                var result = client.ExecuteQuery(query, parameters: null);

                foreach (var row in result)
                {
                    dbuid = row["DBUID"].ToString();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return Json(dbuid);
        }

        [HttpPost]
        public JsonResult GetFields(string dbuid)
        {
            var features = new Features();
            try
            {
                string projectId = "firebird-276623";
                var client = BigQueryClient.Create(projectId);
                string query = $"SELECT bld_avg_Above_ground_floors , bld_avg_building_Area , bld_avg_construction_year , bld_avg_lot_Area , 	bld_avg_property_count , 	" +
                    $"  bld_median_building_type , 	bld_median_catagorie_building , 	bp_Apprentice_or_trade_school_diploma , 	bp_Average_Age , 	bp_Average_Household_sizepersons , " +
                    $"	bp_Average_Income , 	bp_Buildings_with_5_or_more_floors , 	bp_Buildings_with_less_than_5_floors , 	bp_College , 	bp_construction_after_2000 , 	" +
                    $"  bp_construction_between_1981_and_2000 , 	bp_Contruction_before1980 , 	bp_Couple_with_Children , 	bp_Couple_without_children , 	bp_firestation_count , " +
                    $"	bp_Immigrant_population , 	bp_Language_English_Spoken , 	bp_Language_French_Spoken , 	bp_Less_Than_50000 , 	bp_Mobile_homes , 	bp_No_Diploma , 	" +
                    $"  bp_Non_immigrant_population , 	bp_Other_Language_Spoken , 	bp_Owners , 	bp_Population , 	bp_Population_density , 	bp_Renters , 	bp_Secondaryhighschool , 	" +
                    $"  bp_Semi_detached_or_row_houses , 	bp_Single_Family_Homes , 	bp_Single_Parent_Families , 	bp_Unemployment_rate , 	bp_University , 	crime_db_count , 	da_area , 	" +
                    $"  da_Firestation_count , 	da_pop_density , 	da_population , 	da_total_private_dwellings , 	db_area , 	db_density , 	db_population , 	db_tot_Private_dwellings , 	" +
                    $"  iw_weather_str , 	Month , 	nbr_comment_311 , 	nbr_complain_311 , 	nbr_request_311 , 	Part_of_Day , 	season , 	Weekday " +
                    $"FROM dw.data " +
                    $"WHERE DBuid = {dbuid} " +
                    $"LIMIT 1 ";

                var result = client.ExecuteQuery(query, parameters: null);

                foreach (var row in result)
                {
                    features.bld_avg_Above_ground_floors = Convert.ToDouble(row["bld_avg_Above_ground_floors"].ToString());
                    features.bld_avg_building_Area = Convert.ToDouble(row["bld_avg_building_Area"].ToString());
                    features.bld_avg_construction_year = Convert.ToDouble(row["bld_avg_construction_year"].ToString());
                    features.bld_avg_lot_Area = Convert.ToDouble(row["bld_avg_lot_Area"].ToString());
                    features.bld_avg_property_count = Convert.ToDouble(row["bld_avg_property_count"].ToString());

                    features.bld_median_building_type = row["bld_median_building_type"].ToString();
                    features.bld_median_catagorie_building = row["bld_median_catagorie_building"].ToString();

                    features.bp_Apprentice_or_trade_school_diploma = Convert.ToDouble(row["bp_Apprentice_or_trade_school_diploma"].ToString());
                    features.bp_Average_Age = Convert.ToDouble(row["bp_Average_Age"].ToString());
                    features.bp_Average_Household_sizepersons = Convert.ToDouble(row["bp_Average_Household_sizepersons"].ToString());
                    features.bp_Average_Income = Convert.ToDouble(row["bp_Average_Income"].ToString());
                    features.bp_Buildings_with_5_or_more_floors = Convert.ToDouble(row["bp_Buildings_with_5_or_more_floors"].ToString());
                    features.bp_Buildings_with_less_than_5_floors = Convert.ToDouble(row["bp_Buildings_with_less_than_5_floors"].ToString());
                    features.bp_College = Convert.ToDouble(row["bp_College"].ToString());
                    features.bp_construction_after_2000 = Convert.ToDouble(row["bp_construction_after_2000"].ToString());
                    features.bp_construction_between_1981_and_2000 = Convert.ToDouble(row["bp_construction_between_1981_and_2000"].ToString());
                    features.bp_Contruction_before1980 = Convert.ToDouble(row["bp_Contruction_before1980"].ToString());
                    features.bp_Couple_with_Children = Convert.ToDouble(row["bp_Couple_with_Children"].ToString());
                    features.bp_Couple_without_children = Convert.ToDouble(row["bp_Couple_without_children"].ToString());
                    features.bp_firestation_count = Convert.ToDouble(row["bp_firestation_count"].ToString());
                    features.bp_Immigrant_population = Convert.ToDouble(row["bp_Immigrant_population"].ToString());
                    features.bp_Language_English_Spoken = Convert.ToDouble(row["bp_Language_English_Spoken"].ToString());
                    features.bp_Language_French_Spoken = Convert.ToDouble(row["bp_Language_French_Spoken"].ToString());
                    features.bp_Less_Than_50000 = Convert.ToDouble(row["bp_Less_Than_50000"].ToString());
                    features.bp_Mobile_homes = Convert.ToDouble(row["bp_Mobile_homes"].ToString());
                    features.bp_No_Diploma = Convert.ToDouble(row["bp_No_Diploma"].ToString());
                    features.bp_Non_immigrant_population = Convert.ToDouble(row["bp_Non_immigrant_population"].ToString());
                    features.bp_Other_Language_Spoken = Convert.ToDouble(row["bp_Other_Language_Spoken"].ToString());
                    features.bp_Owners = Convert.ToDouble(row["bp_Owners"].ToString());
                    features.bp_Population = Convert.ToDouble(row["bp_Population"].ToString());
                    features.bp_Population_density = Convert.ToDouble(row["bp_Population_density"].ToString());
                    features.bp_Renters = Convert.ToDouble(row["bp_Renters"].ToString());
                    features.bp_Secondaryhighschool = Convert.ToDouble(row["bp_Secondaryhighschool"].ToString());
                    features.bp_Semi_detached_or_row_houses = Convert.ToDouble(row["bp_Semi_detached_or_row_houses"].ToString());
                    features.bp_Single_Family_Homes = Convert.ToDouble(row["bp_Single_Family_Homes"].ToString());
                    features.bp_Single_Parent_Families = Convert.ToDouble(row["bp_Single_Parent_Families"].ToString());
                    features.bp_Unemployment_rate = Convert.ToDouble(row["bp_Unemployment_rate"].ToString());
                    features.bp_University = Convert.ToDouble(row["bp_University"].ToString());
                    features.crime_db_count = Convert.ToDouble(row["crime_db_count"].ToString());
                    features.da_area = Convert.ToDouble(row["da_area"].ToString());
                    features.da_Firestation_count = Convert.ToDouble(row["da_Firestation_count"].ToString());
                    features.da_pop_density = Convert.ToDouble(row["da_pop_density"].ToString());
                    features.da_population = Convert.ToDouble(row["da_population"].ToString());
                    features.da_total_private_dwellings = Convert.ToDouble(row["da_total_private_dwellings"].ToString());
                    features.db_area = Convert.ToDouble(row["db_area"].ToString());
                    features.db_density = Convert.ToDouble(row["db_density"].ToString());
                    features.db_population = Convert.ToDouble(row["db_population"].ToString());
                    features.db_tot_Private_dwellings = Convert.ToDouble(row["db_tot_Private_dwellings"].ToString());

                    features.iw_weather_str = row["iw_weather_str"].ToString();
                    features.Month = row["Month"].ToString();

                    features.nbr_comment_311 = Convert.ToDouble(row["nbr_comment_311"].ToString());
                    features.nbr_complain_311 = Convert.ToDouble(row["nbr_complain_311"].ToString());
                    features.nbr_request_311 = Convert.ToDouble(row["nbr_request_311"].ToString());
                    features.Part_of_Day = row["Part_of_Day"].ToString();
                    features.season = row["season"].ToString();
                    features.Weekday = row["Weekday"].ToString();

                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return Json(features);
        }

        [HttpPost]
        public JsonResult GetPrediction(
            double bld_avg_Above_ground_floors,
            double bld_avg_building_Area,
            double bld_avg_construction_year,
            double bld_avg_lot_Area,
            double bld_avg_property_count,

            string bld_median_building_type,
            string bld_median_catagorie_building,

            double bp_Apprentice_or_trade_school_diploma,
            double bp_Average_Age,
            double bp_Average_Household_sizepersons,
            double bp_Average_Income,
            double bp_Buildings_with_5_or_more_floors,
            double bp_Buildings_with_less_than_5_floors,
            double bp_College,
            double bp_construction_after_2000,
            double bp_construction_between_1981_and_2000,
            double bp_Contruction_before1980,
            double bp_Couple_with_Children,
            double bp_Couple_without_children,
            double bp_firestation_count,
            double bp_Immigrant_population,
            double bp_Language_English_Spoken,
            double bp_Language_French_Spoken,
            double bp_Less_Than_50000,
            double bp_Mobile_homes,
            double bp_No_Diploma,
            double bp_Non_immigrant_population,
            double bp_Other_Language_Spoken,
            double bp_Owners,
            double bp_Population,
            double bp_Population_density,
            double bp_Renters,
            double bp_Secondaryhighschool,
            double bp_Semi_detached_or_row_houses,
            double bp_Single_Family_Homes,
            double bp_Single_Parent_Families,
            double bp_Unemployment_rate,
            double bp_University,
            double crime_db_count,
            double da_area,
            double da_Firestation_count,
            double da_pop_density,
            double da_population,
            double da_total_private_dwellings,
            double db_area,
            double db_density,
            double db_population,
            double db_tot_Private_dwellings,
            string iw_weather_str,
            string Month,
            double nbr_comment_311,
            double nbr_complain_311,
            double nbr_request_311,
            string Part_of_Day,
            string season,
            string Weekday)
        {
            try
            {
                var payloadArg = String.Format("{'bld_avg_Above_ground_floors':{0},"+
                                                "'bld_avg_building_Area':{1}," +
                                                "'bld_avg_construction_year':{2}," +
                                                "'bld_avg_lot_Area':{3}," +
                                                "'bld_avg_property_count':{4}," +
                                                "'bld_median_building_type':{5}," +
                                                "'bld_median_catagorie_building':{6}," +
                                                "'bp_Apprentice_or_trade_school_diploma':{7}," +
                                                "'bp_Average_Age':{8}," +
                                                "'bp_Average_Household_sizepersons':{9}," +
                                                "'bp_Average_Income':{10}," +
                                                "'bp_Buildings_with_5_or_more_floors':{11}," +
                                                "'bp_Buildings_with_less_than_5_floors':{12}," +
                                                "'bp_College':{13}," +
                                                "'bp_construction_after_2000':{14}," +
                                                "'bp_construction_between_1981_and_2000':{15}," +
                                                "'bp_Contruction_before1980':{16}," +
                                                "'bp_Couple_with_Children':{17}," +
                                                "'bp_Couple_without_children':{18}," +
                                                "'bp_firestation_count':{19}," +
                                                "'bp_Immigrant_population':{20}," +
                                                "'bp_Language_English_Spoken':{21}," +
                                                "'bp_Language_French_Spoken':{22}," +
                                                "'bp_Less_Than_50000':{23}," +
                                                "'bp_Mobile_homes':{24}," +
                                                "'bp_No_Diploma':{25}," +
                                                "'bp_Non_immigrant_population':{26}," +
                                                "'bp_Other_Language_Spoken':{27}," +
                                                "'bp_Owners':{28}," +
                                                "'bp_Population':{29}," +
                                                "'bp_Population_density':{30}," +
                                                "'bp_Renters':{31}," +
                                                "'bp_Secondaryhighschool':{32}," +
                                                "'bp_Semi_detached_or_row_houses':{33}," +
                                                "'bp_Single_Family_Homes':{34}," +
                                                "'bp_Single_Parent_Families':{35}," +
                                                "'bp_Unemployment_rate':{36}," +
                                                "'bp_University':{37}," +
                                                "'crime_db_count':{38}," +
                                                "'da_area':{39}," +
                                                "'da_Firestation_count':{40}," +
                                                "'da_pop_density':{41}," +
                                                "'da_population':{42}," +
                                                "'da_total_private_dwellings':{43}," +
                                                "'db_area':{44}," +
                                                "'db_density':{45}," +
                                                "'db_population':{46}," +
                                                "'db_tot_Private_dwellings':{47}," +
                                                "'iw_weather_str':{48}," +
                                                "'Month':{49}," +
                                                "'nbr_comment_311':{50}," +
                                                "'nbr_complain_311':{51}," +
                                                "'nbr_request_311':{52}," +
                                                "'Part_of_Day':{53}," +
                                                "'season':{54}," +
                                                "'Weekday':{55}}", bld_avg_Above_ground_floors,
                                                                    bld_avg_building_Area,
                                                                    bld_avg_construction_year,
                                                                    bld_avg_lot_Area,
                                                                    bld_avg_property_count,
                                                                    bld_median_building_type,
                                                                    bld_median_catagorie_building,
                                                                    bp_Apprentice_or_trade_school_diploma,
                                                                    bp_Average_Age,
                                                                    bp_Average_Household_sizepersons,
                                                                    bp_Average_Income,
                                                                    bp_Buildings_with_5_or_more_floors,
                                                                    bp_Buildings_with_less_than_5_floors,
                                                                    bp_College,
                                                                    bp_construction_after_2000,
                                                                    bp_construction_between_1981_and_2000,
                                                                    bp_Contruction_before1980,
                                                                    bp_Couple_with_Children,
                                                                    bp_Couple_without_children,
                                                                    bp_firestation_count,
                                                                    bp_Immigrant_population,
                                                                    bp_Language_English_Spoken,
                                                                    bp_Language_French_Spoken,
                                                                    bp_Less_Than_50000,
                                                                    bp_Mobile_homes,
                                                                    bp_No_Diploma,
                                                                    bp_Non_immigrant_population,
                                                                    bp_Other_Language_Spoken,
                                                                    bp_Owners,
                                                                    bp_Population,
                                                                    bp_Population_density,
                                                                    bp_Renters,
                                                                    bp_Secondaryhighschool,
                                                                    bp_Semi_detached_or_row_houses,
                                                                    bp_Single_Family_Homes,
                                                                    bp_Single_Parent_Families,
                                                                    bp_Unemployment_rate,
                                                                    bp_University,
                                                                    crime_db_count,
                                                                    da_area,
                                                                    da_Firestation_count,
                                                                    da_pop_density,
                                                                    da_population,
                                                                    da_total_private_dwellings,
                                                                    db_area,
                                                                    db_density,
                                                                    db_population,
                                                                    db_tot_Private_dwellings,
                                                                    iw_weather_str,
                                                                    Month,
                                                                    nbr_comment_311,
                                                                    nbr_complain_311,
                                                                    nbr_request_311,
                                                                    Part_of_Day,
                                                                    season,
                                                                    Weekday);


            }
            catch (Exception ex)
            {
                return null;
            }

            return Json("");
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

                var response = predictionServiceClient.Predict(request);
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
