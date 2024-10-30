using Newtonsoft.Json;
using Serilog;
using System.Globalization;
using TrainingPeaks.DataAnalytics.ConsoleApp.Models;
using TrainingPeaks.DataAnalytics.Core.DataContext;
using TrainingPeaks.DataAnalytics.Core.Models;

namespace TrainingPeaks.DataAnalytics.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                //.MinimumLevel.Override("System.Net.Http.HttpClient", Serilog.Events.LogEventLevel.Warning) // Filter out HttpClient logs below Warning
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            AnswerQuestions();
        }

        static void AnswerQuestions()
        {
            try
            {
                Log.Information("Create Json DataContext");
                string _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data");
                JsonDataContext context = new JsonDataContext(_dataFilePath);
                if (context != null) {
                    AnswerOutput answerOutput = new AnswerOutput();
                    
                    // How many total pounds have all of these athletes combined Bench Pressed?
                    answerOutput.Question1 = context.TotalWeight();

                    // How many total pounds did Barry Moore Back Squat in 2016?
                    var userId = context.Users?.SingleOrDefault(u => u.FirstName == "Barry" && u.LastName == "Moore")?.Id;
                    var exerciseId = context.Exercises?.SingleOrDefault(e => e.Title == "Back Squat")?.Id;
                    var daterange = new DateRange(new DateTime(2016, 1, 1), new DateTime(2016, 12, 31,23,59,59));
                    DataFilter filter = new DataFilter{
                        ExerciseId = exerciseId,
                        UserId = userId,
                        DateRange = daterange,
                    };
                    answerOutput.Question2 = context.TotalWeight(filter);

                    // In what month of 2017 did Barry Moore Back Squat the most total weight?
                    filter.DateRange = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 12, 31,23,59,59));
                    var monthData = context.TotalWeightByMonth(filter);
                    var monthWithHighestWeight = (int)(monthData.MaxBy(m => m.TotalWeight)?.Month ?? 0);
                    var year = (int)(monthData.MaxBy(m => m.TotalWeight)?.Year ?? 0);
                    var month = CultureInfo.CurrentCulture.
                                                DateTimeFormat.GetAbbreviatedMonthName(monthWithHighestWeight);
                    answerOutput.Question3 = $"{month} {year}";
                    // What is Abby Smith's all-time Bench Press PR weight?
                    filter.ExerciseId = context.Exercises?.SingleOrDefault(e => e.Title == "Bench Press")?.Id;
                    filter.UserId = context.Users?.SingleOrDefault(u => u.FirstName == "Abby" && u.LastName == "Smith")?.Id;
                    filter.DateRange = null;
                    var weights = context.Weight(filter);
                    answerOutput.Question4 = weights.Max();
                    Console.WriteLine(JsonConvert.SerializeObject(answerOutput));
                }

            }
            catch (Exception ex)
            {

                Log.Error($"Unexpected Error in AnswerQuestions: {ex.Message}", ex);
            }


        }
    }
}
