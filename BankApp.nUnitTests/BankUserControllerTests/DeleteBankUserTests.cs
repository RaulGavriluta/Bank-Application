using Moq;
using BankApp.Interfaces;
using BankApp.Controllers;
using Microsoft.AspNetCore.Mvc;
namespace BankApp.nUnitTests.BankUserControllerTests
{
    public class DeleteBankUserTests
    {
        private Mock<IBankUserDataOps> _bankUserDataOpsMock;
        private BankUserController _bankUsersController;

        [SetUp]
        public void Setup()
        {
            _bankUserDataOpsMock = new Mock<IBankUserDataOps>();
            _bankUsersController = new BankUserController(_bankUserDataOpsMock.Object);
        }

        [Test]
        public async Task DeleteBankUserAsync_UserExists_ReturnsNoContent()
        {
            //Arrange
            _bankUserDataOpsMock.Setup(s => s.DeleteBankUserAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            //Act
            var result = await _bankUsersController.DeleteBankUser(1);
            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);   
        }

        [Test]
        public async Task DeleteBankUserAsync_NotExistingUser_ReturnsNotFound()
        {
            //Arrange
            _bankUserDataOpsMock.Setup(s => s.DeleteBankUserAsync(It.IsAny<int>())).ThrowsAsync(new ArgumentException("User with id 1 does not exist."));

            //Act
            var result = await _bankUsersController.DeleteBankUser(1);
            var message = result as NotFoundObjectResult;
            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual("User with id 1 does not exist.", message.Value);

        }
    }
}