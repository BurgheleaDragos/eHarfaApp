using System.Collections;
using eHarfaApp.Shared.DAL;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eHarfaApp.Shared.Pages;

public partial class Home: ComponentBase
{
    private string factor => FormFactor.GetFormFactor();
    private string platform => FormFactor.GetPlatform();
    private string Search { get; set; }
    public IEnumerable<SongCategory> Categories { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Categories = new List<SongCategory>();
        Categories = Categories.Append(new SongCategory()
        {
            Id = "id1",
            Icon = Icons.Material.Outlined.Start,
            Title = "test Dragos category",
            Content = "Dragos category"
        }).Append( 
            new SongCategory()
        {
            Id = "id2",
            Icon = Icons.Material.Outlined.CallEnd,
            Title = "test Dragos category2",
            Content = "Dragos category22"
        });
        await base.OnInitializedAsync();
    }
}