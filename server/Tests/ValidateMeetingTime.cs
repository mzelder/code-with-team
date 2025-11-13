using api.Services;
using api.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using api.Utils;

namespace Tests
{
    public class ValidateMeetingTimeTests
    {
        private ChatService CreateChatService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var timeHelper = new MeetingTimeHelper();
            return new ChatService(context, timeHelper);
        }

        [Fact]
        public void IsValidMeetingTime_WithValidFutureDate_ReturnsTrue()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:{tomorrow.Hour:D2}:{tomorrow.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        #region Format Validation - Part Count

        [Fact]
        public void IsValidMeetingTime_WithLessThan4Parts_ThrowsFormatException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => service.IsValidMeetingTime("11:08:14"));
            Assert.Contains("Invalid meeting time format", exception.Message);
        }

        [Fact]
        public void IsValidMeetingTime_WithOnly1Part_ThrowsFormatException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => service.IsValidMeetingTime("11"));
            Assert.Contains("Invalid meeting time format", exception.Message);
        }

        [Fact]
        public void IsValidMeetingTime_WithMoreThan4Parts_ThrowsFormatException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => service.IsValidMeetingTime("11:08:14:30:00"));
            Assert.Contains("Invalid meeting time format", exception.Message);
        }

        [Fact]
        public void IsValidMeetingTime_WithExactly4Parts_ProcessesCorrectly()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:15:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert - Should not throw, result depends on date validation
            Assert.IsType<bool>(result);
        }

        #endregion

        #region Format Validation - Non-Numeric Parts

        [Fact]
        public void IsValidMeetingTime_WithNonNumericMonth_ThrowsFormatException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => service.IsValidMeetingTime("AA:09:14:30"));
            Assert.Contains("All parts must be numbers", exception.Message);
        }

        [Fact]
        public void IsValidMeetingTime_WithNonNumericDay_ThrowsFormatException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => service.IsValidMeetingTime("11:BB:14:30"));
            Assert.Contains("All parts must be numbers", exception.Message);
        }

        [Fact]
        public void IsValidMeetingTime_WithNonNumericHour_ThrowsFormatException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => service.IsValidMeetingTime("11:09:CC:30"));
            Assert.Contains("All parts must be numbers", exception.Message);
        }

        [Fact]
        public void IsValidMeetingTime_WithNonNumericMinute_ThrowsFormatException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => service.IsValidMeetingTime("11:09:14:DD"));
            Assert.Contains("All parts must be numbers", exception.Message);
        }

        #endregion

        #region Month Validation

        [Fact]
        public void IsValidMeetingTime_WithMonth00_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("00:09:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithMonth13_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("13:09:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithMonth99_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("99:09:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithValidMonthInRange_ProcessesCorrectly()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(2);
            var input = $"{future.Month:D2}:{future.Day:D2}:15:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Day Validation - General

        [Fact]
        public void IsValidMeetingTime_WithDay00_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();
            var futureMonth = DateTime.UtcNow.AddDays(5).Month;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                service.IsValidMeetingTime($"{futureMonth:D2}:00:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithDay32_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("11:32:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithDay31InNovember_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("11:31:14:30"));
        }

        #endregion

        #region Day Validation - Month-Specific

        [Fact]
        public void IsValidMeetingTime_WithDay31InJanuary_ValidDay()
        {
            // Arrange
            var service = CreateChatService();
            // Will be past if current month is after January
            
            // Act & Assert - Should throw for date validation, not day validation
            var result = service.IsValidMeetingTime("01:31:15:00");
            Assert.False(result); // Past date
        }

        [Fact]
        public void IsValidMeetingTime_WithDay29InFebruaryNonLeapYear_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();
            // 2025 is not a leap year

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("02:29:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithDay30InFebruary_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("02:30:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithDay31InApril_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("04:31:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithDay31InJune_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("06:31:14:30"));
        }

        [Fact]
        public void IsValidMeetingTime_WithDay31InSeptember_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime("09:31:14:30"));
        }

        #endregion

        #region Hour Validation

        [Fact]
        public void IsValidMeetingTime_WithHour00_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:00:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithHour12_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:12:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithHour23_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:23:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithHour24_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:24:30";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime(input));
        }

        [Fact]
        public void IsValidMeetingTime_WithHour99_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:99:30";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime(input));
        }

        #endregion

        #region Minute Validation

        [Fact]
        public void IsValidMeetingTime_WithMinute00_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:14:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithMinute30_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:14:30";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithMinute59_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:14:59";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithMinute60_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:14:60";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime(input));
        }

        [Fact]
        public void IsValidMeetingTime_WithMinute99_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:14:99";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.IsValidMeetingTime(input));
        }

        #endregion

        #region Past Date Validation

        [Fact]
        public void IsValidMeetingTime_WithPastDate_ReturnsFalse()
        {
            // Arrange
            var service = CreateChatService();
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var input = $"{yesterday.Month:D2}:{yesterday.Day:D2}:14:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithCurrentTime_ReturnsFalse()
        {
            // Arrange
            var service = CreateChatService();
            var now = DateTime.UtcNow;
            var input = $"{now.Month:D2}:{now.Day:D2}:{now.Hour:D2}:{now.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.False(result); // Must be > now, not >= now
        }

        [Fact]
        public void IsValidMeetingTime_WithFutureTimeToday_ReturnsTrue()
        {
            // Arrange
            var service = CreateChatService();
            var futureToday = DateTime.UtcNow.AddHours(2);
            var input = $"{futureToday.Month:D2}:{futureToday.Day:D2}:{futureToday.Hour:D2}:{futureToday.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Seven Day Limit Validation

        [Fact]
        public void IsValidMeetingTime_WithDate1DayFromNow_ReturnsTrue()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(1);
            var input = $"{future.Month:D2}:{future.Day:D2}:{future.Hour:D2}:{future.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithDate3DaysFromNow_ReturnsTrue()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(3);
            var input = $"{future.Month:D2}:{future.Day:D2}:{future.Hour:D2}:{future.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithDate6DaysFromNow_ReturnsTrue()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(6);
            var input = $"{future.Month:D2}:{future.Day:D2}:{future.Hour:D2}:{future.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithDateExactly7DaysFromNow_ReturnsTrue()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(7);
            var input = $"{future.Month:D2}:{future.Day:D2}:{future.Hour:D2}:{future.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithDate8DaysFromNow_ReturnsFalse()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(8);
            var input = $"{future.Month:D2}:{future.Day:D2}:{future.Hour:D2}:{future.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithDate10DaysFromNow_ReturnsFalse()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(10);
            var input = $"{future.Month:D2}:{future.Day:D2}:{future.Hour:D2}:{future.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithDate7DaysPlus1Minute_ReturnsFalse()
        {
            // Arrange
            var service = CreateChatService();
            var future = DateTime.UtcNow.AddDays(7).AddMinutes(1);
            var input = $"{future.Month:D2}:{future.Day:D2}:{future.Hour:D2}:{future.Minute:D2}";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void IsValidMeetingTime_WithMidnight_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:00:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithEndOfDay_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:23:59";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidMeetingTime_WithNoon_Valid()
        {
            // Arrange
            var service = CreateChatService();
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var input = $"{tomorrow.Month:D2}:{tomorrow.Day:D2}:12:00";

            // Act
            var result = service.IsValidMeetingTime(input);

            // Assert
            Assert.True(result);
        }

        #endregion
    }
}
