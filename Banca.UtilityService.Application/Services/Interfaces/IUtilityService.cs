using Banca.UtilityService.Domain;

namespace Banca.UtilityService.Application;
public interface IUtilityService
{
    Task<Result<string>> GenerateOtp(GenerateOtpRequest request);
    Task<Result<string>> ValidateOtp(ValidateOtpRequest request);
}
