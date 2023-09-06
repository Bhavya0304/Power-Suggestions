using PowerSuggestion.Actions;
using PowerSuggestion.Helpers;
using PowerSuggestion.ViewModels;
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
    /// Interaction logic for EntityProperty.xaml
    /// </summary>
    public partial class EntityProperty : UserControl
    {
        private CRMSuggestionActions suggestionActions;
        public Models.EntityAttributes EntityData;
        public EntityMetadataModel EntityMetadata;
        public string CurrentEntityName = "";
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


        public EntityProperty()
        {
            InitializeComponent();
            suggestionActions = new CRMSuggestionActions();
            EntityMetadata = new EntityMetadataModel();
            DataContext = EntityMetadata;
        }


        public void OnReset(object sender = null, RoutedEventArgs e = null)
        {
            GetEntityAsync(CurrentEntityName);

        }

        private async Task GetEntityAsync(string EntityName)
        {
            try
            {
                _showLoader.DynamicInvoke();
                Models.EntityMetadata data = await Task.Run(() =>
                {
                    return suggestionActions.GetEntityMetadata(EntityName);
                });
                Convertors.ConvertEntityModelToViewModel(data, EntityMetadata);
                _hideLoader.DynamicInvoke();
            }
            catch (Exception ex)
            {

                VS.MessageBox.ShowErrorAsync("Something Went Wrong!", ex.Message + "\nIf the problem persist try reconnection CRM!");
            }
        }
    }
}
