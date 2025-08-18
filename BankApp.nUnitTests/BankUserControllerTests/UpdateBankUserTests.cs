using BankApp.Models;
using BankApp.Interfaces;
using BankApp.Controllers;
using Moq;
using BankApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.nUnitTests;

public class UpdateBankUserTests
{
    private Mock<IBankUserDataOps> _bankUserDataOpsMock;
    private BankUserController _bankUserController;

    [SetUp]
    public void Setup()
    {
        _bankUserDataOpsMock = new Mock<IBankUserDataOps>();
        _bankUserController = new BankUserController( _bankUserDataOpsMock.Object );
    }

    [Test]
    public async Task UpdateBankUser_IdMismatch_ReturnBadRequest()
    {
        var dto = new BankUserUpdateDTO
        {
            BankUserId = 2,
            Name = "Mike",
            Email = "mike@gmail.com",
            Phone = "0722222222"
        };
        //Assign
        _bankUserDataOpsMock.Setup(s => s.UpdateBankUserAsync(It.IsAny<BankUser>())).ThrowsAsync(new ArgumentException("User ID mismatch"));

        //Act
        var result = await _bankUserController.UpdateBankUser(1, dto);
        var badRequest = result as BadRequestObjectResult;
        //Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        Assert.AreEqual("User ID mismatch.", badRequest.Value);
    }

    [Test]
    public async Task UpdateBankUser_InexistentUser_ReturnsNotFound()
    {
        //Assign
        var dto = new BankUserUpdateDTO
        {
            BankUserId = 2,
            Name = "Mike",
            Email = "mike@gmail.com",
            Phone = "0722222222"
        };

        _bankUserDataOpsMock.Setup(s => s.GetBankUserByIdAsync(It.IsAny<int>())).ReturnsAsync((BankUser)null);

        //Act
        var result = await _bankUserController.UpdateBankUser(2, dto);
        //Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task UpdateBankUser_UpdateSuccess_ReturnsNoContent()
    {
        //Assign
        var dto = new BankUserUpdateDTO
        {
            BankUserId = 2,
            Name = "Mike",
            Email = "mike@gmail.com",
            Phone = "0722222222"
        };

        var existingUser = new BankUser
        {
            BankUserId = 2,
            Name = "John",
            Email = "john@gmail.com",
            Phone = "0711111111"
        };

        _bankUserDataOpsMock.Setup(s => s.GetBankUserByIdAsync(2)).ReturnsAsync(existingUser);
        _bankUserDataOpsMock.Setup(s => s.UpdateBankUserAsync(It.IsAny<BankUser>())).Returns(Task.CompletedTask);
        //Act
        var result = await _bankUserController.UpdateBankUser(2, dto);

        //Assert
        Assert.IsInstanceOf<NoContentResult>(result);
        Assert.AreEqual("Mike", existingUser.Name);
        Assert.AreEqual("mike@gmail.com", existingUser.Email);
    }
}
