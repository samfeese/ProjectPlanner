using Moq;
using ProjectPlanner.Interfaces;
using ProjectPlanner.Models;
using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Test
{
    public class MainViewModelTest
    {
        [Fact]
        public async void MainPageViewModelLoad()
        {
            // Arrange
            var dbMock = new Mock<IDatabaseHelper>();
            dbMock.Setup(db => db.GetAllAsync<Project>()).ReturnsAsync(new List<Project>
            {
                new Project { Id = 1, Name = "Super Cool Project" },
                new Project { Id = 2, Name = "Not so Cool Project" },
            });
            // Act
            var vm = new MainPageViewModel(dbMock.Object);
            await vm.Load();
            // Assert
            Assert.Equal(2, vm.GetProjects.Count);
        }
    }
}