using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace PowerSuggestion.MenuItem
{
    [Command(PackageIds.PublishWRFile)]
    internal sealed class PublishWebResource : BaseCommand<PublishWebResource>
    {

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            Modal.JavascriptUpload js = new Modal.JavascriptUpload();

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = (DTE)await ServiceProvider.GetGlobalServiceAsync(typeof(DTE));
            var selectedItems = dte.SelectedItems;
            var filePath = "";
            if (selectedItems.Count == 1)
            {

                var selectedItem = selectedItems.Item(1);

                var projectItem = selectedItem.ProjectItem;

                filePath = projectItem.Properties.Item("FullPath").Value.ToString();

            }
            js.viewModel.JSFile = filePath;
            js.ShowDialog();
        }
    }
}
