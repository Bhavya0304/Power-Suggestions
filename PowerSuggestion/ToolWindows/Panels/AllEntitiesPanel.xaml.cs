using Microsoft.Xrm.Sdk.Metadata;
using PowerSuggestion.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AllEntitiesPanel.xaml
    /// </summary>
    public partial class AllEntitiesPanel : UserControl
    {
        private CRMSuggestionActions suggestionActions;
        private List<Models.EntityMetadata> entitiesData;
        private Delegate _CallNext;
        public Delegate CallNext
        {
            set { _CallNext = value; }
        }

        public AllEntitiesPanel()
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
            List<Models.EntityMetadata> newList = new List<Models.EntityMetadata>();
            newList.AddRange(entitiesData.Where(x => x.DisplayName.Contains(SearchBox.Text)).ToList());
            newList.AddRange(entitiesData.Where(x => x.LogicalName.Contains(SearchBox.Text)).ToList());

            CreateEntityList(newList.Distinct().ToList());
        }

        public void OnReset(object sender = null, RoutedEventArgs e = null)
        {
            GetAllEntitiesAsync();
        }

        public void ShowAttributes(object sender = null, RoutedEventArgs e = null)
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

            object[] args = new object[1];
            args[0] = LogicalName;
            _CallNext.DynamicInvoke(args);
        }

        private async Task GetAllEntitiesAsync()
        {

            List<Models.EntityMetadata> data = await Task.Run(() =>
            {
                return suggestionActions.GetAllEntities();
            });
            entitiesData = data;
            CreateEntityList(entitiesData);

        }

        private void CreateEntityList(List<Models.EntityMetadata> entities)
        {

            EntitiesListView.ItemsSource = entities;

        }



    }
}
