using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using UnitTestsTarget;

namespace UnitTests
{
    class VuksCalendarServiceTests
    {
        [Test]
        public void GetWorkingYesterday_RealLogic_Monday_ReturnsFriday()
        {
            // Arrange
            var today = new DateTime(2020, 10, 05);

            var service = new CalendarService(new DayShiftService(new DayOfWeekService()));

            // Act
            var result = service.GetWorkingYesterday(today);

            // Assert
            result.Should().Be(new DateTime(2020, 10, 02));
        }

        [Test]
        public void GetWorkingYesterday_AllDatesWorking_ReturnsYesterday()
        {
            // Arrange
            var today = new DateTime(2020, 10, 05);

            var dayOfWeekService = new Mock<IDayOfWeekService>();
            dayOfWeekService
                .Setup(x => x.IsWeekend(It.IsAny<DateTime>()))
                .Returns(false);

            var service = new CalendarService(new DayShiftService(dayOfWeekService.Object));

            // Act
            var result = service.GetWorkingYesterday(today);

            // Assert
            result.Should().Be(new DateTime(2020, 10, 04));
        }

        [Test]
        public void IsWeekend_RealLogic_Friday_ReturnsFalse()
        {
            var today = new DateTime(2020,10,02);

            var service = new DayOfWeekService();

            var result = service.IsWeekend(today);

            result.Should().Be(false);
        }

        [Test]
        public void IsWeekend_RealLogic_Sunday_ReturnsTrue()
        {
            //Arrange
            var today = new DateTime(2020, 10, 04);

            var service = new DayOfWeekService();

            var result = service.IsWeekend(today);

            result.Should().Be(true);
        }

        [Test]
        public void GetShiftBusinessDay_RealLogic_Thursday_ReturnsFriday()
        {
            var thursday = new DateTime(2020, 10, 01);
            var friday = new DateTime(2020, 10, 02);

            var service = new DayShiftService(new DayOfWeekService());
            
            var result = service.GetShiftBusinessDay(thursday, 1);

            result.Should().Be(friday);
        }

        [Test]
        public void GetShiftBusinessDay_RealLogic_Monday_ReturnsFriday()
        {
            var monday = new DateTime(2020, 10, 05);
            var friday = new DateTime(2020, 10, 02);

            var service = new DayShiftService(new DayOfWeekService());

            var result = service.GetShiftBusinessDay(monday, -1);

            result.Should().Be(friday);
        }

    }
}
