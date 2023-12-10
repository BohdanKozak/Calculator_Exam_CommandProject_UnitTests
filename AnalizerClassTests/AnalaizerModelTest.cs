using AnalaizerClassLibrary;
using GraphInterface;
using MySql.Data.MySqlClient;

namespace AnalizerClassTests
{
    [TestClass]
    public class AnalaizerModelTest
    {
        string connectionString = "server=localhost;port=3306;database=sample;uid=root;password=10Gejhupov!";
        List<CalculatorValue> list;
        [TestMethod]
        public void Format_ValidTextShouldBeFormatted()
        {
            string newExpressionStr;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                // DbConnection that is already opened
                using (CalculatorDbContext context = new CalculatorDbContext(connection, false))
                {
                    list = context.CalculatorValues.ToList<CalculatorValue>();
                    AnalaizerClass.expression = list[0].Value;//"1 + 1"
                    // Act 
                    newExpressionStr = AnalaizerClass.Format();
                }
            }

            // Assert
            Assert.AreEqual(list[0].Expected, newExpressionStr); //"1+1"

        }

        [TestMethod]
        public void Format_EmptyTextShouldReturnEmptyString()
        {
            // Arrange
            AnalaizerClass.expression = list[1].Value;//""

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual(list[1].Expected, newExpressionStr);//""
        }

        private const int MAX_LENGHT_EXPRESSION = 65536;
        [TestMethod]
        public void Format_TextExceedsMaxLengthShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = list[2].Value + new string('1', MAX_LENGHT_EXPRESSION);//"1234567890"

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual(list[2].Expected, newExpressionStr);

        }

        [TestMethod]
        public void Format_InvalidStartCharacterShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = list[3].Value;//"#1 + 1"

            // Act 
            var newExpressionStr = AnalaizerClass.Format();


            Assert.AreEqual(list[3].Expected, newExpressionStr);
        }

        [TestMethod]
        public void Format_TwoOperatorsInARowShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = list[4].Value;//"1 + + 1"

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual(list[4].Expected, newExpressionStr);

        }

        [TestMethod]
        public void Format_UnfinishedExpressionShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = list[5].Value;//"1 + "

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual(list[5].Expected, newExpressionStr);
        }
    }
}