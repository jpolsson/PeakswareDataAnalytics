
using TrainingPeaks.DataAnalytics.Core.DataContext;
using TrainingPeaks.DataAnalytics.Core.Models;

namespace TrainingPeaks.DataAnalytics.Core.UnitTests.DataContext
{
    internal class JsonDataContextShould
    {
        private JsonDataContext _context;
        [SetUp]
        public void Setup()
        {
            // create JsonDataContext instance
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string testDataPath = Path.Combine(projectDirectory, "..", "..", "..", "TestData");
            _context = new JsonDataContext(testDataPath);
        }
#region TotalWeightTests
        [Test]
        public void CorrectlyCalculateTotalWeightWithNoFilters()
        {
            //Arrange
            var expectedTotalWeight = 56460;

            //Act
            var totalWeight = _context.TotalWeight();

            //Assert
            Assert.That(totalWeight, Is.EqualTo(expectedTotalWeight));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightFilterByUser()
        {
            //Arrange
            var expectedTotalWeight = 20070;
            var userId = 5101;
            var filter = new DataFilter { UserId = userId };

            //Act
            var totalWeight = _context.TotalWeight(filter);

            //Assert
            Assert.That(totalWeight, Is.EqualTo(expectedTotalWeight));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightFilterByExercise()
        {
            //Arrange
            var expectedTotalWeight = 33145;
            var exerciseId = 326;
            var filter = new DataFilter { ExerciseId = exerciseId };

            //Act
            var totalWeight = _context.TotalWeight(filter);

            //Assert
            Assert.That(totalWeight, Is.EqualTo(expectedTotalWeight));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightFilterByDateRange()
        {
            //Arrange
            var expectedTotalWeight = 28390;
           
            var dateRange = new DateRange(new DateTime(2018,6,3), new DateTime(2018, 6, 12,23,59,59));            
            var filter = new DataFilter { DateRange = dateRange };

            //Act
            var totalWeight = _context.TotalWeight(filter);

            //Assert
            Assert.That(totalWeight, Is.EqualTo(expectedTotalWeight));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightWithCombinedFilters()
        {
            //Arrange
            var expectedTotalWeight = 14810;
            var exerciseId = 326;
            var userId = 29891;
            var dateRange = new DateRange(new DateTime(2018, 6, 3), new DateTime(2018, 6, 12, 23, 59, 59));
            var filter = new DataFilter
            {
                ExerciseId = exerciseId,
                UserId = userId,
                DateRange = dateRange,
            };

            //Act
            var totalWeight = _context.TotalWeight(filter);

            //Assert
            Assert.That(totalWeight, Is.EqualTo(expectedTotalWeight));
        }
        #endregion

#region TotalWeightByMonthTests
        [Test]
        public void CorrectlyCalculateTotalWeightByMonthWithNoFilters()
        {
            //Arrange
            var expectedListCount = 5;
            var expectedJan2016Total = 6490;

            //Act
            var totalWeightByMonth = _context.TotalWeightByMonth();

            //Assert
            Assert.That(totalWeightByMonth.Count, Is.EqualTo(expectedListCount));
            var jan2016Total = totalWeightByMonth
             .Where(c => c.Year == 2016 && c.Month == 1)
             .Select(m => m.TotalWeight)
             .FirstOrDefault();

            Assert.That(jan2016Total, Is.EqualTo(expectedJan2016Total));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightByMonthWithFilterByUser()
        {
            //Arrange
            var expectedListCount = 2;
            var expectedNov2017Total = 10925;
            var userId = 9705;
            var filter = new DataFilter { UserId = userId };

            //Act
            var totalWeightByMonth = _context.TotalWeightByMonth(filter);

            //Assert
            Assert.That(totalWeightByMonth.Count, Is.EqualTo(expectedListCount));
            var nov2017Total = totalWeightByMonth
             .Where(c => c.Year == 2017 && c.Month == 11)
             .Select(m => m.TotalWeight)
             .FirstOrDefault();

            Assert.That(nov2017Total, Is.EqualTo(expectedNov2017Total));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightByMonthWithFilterByExercise()
        {
            //Arrange
            var expectedListCount = 4;
            var expectedJuly2018Total = 7500;
            var exerciseId = 797;
            var filter = new DataFilter { ExerciseId = exerciseId };

            //Act
            var totalWeightByMonth = _context.TotalWeightByMonth(filter);

            //Assert
            Assert.That(totalWeightByMonth.Count, Is.EqualTo(expectedListCount));
            var july2018Total = totalWeightByMonth
             .Where(c => c.Year == 2018 && c.Month == 7)
             .Select(m => m.TotalWeight)
             .FirstOrDefault();

            Assert.That(july2018Total, Is.EqualTo(expectedJuly2018Total));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightByMonthWithFilterByDateRange()
        {
            //Arrange
            var expectedListCount = 2;
            var expectedNov2017Total = 10925;
            var dateRange = new DateRange(new DateTime(2016, 1, 25), new DateTime(2017, 11, 20, 23, 59, 59));
            var filter = new DataFilter { DateRange = dateRange };

            //Act
            var totalWeightByMonth = _context.TotalWeightByMonth(filter);

            //Assert
            Assert.That(totalWeightByMonth.Count, Is.EqualTo(expectedListCount));
            var nov2017Total = totalWeightByMonth
             .Where(c => c.Year == 2017 && c.Month == 11)
             .Select(m => m.TotalWeight)
             .FirstOrDefault();

            Assert.That(nov2017Total, Is.EqualTo(expectedNov2017Total));
        }

        [Test]
        public void CorrectlyCalculateTotalWeightByMonthWithCombinedFilters()
        {
            //Arrange
            var expectedListCount = 1;
            var expectedJune2018Total = 14810;
            var exerciseId = 326;
            var userId = 29891;
            var dateRange = new DateRange(new DateTime(2017, 11, 20), new DateTime(2018, 6, 12, 23, 59, 59));
            var filter = new DataFilter
            {
                ExerciseId = exerciseId,
                UserId = userId,
                DateRange = dateRange
            };

            //Act
            var totalWeightByMonth = _context.TotalWeightByMonth(filter);

            //Assert
            Assert.That(totalWeightByMonth.Count, Is.EqualTo(expectedListCount));
            var june2018Total = totalWeightByMonth
             .Where(c => c.Year == 2018 && c.Month == 6)
             .Select(m => m.TotalWeight)
             .FirstOrDefault();

            Assert.That(june2018Total, Is.EqualTo(expectedJune2018Total));
        }
        #endregion

#region WeightTests
        [Test]
        public void CorrectlyFetchWeightsWithNoFilters()
        {
            //Arrange
            var expectedListCount = 40;
            var expectedMax = 350;

            //Act
            var weights = _context.Weight();

            //Assert
            Assert.That(weights.Count, Is.EqualTo(expectedListCount));
            var max = weights.Max();

            Assert.That(max, Is.EqualTo(expectedMax));
        }

        [Test]
        public void CorrectlyFetchWeightsWithFilterByUser()
        {
            //Arrange
            var expectedListCount = 15;
            var expectedMax = 285;
            var userId = 9705;
            var filter = new DataFilter { UserId = userId };

            //Act
            var weights = _context.Weight(filter);

            //Assert
            Assert.That(weights.Count, Is.EqualTo(expectedListCount));
            var max = weights.Max();

            Assert.That(max, Is.EqualTo(expectedMax));
        }

        [Test]
        public void CorrectlyFetchWeightsWithFilterByExercise()
        {
            //Arrange
            var expectedListCount = 7;
            var expectedMax = 275;
            var exerciseId = 568;
            var filter = new DataFilter { ExerciseId = exerciseId };

            //Act
            var weights = _context.Weight(filter);

            //Assert
            Assert.That(weights.Count, Is.EqualTo(expectedListCount));
            var max = weights.Max();

            Assert.That(max, Is.EqualTo(expectedMax));
        }

        [Test]
        public void CorrectlyFetchWeightsWithFilterByDateRange()
        {
            //Arrange
            var expectedListCount = 21;
            var expectedMax = 350;
            var dateRange = new DateRange(new DateTime(2018, 6, 3), new DateTime(2018, 7, 3, 23, 59, 59));
            var filter = new DataFilter { DateRange = dateRange };

            //Act
            var weights = _context.Weight(filter);

            //Assert
            Assert.That(weights.Count, Is.EqualTo(expectedListCount));
            var max = weights.Max();

            Assert.That(max, Is.EqualTo(expectedMax));
        }

        [Test]
        public void CorrectlyFetchWeightsWithCombinedFilters()
        {
            //Arrange
            var expectedListCount = 2;
            var expectedMax = 275;
            var exerciseId = 568;
            var userId = 5101;
            var dateRange = new DateRange(new DateTime(2017, 11, 20), new DateTime(2018, 6, 12, 23, 59, 59));
            var filter = new DataFilter
            {
                ExerciseId = exerciseId,
                UserId = userId,
                DateRange = dateRange
            };

            //Act
            var weights = _context.Weight(filter);

            //Assert
            Assert.That(weights.Count, Is.EqualTo(expectedListCount));
            var max = weights.Max();

            Assert.That(max, Is.EqualTo(expectedMax));
        }

        #endregion

        #region Error Tests

        [Test]
        public void ThrowFileNotFoundErrorWhenPassedInvalidPath()
        {
            //Arrange
            string invalidPath = "C:\\";

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => new JsonDataContext(invalidPath));

            // Verify exception message to ensure it's about the missing file
            Assert.That(exception.Message, Does.Contain("does not exist"));
        }
        #endregion
    }
}
