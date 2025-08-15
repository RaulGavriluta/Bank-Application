using BankApp.Controllers;
using BankApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq;
using BankApp.DTO;
using BankApp.Models;

namespace BankApp.nUnitTests;

public class GetAllBankUsersTests
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
    public async Task GetAllBankUsers_ReturnsOk()
    {
        //Arrange
        var users = new BankUserDTO[]
        {
            new BankUserDTO { 
                BankUserId = 1, 
                Name ="John Doe", 
                Email = "johndoe@example.com", 
                Password = "parola123", 
                Phone = "0722222222" }
        };
        _bankUserDataOpsMock.Setup(s => s.GetBankUsersAsync()).ReturnsAsync(users);
        //Act
        var result = await _bankUserController.GetAllBankUsers();
        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}
