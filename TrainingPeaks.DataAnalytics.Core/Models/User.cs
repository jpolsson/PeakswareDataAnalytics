
using Newtonsoft.Json;

namespace TrainingPeaks.DataAnalytics.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        [JsonProperty("name_first")]
        public string FirstName { get; set; } = null!;
        [JsonProperty("name_last")]
        public string LastName { get; set; } = null!;
    }
}
