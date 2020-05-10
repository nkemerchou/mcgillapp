project_id = 'firebird-276623'
compute_region = 'us-central1'
model_display_name = 'Tryagain'
payload = {'bld_avg_Above_ground_floors':1.92857142857142,	'bld_avg_building_Area':167.571428571428,	'bld_avg_lot_Area':354.9375,	'bld_avg_property_count':1.64285714285714,	'bld_median_building_type':1000,	'bld_median_catagorie_building':0,	'bp_Average_Age':44,	'bp_Average_Income':67517,	'bp_Buildings_with_5_or_more_floors':4,	'bp_Buildings_with_less_than_5_floors':34,	'bp_College':19,	'bp_construction_after_2000':6,	'bp_construction_between_1981_and_2000':16,	'bp_Couple_without_children':36,	'bp_firestation_count':1,	'bp_Immigrant_population':16,	'bp_Language_French_Spoken':33,	'bp_Mobile_homes':0,	'bp_Population':4958,	'bp_Population_density':473,	'bp_Secondaryhighschool':24,	'bp_Semi_detached_or_row_houses':18,	'bp_Unemployment_rate':8,	'crime_db_count':1,	'da_area':0.1341,	'da_Firestation_count':0,	'da_pop_density':3795.67486950037,	'da_population':509,	'db_area':0.0228,	'db_density':1315.78947368421,	'db_population':30,	'iw_weather_str':'6',	'Month':'4',	'nbr_comment_311':0,	'nbr_complain_311':0,	'nbr_request_311':0,	'Part_of_Day':'2',	'season':'1',	'Weekday':'0'}

from google.cloud import automl_v1beta1 as automl

client = automl.TablesClient(project=project_id, region=compute_region)

response = client.predict(model_display_name=model_display_name,inputs=payload,feature_importance=False)

print(response)