
using Newtonsoft.Json;

namespace TrainingPeaks.DataAnalytics.Core.Models
{
    public class JsonSet
    {
        [JsonProperty("reps")]
        public int Reps { get; set; }
        [JsonProperty("weight")]
        public int? WeightRaw { get; set; }
        public int Weight => WeightRaw ?? 0;
    }
}
