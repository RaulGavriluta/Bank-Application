using BankApp.Interfaces;
using BankApp.Models;
using Moq;
using BankApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using BankApp.DTO;

namespace BankApp.nUnitTests;

public class GetBankUsersByIdTests
{
    private Mock<IBankUserDataOps> _bankUserDataOpsMock;
    private BankUserController _bankUserController;

    [SetUp]
    public void Setup()
    {
        _bankUserDataOpsMock = new Mock<IBankUserDataOps>();
        _bankUserController = new BankUserController(_bankUserDataOpsMock.Object);
    }

    [Test]
    public async Task GetBankUsersById_InexistentUser_ReturnsNotFound()
    {
        //Arrange
        _bankUserDataOpsMock.Setup(s => s.GetBankUserByIdAsync(It.IsAny<int>())).ReturnsAsync((BankUser)null);

        //Act
        var result = await _bankUserController.GetBankUserById(1);

        //Assign
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task GetBankUsersById_AccountFound_ReturnsOk()
    {
        //Arrange
        var user = new BankUser
        {
            BankUserId = 1,
            Name = "John Doe",
            Email = "johndoe@gmail.com",
            Phone = "0741111111",
            Password = "parola"
        };

        _bankUserDataOpsMock.Setup(s => s.GetBankUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

        //Act
        var result = await _bankUserController.GetBankUserById(1);

        //Assign
        var okresult = result as OkObjectResult;
        Assert.IsNotNull(okresult);
        var dto = okresult.Value as BankUserDTO;
        Assert.AreEqual(1, dto.BankUserId);
        Assert.AreEqual("John Doe", dto.Name);
        Assert.AreEqual("johndoe@gmail.com", dto.Email);
    }
}
