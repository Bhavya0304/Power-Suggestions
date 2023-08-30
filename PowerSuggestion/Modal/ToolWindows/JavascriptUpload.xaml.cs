using Microsoft.VisualStudio.PlatformUI;
using PowerSuggestion.ViewModels;

namespace PowerSuggestion.Modal
{
    public partial class JavascriptUpload : DialogWindow
    {
        public WebResourceModel viewModel;
        public JavascriptUpload()
        {
            InitializeComponent();
            viewModel = new WebResourceModel();
            DataContext = viewModel;
        }

    }
}
