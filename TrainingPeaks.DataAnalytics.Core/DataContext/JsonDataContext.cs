using Newtonsoft.Json;
using Serilog;
using System.Reflection.Metadata.Ecma335;
using TrainingPeaks.DataAnalytics.Core.Models;

namespace TrainingPeaks.DataAnalytics.Core.DataContext
{
    public class JsonDataContext
    {
        public readonly List<JsonWorkout>? JsonWorkouts;
        public readonly List<User>? Users;
        public readonly List<Exercise>? Exercises;
        private readonly string _exerciseFileName = "exercises.json";
        private readonly string _userFileName = "users.json";
        private readonly string _workoutFileName = "workouts.json";
        public JsonDataContext(string pathToJsonData)
        {
            // Load Users
            Log.Information("Load Users");
            string userJson = ReadJsonData($"{pathToJsonData}\\{_userFileName}") ?? string.Empty;
            Users = JsonConvert.DeserializeObject<List<User>>(userJson);
            if (Users == null) throw new Exception("User Data is null");

            // Load Exercises
            Log.Information("Load Exercises");
            string exerciseJson = ReadJsonData($"{pathToJsonData}\\{_exerciseFileName}") ?? string.Empty;
            Exercises = JsonConvert.DeserializeObject<List<Exercise>>(exerciseJson);
            if (Exercises == null) throw new Exception("Exercise Data is null");

            //Load WorkoutData
            Log.Information("Load Workouts");
            string jsonData = ReadJsonData($"{pathToJsonData}\\{_workoutFileName}") ?? string.Empty;
            JsonWorkouts = JsonConvert.DeserializeObject<List<JsonWorkout>>(jsonData);
            // Ensure data is valid
            if (JsonWorkouts == null) throw new Exception("Workout Data is null");
        }

        private string ReadJsonData(string pathToJsonData)
        {
            // Ensure the file exists
            if (!File.Exists(pathToJsonData))
            {
                throw new FileNotFoundException($"The file: {pathToJsonData} does not exist.");
            }

            return File.ReadAllText(pathToJsonData);
        }

        public int  TotalWeight(DataFilter? filter = null)
        {
            int returnValue = 0;
            try
            {
                var query = JsonWorkouts?.AsQueryable();

                // Apply filter for UserId if provided
                if ((filter?.UserId ?? null).HasValue)
                {
                    query = query?.Where(workout => workout.UserId == filter.UserId);
                }
                // Apply filter for DateRange if provided
                if (filter?.DateRange != null)
                {
                    query = query?.Where(workout =>
                        workout.DateTimeCompleted >= filter.DateRange.Start &&
                        workout.DateTimeCompleted <= filter.DateRange.End);
                }
                // Flatten to blocks and apply filter for ExerciseId if provided
                var blockQuery = query?.SelectMany(workout => workout.Blocks);

                if ((filter?.ExerciseId??null).HasValue)
                {
                    blockQuery = blockQuery?.Where(block => block.ExerciseId == filter.ExerciseId);
                }


                // Flatten to sets and calculate the sum
                returnValue = blockQuery
                    .SelectMany(block => block.Sets)
                    .Sum(set => set.Reps * set.Weight);
            }
            catch (Exception ex)
            {

                throw new Exception($"Unexpected error in Total Pounds: {ex.Message}", ex);
            }

            return returnValue;
        }

        public List<MonthTotalWeight> TotalWeightByMonth(DataFilter? filter = null)
        {
            List<MonthTotalWeight> monthTotalWeights = new List<MonthTotalWeight>();

            try
            {
                var query = JsonWorkouts?.AsQueryable();

                // Apply filter for UserId if provided
                if ((filter?.UserId ?? null).HasValue)
                {
                    query = query?.Where(workout => workout.UserId == filter.UserId);
                }

                // Apply filter for DateRange if provided
                if (filter?.DateRange != null)
                {
                    query = query?.Where(workout =>
                        workout.DateTimeCompleted >= filter.DateRange.Start &&
                        workout.DateTimeCompleted <= filter.DateRange.End);
                }

                // Flatten to blocks and apply filter for ExerciseId if provided
                var blockQuery = query?.SelectMany(workout => workout.Blocks, (workout, block) => new
                {
                    WorkoutYear = workout.DateTimeCompleted.Year,
                    WorkoutMonth = workout.DateTimeCompleted.Month,
                    Block = block
                });

                if (filter?.ExerciseId.HasValue == true)
                {
                    blockQuery = blockQuery?.Where(b => b.Block.ExerciseId == filter.ExerciseId.Value);
                }

                // Flatten to sets, group by month, and calculate the total weight for each month
                monthTotalWeights = blockQuery
                    .SelectMany(b => b.Block.Sets, (b, set) => new
                    {
                        Year = b.WorkoutYear,
                        Month = b.WorkoutMonth,
                        TotalWeight = set.Reps * set.Weight
                    })
                    .GroupBy(x => new { x.Year, x.Month })
                    .Select(g => new MonthTotalWeight
                    {   
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalWeight = g.Sum(x => x.TotalWeight)
                    })
                    .OrderBy(m => m.Month)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error in Total Weight calculation: {ex.Message}", ex);
            }

            return monthTotalWeights;
        }

        public List<int> Weight(DataFilter? filter = null)
        {
            List<int> weights = new List<int>();

            try
            {
                var query = JsonWorkouts?.AsQueryable();

                // Apply filter for UserId if provided
                if ((filter?.UserId ?? null).HasValue)
                {
                    query = query?.Where(workout => workout.UserId == filter.UserId);
                }

                // Apply filter for DateRange if provided
                if (filter?.DateRange != null)
                {
                    query = query?.Where(workout =>
                        workout.DateTimeCompleted >= filter.DateRange.Start &&
                        workout.DateTimeCompleted <= filter.DateRange.End);
                }

                // Flatten to blocks and apply filter for ExerciseId if provided
                var blockQuery = query?.SelectMany(workout => workout.Blocks, (workout, block) => new
                {
                    WorkoutMonth = workout.DateTimeCompleted.Month,
                    Block = block
                });

                if (filter?.ExerciseId.HasValue == true)
                {
                    blockQuery = blockQuery?.Where(b => b.Block.ExerciseId == filter.ExerciseId.Value);
                }

                // Flatten to sets, group by month, and calculate the total weight for each month
                weights = blockQuery
                    .SelectMany(b => b.Block.Sets)
                    .Where(set => set.Reps > 0)           // Filter out sets where Reps are 0
                    .Select(set => set.Weight)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error in Total Weight calculation: {ex.Message}", ex);
            }

            return weights;
        }
    }
}
