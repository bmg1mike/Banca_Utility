using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banca.UtilityService.Domain.DTOs
{
    public class OtpResponse
    {
        public string? ResponseCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}