using PowerSuggestion.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowerSuggestion.ToolWindows.Panels
{
    /// <summary>
    /// Interaction logic for AllAttributesPanel.xaml
    /// </summary>
    public partial class AllAttributesPanel : UserControl
    {
        private CRMSuggestionActions suggestionActions;
        private List<Models.AttributeMetadata> entitiesData;
        public string CurrentEntityName = "";
        private Delegate _CallNext;
        private Delegate _showLoader;
        private Delegate _hideLoader;
        public Delegate ShowLoader
        {
            set { _showLoader = value; }
        }

        public Delegate HideLoader
        {
            set { _hideLoader = value; }
        }
        public Delegate CallNext
        {
            set { _CallNext = value; }
        }
        public AllAttributesPanel()
        {
            InitializeComponent();
            suggestionActions = new CRMSuggestionActions();
        }
        public void CopyName(object sender = null, RoutedEventArgs e = null)
        {
            string LogicalName = (e.Source as Button).Tag.ToString();
            Clipboard.SetText(LogicalName);
        }

        public async void InsertName(object sender = null, RoutedEventArgs e = null)
        {
            string LogicalName = (e.Source as Button).Tag.ToString();
            var docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView == null)
            {
                return;
            }
            var selectionText = docView?.TextView.Selection.SelectedSpans.FirstOrDefault();
            if (selectionText.HasValue)
            {
                docView?.TextBuffer.Replace(selectionText.Value, LogicalName);
            }
        }
        public void Search(object sender = null, RoutedEventArgs e = null)
        {
            List<Models.AttributeMetadata> newList = new List<Models.AttributeMetadata>();
            newList.AddRange(entitiesData.Where(x => x.DisplayName.ToLower().Contains(SearchBox.Text.ToLower())).ToList());
            newList.AddRange(entitiesData.Where(x => x.LogicalName.ToLower().Contains(SearchBox.Text.ToLower())).ToList());

            CreateAttributeList(newList.Distinct().ToList());
        }
        public void OnReset(object sender = null, RoutedEventArgs e = null)
        {
            GetAllAttributesAsync(CurrentEntityName);
        }



        public void GoNext(object sender = null, RoutedEventArgs e = null)
        {

            string LogicalName = "";
            if (e.Source as Button != null)
            {
                LogicalName = (e.Source as Button).Tag.ToString();
            }
            else if (e.Source as ListViewItem != null)
            {
                LogicalName = (e.Source as ListViewItem).Tag.ToString();
            }
            object[] args = new object[2];
            args[0] = LogicalName;
            args[1] = CurrentEntityName;
            _CallNext.DynamicInvoke(args);
        }

        private async Task GetAllAttributesAsync(string EntityName)
        {
            try
            {
                _showLoader.DynamicInvoke();
                List<Models.AttributeMetadata> data = await Task.Run(() =>
                {
                    return suggestionActions.GetEntityWithAttributes(EntityName);
                });
                entitiesData = data;
                CreateAttributeList(entitiesData);
                _hideLoader.DynamicInvoke();
            }
            catch (Exception ex)
            {

                VS.MessageBox.ShowErrorAsync("Something Went Wrong!", ex.Message + "\nIf the problem persist try reconnection CRM!");
                _hideLoader.DynamicInvoke();
            }


        }

        private void CreateAttributeList(List<Models.AttributeMetadata> entities)
        {

            AttributeListView.ItemsSource = entities;

        }
    }
}
