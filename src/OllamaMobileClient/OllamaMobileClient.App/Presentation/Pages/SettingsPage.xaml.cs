using OllamaMobileClient.App.ViewModels;

namespace OllamaMobileClient.App.Presentation.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _vm;

    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync(CancellationToken.None);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await _vm.SaveAsync(CancellationToken.None);
    }
}