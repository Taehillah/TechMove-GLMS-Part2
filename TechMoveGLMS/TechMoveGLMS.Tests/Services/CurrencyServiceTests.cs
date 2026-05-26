using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using TechMoveGLMS.Web.Services;

namespace TechMoveGLMS.Tests.Services;

public class CurrencyServiceTests
{
    [Fact]
    public void ConvertUsdToZar_UsesSuppliedRate()
    {
        // Arrange
        var service = new CurrencyService(
            new HttpClient(),
            new ConfigurationBuilder().AddInMemoryCollection().Build(),
            NullLogger<CurrencyService>.Instance);

        // Act
        var result = service.ConvertUsdToZar(100m, 18.50m);

        // Assert
        Assert.Equal(1850m, result);
    }
}
