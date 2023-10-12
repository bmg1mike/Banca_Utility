using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banca.UtilityService.Domain;

public class SterlingBranchDto
{
    public string? BranchCode { get; set; }
    public string? BranchName { get; set; }
    public string? BranchAlias { get; set; }
    public string? SubDivCode { get; set; }
    public string? StateId { get; set; }
    public string? StateName { get; set; }
    public string? RegionId { get; set; }
    public string? RegionName { get; set; }
}

public class SterlingBranchList
{
    public string? ResponseCode { get; set; }
    public string? ResponseDescription { get; set; }
    public List<SterlingBranchDto>? SterlingBranches { get; set; }
}