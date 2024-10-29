
using Newtonsoft.Json;
using System.Reflection;

namespace TrainingPeaks.DataAnalytics.Core.Models
{
    public class JsonWorkout
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("datetime_completed")]
        public DateTime DateTimeCompleted { get; set; }
        [JsonProperty("blocks")]
        public List<Block> Blocks { get; set; } = null!;
    }
}
