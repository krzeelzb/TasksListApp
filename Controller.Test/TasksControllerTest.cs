using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TasksListApp.Controllers;
using TasksListApp.Models;
using TasksListApp.Services;

namespace TasksListApp.Tests.Controllers
{
    [TestFixture]
    public class TasksControllerTest
    {

        private Mock<ILogger<TasksController>> loggerMock;
        private Mock<ITaskService> taskServiceMock;
        private TasksController tasksController;

        [SetUp]
        public void SetUp()
        {
            loggerMock = new Mock<ILogger<TasksController>>(MockBehavior.Loose);
            taskServiceMock = new Mock<ITaskService>(MockBehavior.Strict);
            tasksController = new TasksController(taskServiceMock.Object, loggerMock.Object);
        }

        [Test]
        public void Get_ShouldReturnListOfTasks()
        {
            // arrange
            var expectedTasksList = new List<TaskItem>
            {
                { new TaskItem(){ Id = "id1", Title = "Test Title", IsCompleted = true} },
                { new TaskItem(){ Id = "id2", Title = "Test Title", IsCompleted = true} }
            };

            taskServiceMock
                .Setup(i => i.GetTasks())
                .Returns(expectedTasksList);

            // act
            var response = tasksController.Get();
            
            // assert
            Assert.IsNotNull(response);
            Assert.That(response.Value, Is.EqualTo(expectedTasksList));
        }
        
        [Test]
        public void Get_ShouldReturnNotFound_IfListEmpty()
        {
            // arrange
            var expectedTasksList = new List<TaskItem> { };

            taskServiceMock
                .Setup(i => i.GetTasks())
                .Returns(expectedTasksList);

            // act
            var response = tasksController.Get();
            
            // assert
            Assert.IsNull(response.Value);
            Assert.That(response.Result.GetType(), Is.EqualTo(typeof(NotFoundObjectResult)));
        }
    }
}
