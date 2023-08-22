global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Threading;

namespace PowerSuggestion
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(OptionsProvider.ConnectionParametersOptions), "PowerSuggestion", "ConnectionParameters", 0, 0, true, SupportsProfiles = true)]
    [Guid(PackageGuids.PowerSuggestionString)]
    [ProvideToolWindow(typeof(SuggestionWindow.Pane), Style = VsDockStyle.Tabbed, MultiInstances = false, Window = ToolWindowGuids.SolutionExplorer)]
    public sealed class PowerSuggestionPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();
            this.RegisterToolWindows();
        }
    }
}