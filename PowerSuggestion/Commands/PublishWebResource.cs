using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows;

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
                if (js.viewModel != null)
                {
                    if(new FileInfo(filePath).Extension.ToLower() == ".js")
                    {
                        js.viewModel.JSFile = filePath;
                        js.viewModel.JSFilepath = filePath;
                        js.viewModel.JSFileName = projectItem.Name;

                        js.Show();
                    }
                    else
                    {
                        VS.MessageBox.ShowError("Only JS File Supprted!", "Only file are supported for now!");
                    }
                    
                }

            }
            else
            {
                VS.MessageBox.ShowErrorAsync("Not Supported Action", "Please Select One and Only One Item");
            }


        }
    }
}
