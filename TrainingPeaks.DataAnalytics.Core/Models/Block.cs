
using Newtonsoft.Json;

namespace TrainingPeaks.DataAnalytics.Core.Models
{
    public class Block
    {
        [JsonProperty("exercise_id")]
        public int ExerciseId { get; set; }
        [JsonProperty("sets")]
        public List<JsonSet> Sets { get; set; } = null!;
    }
}
