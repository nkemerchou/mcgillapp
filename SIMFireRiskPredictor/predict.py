project_id = 'firebird-276623'
compute_region = 'us-central1'
model_display_name = 'Tryagain_20200508081549'

from google.cloud import automl_v1beta1 as automl
import sys
import json
from json.decoder import JSONDecoder

input = str(sys.argv[1])
payload = JSONDecoder().decode(input)

client = automl.TablesClient(project=project_id, region=compute_region)


response = client.predict(model_display_name=model_display_name,inputs=payload,feature_importance=False)
print(response)
#print(input)
#print(JSONDecoder().decode(input))