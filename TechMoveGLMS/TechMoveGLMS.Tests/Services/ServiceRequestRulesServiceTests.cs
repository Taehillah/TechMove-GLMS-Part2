using TechMoveGLMS.Web.Models.Entities;
using TechMoveGLMS.Web.Models.Enums;
using TechMoveGLMS.Web.Services;

namespace TechMoveGLMS.Tests.Services;

public class ServiceRequestRulesServiceTests
{
    [Fact]
    public void ValidateContractAllowsServiceRequest_ActiveContractAllowsRequest()
    {
        // Arrange
        var service = new ServiceRequestRulesService();
        var contract = new Contract { Status = ContractStatus.Active };

        // Act
        var exception = Record.Exception(() => service.ValidateContractAllowsServiceRequest(contract));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ValidateContractAllowsServiceRequest_ExpiredContractBlocksRequest()
    {
        // Arrange
        var service = new ServiceRequestRulesService();
        var contract = new Contract { Status = ContractStatus.Expired };

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => service.ValidateContractAllowsServiceRequest(contract));

        // Assert
        Assert.Equal("Service requests cannot be created for expired contracts.", exception.Message);
    }

    [Fact]
    public void ValidateContractAllowsServiceRequest_OnHoldContractBlocksRequest()
    {
        // Arrange
        var service = new ServiceRequestRulesService();
        var contract = new Contract { Status = ContractStatus.OnHold };

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => service.ValidateContractAllowsServiceRequest(contract));

        // Assert
        Assert.Equal("Service requests cannot be created while a contract is on hold.", exception.Message);
    }
}
