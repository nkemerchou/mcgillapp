using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMFireRiskPredictor.Models
{
    public class Features
    {
        public double bld_avg_Above_ground_floors { get; set; }
        public double bld_avg_building_Area { get; set; }
        public double bld_avg_construction_year { get; set; }
        public double bld_avg_lot_Area { get; set; }
        public double bld_avg_property_count { get; set; }

        public string bld_median_building_type { get; set; }
        public string bld_median_catagorie_building { get; set; }

        public double bp_Apprentice_or_trade_school_diploma { get; set; }
        public double bp_Average_Age { get; set; }
        public double bp_Average_Household_sizepersons { get; set; }
        public double bp_Average_Income { get; set; }
        public double bp_Buildings_with_5_or_more_floors { get; set; }
        public double bp_Buildings_with_less_than_5_floors { get; set; }
        public double bp_College { get; set; }
        public double bp_construction_after_2000 { get; set; }
        public double bp_construction_between_1981_and_2000 { get; set; }
        public double bp_Contruction_before1980 { get; set; }
        public double bp_Couple_with_Children { get; set; }
        public double bp_Couple_without_children { get; set; }
        public double bp_firestation_count { get; set; }
        public double bp_Immigrant_population { get; set; }
        public double bp_Language_English_Spoken { get; set; }
        public double bp_Language_French_Spoken { get; set; }
        public double bp_Less_Than_50000 { get; set; }
        public double bp_Mobile_homes { get; set; }
        public double bp_No_Diploma { get; set; }
        public double bp_Non_immigrant_population { get; set; }
        public double bp_Other_Language_Spoken { get; set; }
        public double bp_Owners { get; set; }
        public double bp_Population { get; set; }
        public double bp_Population_density { get; set; }
        public double bp_Renters { get; set; }
        public double bp_Secondaryhighschool { get; set; }
        public double bp_Semi_detached_or_row_houses { get; set; }
        public double bp_Single_Family_Homes { get; set; }
        public double bp_Single_Parent_Families { get; set; }
        public double bp_Unemployment_rate { get; set; }
        public double bp_University { get; set; }
        public double crime_db_count { get; set; }
        public double da_area { get; set; }
        public double da_Firestation_count { get; set; }
        public double da_pop_density { get; set; }
        public double da_population { get; set; }
        public double da_total_private_dwellings { get; set; }
        public double db_area { get; set; }
        public double db_density { get; set; }
        public double db_population { get; set; }
        public double db_tot_Private_dwellings { get; set; }
        public string iw_weather_str { get; set; }
        public string Month { get; set; }
        public double nbr_comment_311 { get; set; }
        public double nbr_complain_311 { get; set; }
        public double nbr_request_311 { get; set; }
        public string Part_of_Day { get; set; }
        public string season { get; set; }
        public string Weekday { get; set; }

    }
}
