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
        public AllAttributesPanel()
        {
            InitializeComponent();
            suggestionActions = new CRMSuggestionActions();
        }

        public void Search(object sender = null, RoutedEventArgs e = null)
        {
            List<Models.AttributeMetadata> newList = new List<Models.AttributeMetadata>();
            newList.AddRange(entitiesData.Where(x => x.DisplayName.Contains(SearchBox.Text)).ToList());
            newList.AddRange(entitiesData.Where(x => x.LogicalName.Contains(SearchBox.Text)).ToList());

            CreateAttributeList(newList.Distinct().ToList());
        }
        public void OnReset(object sender = null, RoutedEventArgs e = null)
        {
            GetAllAttributesAsync(CurrentEntityName);
        }

        public void GoNext(object sender = null, RoutedEventArgs e = null)
        {

        }

        private async Task GetAllAttributesAsync(string EntityName)
        {

            List<Models.AttributeMetadata> data = await Task.Run(() =>
            {
                return suggestionActions.GetEntityWithAttributes(EntityName);
            });
            entitiesData = data;
            CreateAttributeList(entitiesData);

        }

        private void CreateAttributeList(List<Models.AttributeMetadata> entities)
        {

            AttributeListView.ItemsSource = entities;

        }
    }
}
