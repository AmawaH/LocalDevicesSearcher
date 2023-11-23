using Moq;
using LocalDevicesSearcher.Infrastructure.Logger;

namespace LocalDevicesSearcherTest
{
    public class LoggerTests
    {
        [Fact]
        public void Log_WritesToConsoleAndLogFile_WhenCanLogInFileIsTrue()
        {
            // Arrange
            var logToConsoleServiceMock = new Mock<ILogToConsoleService>();
            var logToFileServiceMock = new Mock<ILogToFileService>();

            var logger = new Logger(logToConsoleServiceMock.Object, logToFileServiceMock.Object);

            logger.CreateLogFile("testLogFile");

            // Act
            logger.Log("Test log message");

            // Assert
            logToConsoleServiceMock.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Once);
            logToFileServiceMock.Verify(x => x.WriteToLogFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        //[Fact]
        //public void Log_WritesOnlyToConsole_WhenCanLogInFileIsFalse()
        //{
        //    // Arrange
        //    var logToConsoleServiceMock = new Mock<ILogToConsoleService>();
        //    var logToFileServiceMock = new Mock<ILogToFileService>();

        //    var logger = new Logger(logToConsoleServiceMock.Object, logToFileServiceMock.Object);
        //    logger.CreateLogFile("testLogFile");
        //    logger.DisableLogToFileLogging(); 

        //    // Act
        //    logger.Log("Test log message");

        //    // Assert
        //    logToConsoleServiceMock.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Once);
        //    logToFileServiceMock.Verify(x => x.WriteToLogFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        //}
    }
}
