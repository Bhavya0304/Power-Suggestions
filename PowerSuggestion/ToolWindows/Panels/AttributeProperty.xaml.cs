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


        private async Task GetAttributesAsync(string EntityName, string AttributeName)
        {
            try
            {
                _showLoader.DynamicInvoke();
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
                _hideLoader.DynamicInvoke();
            }
            catch (Exception ex)
            {

                VS.MessageBox.ShowErrorAsync("Something Went Wrong!", ex.Message + "\nIf the problem persist try reconnection CRM!");
                _hideLoader.DynamicInvoke();
            }
        }


    }
}
