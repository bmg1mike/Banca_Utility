using MongoDB.Entities;

namespace Banca.UtilityService.Domain;
public class OutboundLog : Entity
{
    public string? OutboundLogId { get; set; }
    public string? ServiceProvider { get; set; }
    public string? EndPointCalled { get; set; }
    public DateTime LogDate { get; set; }
    public string? RequestDetails { get; set; }
    public DateTime RequestDateTime { get; set; }
    public string? ResponseDetails { get; set; }
    public DateTime ResponseDateTime { get; set; }
    public string? ErrorMessage { get; set; }
    public bool HasError => ErrorMessage != "";
    public bool IsSuccess { get; set; } = true;
}
