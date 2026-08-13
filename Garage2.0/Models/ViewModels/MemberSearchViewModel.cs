namespace Garage2._0.Models.ViewModels;

public class MemberSearchViewModel
{
    public string? Search { get; set; }
    public IEnumerable<MemberListItemViewModel> Members { get; set; } = [];
}

