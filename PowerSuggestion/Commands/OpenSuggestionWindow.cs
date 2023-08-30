namespace PowerSuggestion.Commands
{
    [Command(PackageIds.OpenSuggestionWindow)]
    internal sealed class OpenSuggestionWindow : BaseCommand<OpenSuggestionWindow>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {

            await SuggestionWindow.ShowAsync();
        }
    }
}
