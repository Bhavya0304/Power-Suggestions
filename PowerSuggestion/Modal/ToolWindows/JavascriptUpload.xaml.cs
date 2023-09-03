using Microsoft.VisualStudio.PlatformUI;
using PowerSuggestion.Actions;
using PowerSuggestion.Models;
using PowerSuggestion.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.TextFormatting;
using XamlAnimatedGif;

namespace PowerSuggestion.Modal
{
    public partial class JavascriptUpload : DialogWindow
    {
        public WebResourceModel viewModel;
        private WebResourceAction WebResourceAction { get; set; }
        List<SolutionMetadata> solutionMetadata = new List<SolutionMetadata>();
        List<WebResourceMetedata> webResourceMetadata = new List<WebResourceMetedata>();
        private bool isLoader = true;
        public JavascriptUpload()
        {
            if (CRMService.Service == null)
            {
                VS.MessageBox.ShowError("Connect To CRM First!", "Your Request cannot be completed before connection to CRM GoTo Tools -> Connect CRM!");
                return;
            }
            InitializeComponent();
            ShowLoader();
            viewModel = new WebResourceModel();
            WebResourceAction = new WebResourceAction();
            DataContext = viewModel;
            AnimationBehavior.SetRepeatBehavior(BusyIcon, System.Windows.Media.Animation.RepeatBehavior.Forever);
            GetAllSolutionsAsync();
        }

        private async Task GetAllSolutionsAsync()
        {

            List<Models.SolutionMetadata> data = await Task.Run(() =>
            {
                return WebResourceAction.GetAllSolutions();
            });
            solutionMetadata = data;
            viewModel.solutions = data.OrderBy(x => x.UniqueName).ToList();
            HideLoader();


        }

        private async Task GetAllWRAsync(string Solution)
        {
            ShowLoader();
            List<WebResourceMetedata> data = await Task.Run(() =>
            {
                return WebResourceAction.getAllWebResourceFile(Solution);
            });
            data.Add(new WebResourceMetedata
            {
                Id = Guid.Empty,
                DisplayName = "-- Create New --",
                Name = viewModel.JSFileName
            });
            webResourceMetadata = data;
            viewModel.WebResource = data;

            WebResourceList.SelectedValue = Guid.Empty;
            HideLoader();


        }

        public void ShowLoader()
        {
            mainContent.Visibility = Visibility.Hidden;
            Loader.Visibility = Visibility.Visible;
            isLoader = true;
        }
        public void HideLoader()
        {
            mainContent.Visibility = Visibility.Visible;
            Loader.Visibility = Visibility.Hidden;
            isLoader = false;
        }
        public void GetResource(object sender = null, RoutedEventArgs e = null)
        {
            var Solution = (SolutionMetadata)SolutionList.SelectedItem;
            viewModel.WRPrefix = Solution.Prefix;
            GetAllWRAsync(Solution.UniqueName);
        }

        public bool ValidateForm()
        {
            string Name = WRName.Text;
            string DisplayName = WRDisplayName.Text;
            string SolutionName = (string)SolutionList.SelectedValue;

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(SolutionName))
            {
                return false;
            }

            return true;
        }
        public async void UpdateWR(object sender = null, RoutedEventArgs e = null)
        {

            try
            {
                if (!ValidateForm())
                {
                    VS.MessageBox.ShowError("Please Fill all required Fields first!");
                    return;
                }
                Guid WRId = WebResourceList.SelectedValue != null ? (Guid)WebResourceList.SelectedValue : Guid.Empty;
                this.Close();
                await VS.StatusBar.ShowProgressAsync("Uploading Web Resource", 1, 4);
                string Desc = WRDesc.Text;
                string Name = WRName.Text;
                string DisplayName = WRDisplayName.Text;
                string SolutionName = (string)SolutionList.SelectedValue;
                var solution = solutionMetadata.Where(x => x.UniqueName == SolutionName).FirstOrDefault();
                string Prefix = solution == null ? null : solution.Prefix + "_";
                string JSFilePath = viewModel.JSFilepath;
                Guid Status = await Task.Run(() =>
                {
                    return WebResourceAction.UploadWebResources(JSFilePath, WRId == Guid.Empty ? null : WRId, Desc, Name, DisplayName, SolutionName, Prefix);
                });
                await VS.StatusBar.ShowProgressAsync("Uploading Web Resource Completed", 2, 4);
                await VS.StatusBar.ShowProgressAsync("Publishing Web Resource File", 3, 4);
                bool resStatus = await Task.Run(() =>
                {
                    return WebResourceAction.PublishFile(Status);
                });
                await VS.StatusBar.ShowProgressAsync("Finishing...", 4, 4);
                await VS.StatusBar.ShowMessageAsync("Published Successfully");

            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowErrorAsync("Error in Uploading Web Resource", ex.Message);
            }



        }

        private void WebResourceList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ShowLoader();
            if (WebResourceList.SelectedValue != null)
            {
                Guid WRId = new Guid(WebResourceList.SelectedValue.ToString());
                WebResourceMetedata WR = webResourceMetadata.Where(x => x.Id == WRId).FirstOrDefault();
                viewModel.WRName = WR.Name;
                viewModel.WRDisplayName = WR.DisplayName;
                viewModel.WRDesc = WR.Description;
            }

            HideLoader();

        }
    }
}
