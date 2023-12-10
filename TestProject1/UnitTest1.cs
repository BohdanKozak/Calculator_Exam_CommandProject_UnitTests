using AnalaizerClassLibrary;
using ErrorLibrary;


namespace TestProject1
{
    [TestClass]

    public class AnalaizerModelTest
    {
        public TestContext TestContext { get; set; }
        [TestMethod]

        [DataSource("System.Data.SqlClient", "Server = .;Integrated Security = True;Database = CalculatorDbTest", "CalcValues", DataAccessMethod.Sequential)]

        public void Format_ValidTextShouldBeFormatted()
        {
            // Arrange
            AnalaizerClass.expression = (string)TestContext.DataRow["Values"];

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual("1+1", newExpressionStr);
        }

        [TestMethod]
        public void Format_EmptyTextShouldReturnEmptyString()
        {
            // Arrange
            AnalaizerClass.expression = "";

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual("", newExpressionStr);
        }

        private const int MAX_LENGHT_EXPRESSION = 65536;
        [TestMethod]
        public void Format_TextExceedsMaxLengthShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = "1234567890" + new string('1', MAX_LENGHT_EXPRESSION);

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual("&" + ErrorsExpression.ERROR_07, newExpressionStr);
        }

        [TestMethod]
        public void Format_InvalidStartCharacterShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = "#1 + 1";

            // Act 
            var newExpressionStr = AnalaizerClass.Format();


            Assert.AreEqual("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_02, 0), newExpressionStr);
        }

        [TestMethod]
        public void Format_TwoOperatorsInARowShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = "1 + + 1";

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_04, 2), newExpressionStr);
        }

        [TestMethod]
        public void Format_UnfinishedExpressionShouldReturnError()
        {
            // Arrange
            AnalaizerClass.expression = "1 + ";

            // Act 
            var newExpressionStr = AnalaizerClass.Format();

            // Assert
            Assert.AreEqual("&" + ErrorsExpression.ERROR_05, newExpressionStr);
        }
    }
}