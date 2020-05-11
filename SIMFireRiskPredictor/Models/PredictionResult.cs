using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMFireRiskPredictor.Models
{
    public class PredictionResult
    {
        public double Prediction { get; set; }
        public double CI_S { get; set; }
        public double CI_E { get; set; }
        public string Category { get; set; }
    }
}
