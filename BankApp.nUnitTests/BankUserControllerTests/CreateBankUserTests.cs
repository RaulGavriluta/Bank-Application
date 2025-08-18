using BankApp.Controllers;
using BankApp.DTO;
using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace BankApp.nUnitTests;

public class CreateBankUserTests
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
    public async Task CreateBankUser_ValidDTO_ReturnsOk()
    {
        //Assign
        var dto = new BankUserDTO
        {
            BankUserId = 1,
            Name = "John Doe",
            Email = "johndoe@gmail.com",
            Password = "password",
            Phone = "0711111111"
        };

        _bankUserDataOpsMock.Setup(s => s.AddBankUserAsync(It.IsAny<BankUser>())).Returns(Task.CompletedTask);

        //Act
        var result = await _bankUserController.CreateBankUser(dto);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}
