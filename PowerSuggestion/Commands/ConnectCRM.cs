using EnvDTE;
using PowerSuggestion.Models;

namespace PowerSuggestion
{
    [Command(PackageIds.ConnectCRM)]
    internal sealed class ConnectCRM : BaseCommand<ConnectCRM>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            //Getting Connection String
            ConnectionParameters options = await ConnectionParameters.GetLiveInstanceAsync();
            if (options != null && !string.IsNullOrEmpty(options.Username) && !string.IsNullOrEmpty(options.Password) && !string.IsNullOrEmpty(options.EnviromentUrl))
            {
                CRMService ServiceObj = new CRMService();
                ServiceObj.ConnectToCRM(options.Username,options.Password,options.EnviromentUrl);
            }
            else
            {
                await VS.MessageBox.ShowErrorAsync("CRM Service", "Please Setup Username/Password before connecting to CRM Service In Tools -> Options -> PowerSuggestion -> Connection Parameters");
            }
        }
    }
}
