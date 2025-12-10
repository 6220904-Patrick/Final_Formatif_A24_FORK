using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    Mock<SeatsService> mockService;
    Mock<SeatsController> mockController;

    public SeatsControllerTests()
    {
        mockService = new Mock<SeatsService>();
        mockController = new Mock<SeatsController>(mockService.Object) { CallBase = true };

        mockController.Setup(c => c.UserId).Returns("user1");
    }

    [TestMethod]
    public void ReserveSeatWorks()
    {

        Seat seat = new Seat() { Number = 10 };
        mockService.Setup(s => s.ReserveSeat("user1", 10)).Returns(seat);
        mockController.Setup(c => c.UserId).Returns("user1");

        var reserve = mockController.Object.ReserveSeat(10);
        var result = reserve.Result as OkObjectResult;

        Assert.IsNotNull(result);

    }

    [TestMethod]
    public void ReserveSeatTaken()
    {

        mockService.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatAlreadyTakenException());

        var reserve = mockController.Object.ReserveSeat(10);
        var result = reserve.Result as UnauthorizedResult;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeatOutOfBounds()
    {
        mockService.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatOutOfBoundsException());

        var reserve = mockController.Object.ReserveSeat(150);
        var result = reserve.Result as NotFoundObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual("Could not find 150", result.Value);
    }

    [TestMethod]
    public void ReserveSeatUserAlreadySeated()
    {
        mockService.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new UserAlreadySeatedException());

        var reserve = mockController.Object.ReserveSeat(20);
        var result = reserve.Result as BadRequestResult;

        Assert.IsNotNull(result);
    }
}