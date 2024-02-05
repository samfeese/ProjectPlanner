using Moq;
using ProjectPlanner.Interfaces;
using ProjectPlanner.Models;
using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Test
{
    public class ValidateTest
    {
        [Fact]
        public void Validate_ReturnsFalse_WhenSprintNameIsEmpty()
        {
            // Arrange
            var dbMock = new Mock<IDatabaseHelper>();
            var alertMock = new Mock<IAlertService>();
            var viewModel = new SprintFormViewModel(dbMock.Object, alertMock.Object);
            viewModel.SprintName = ""; // Invalid state

            // Act
            var isValid = viewModel.Validate(); // This is now accessible

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Validate_ReturnsFalse_WhenEndDateIsBeforeStartDate()
        {
            // Arrange
            var dbMock = new Mock<IDatabaseHelper>();
            var alertMock = new Mock<IAlertService>();
            var viewModel = new SprintFormViewModel(dbMock.Object, alertMock.Object);
            viewModel.StartDate = DateTime.Now.AddDays(1);
            viewModel.EndDate = DateTime.Now; // Invalid state

            // Act
            var isValid = viewModel.Validate(); // Directly testing Validate

            // Assert
            Assert.False(isValid);
        }

    }
}
