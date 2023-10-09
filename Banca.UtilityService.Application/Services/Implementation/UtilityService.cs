using System.Text;
using Banca.UtilityService.Domain;
using Banca.UtilityService.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Banca.UtilityService.Application;
public class UtilityService : IUtilityService
{
    private readonly IHttpClientFactory clientFactory;
    private readonly ILogger<UtilityService> logger;
    private readonly IConfiguration configuration;
    private readonly IEncryptionService encryptionService;

    public UtilityService(IHttpClientFactory clientFactory, ILogger<UtilityService> logger, IConfiguration configuration, IEncryptionService encryptionService)
    {
        this.clientFactory = clientFactory;
        this.logger = logger;
        this.configuration = configuration;
        this.encryptionService = encryptionService;
    }

    public async Task<Result<string>> GenerateOtp(GenerateOtpRequest request)
    {
        try
        {
            var url = $"{configuration["CamsUrl"]}GenerateOTP";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var payload = new
            {
                Mobile = request.Mobile,
                Email = request.Email,
                ClientID = "2"
            };

            requestMessage.Headers.Add("ApiKey", "wrqewtreyrutyterewrtretre");
            var encryptedPayload = encryptionService.EncryptAes(JsonConvert.SerializeObject(payload), configuration["AesSecretKey"], configuration["AesInitializationVector"]);
            requestMessage.Content = new StringContent(encryptedPayload, Encoding.UTF8, "application/json");
            var client = clientFactory.CreateClient("Utility");
            var response = await client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var decryptedData = encryptionService.DecryptAes(result, configuration["AesSecretKey"], configuration["AesInitializationVector"]);
                var data = JsonConvert.DeserializeObject<OtpResponse>(decryptedData);
                if (data?.ResponseCode == "1")
                {
                    return Result<string>.Success("Otp sent successfully");
                }
            }

            return Result<string>.Failure("Unable to send OTP, please try again");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result<string>.Failure("Unable to send OTP, please try again");
        }
    }
    public async Task<Result<string>> ValidateOtp(ValidateOtpRequest request)
    {
        try
        {
            var url = $"{configuration["CamsUrl"]}ValidateOTP";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);


            requestMessage.Headers.Add("ApiKey", "wrqewtreyrutyterewrtretre");
            var encryptedPayload = encryptionService.EncryptAes(JsonConvert.SerializeObject(request), configuration["AesSecretKey"], configuration["AesInitializationVector"]);
            requestMessage.Content = new StringContent(encryptedPayload, Encoding.UTF8, "application/json");
            var client = clientFactory.CreateClient("Utility");
            var response = await client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var decryptedData = encryptionService.DecryptAes(result, configuration["AesSecretKey"], configuration["AesInitializationVector"]);
                var data = JsonConvert.DeserializeObject<OtpResponse>(decryptedData);
                if (data?.ResponseCode == "1")
                {
                    return Result<string>.Success("Otp is Valid");
                }
            }

            return Result<string>.Failure("Unable to validate OTP, please try again");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result<string>.Failure("Unable to validate OTP, please try again");
        }
    }
}
