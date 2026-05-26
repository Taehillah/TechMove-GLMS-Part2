namespace TechMoveGLMS.Web.Services;

public interface ICurrencyService
{
    Task<decimal> GetUsdToZarRateAsync();

    decimal ConvertUsdToZar(decimal usd, decimal rate);
}
