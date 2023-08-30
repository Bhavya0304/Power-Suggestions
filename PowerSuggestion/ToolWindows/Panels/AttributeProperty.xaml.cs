using PowerSuggestion.Actions;
using PowerSuggestion.Helpers;
using PowerSuggestion.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AttributeProperty.xaml
    /// </summary>
    public partial class AttributeProperty : UserControl
    {
        private CRMSuggestionActions suggestionActions;
        public Models.AttributeMetadata AttributeData;
        public AttributeMetadataModel AttributeMetadata;
        public string CurrentAttributeName = "";
        public string CurrentEntityName = "";


        public AttributeProperty()
        {
            InitializeComponent();
            suggestionActions = new CRMSuggestionActions();
            AttributeMetadata = new AttributeMetadataModel();
            DataContext = AttributeMetadata;
        }


        public void OnReset(object sender = null, RoutedEventArgs e = null)
        {
            GetAttributesAsync(CurrentEntityName, CurrentAttributeName);

        }



        private async Task GetAttributesAsync(string EntityName, string AttributeName)
        {

            Models.AttributeMetadata data = await Task.Run(() =>
            {
                return suggestionActions.GetAttributeMetadata(EntityName, AttributeName);
            });
            Convertors.ConvertAttributeModelToViewModel(data, AttributeMetadata);
            if (AttributeMetadata.OptionSet == null)
            {
                Options.Visibility = Visibility.Hidden;
            }
            else
            {
                Options.Visibility = Visibility.Visible;
            }

        }


    }
}
