using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Account.DomainApi.Domain.Account;
using Services.Account.DomainApi.Domain.Account.Processor;
using Services.Account.Surface;
using Xunit;

namespace Services.Account.Tests.Controller
{
    public class AccountControllerTest
    {

        readonly Mock<IAccountProcessor> _mockAccountProcessor = null;
        readonly Mock<ILogger<AccountController>> _logger = null;
        AccountController _accountController = null;

        public AccountControllerTest()
        {
            _mockAccountProcessor = new Mock<IAccountProcessor>();
            _logger = new Mock<ILogger<AccountController>>();
            _accountController = new AccountController(_logger.Object, _mockAccountProcessor.Object);
        }

        [Fact]
        public void GetAllMeterReading_When_Returns_Successfull_Response()
        {

            List<MeterDetails> meterDetails = new List<MeterDetails>(){ new MeterDetails() { AccountId = 1, MeterReadingDate = "1234", MeterReadValue = "5467" }};
            _mockAccountProcessor.Setup(x => x.GetAllMeterReading()).Returns(meterDetails);

            var result =  _accountController.GetAllMeterReading();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<List<MeterDetails>>(okResult.Value);
        }

       [Fact]
       public void GetAllMeterReading_When_Returns_Unsuccessfull_Response()
       {
            _mockAccountProcessor.Setup(x => x.GetAllMeterReading()).Throws<Exception>();

            //Assert
            Assert.ThrowsAsync<Exception>(() => _accountController.GetAllMeterReading());
        }
    }
}
