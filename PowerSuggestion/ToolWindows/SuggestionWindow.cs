using Microsoft.VisualStudio.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PowerSuggestion
{
    public class SuggestionWindow : BaseToolWindow<SuggestionWindow>
    {
        public override string GetTitle(int toolWindowId) => "CRM Suggestion Box";

        public override Type PaneType => typeof(Pane);

        public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            return Task.FromResult<FrameworkElement>(new SuggestionWindowControl());
        }

        [Guid("73066d7b-0fca-480d-b699-e32ba215dc74")]
        internal class Pane : ToolWindowPane
        {
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.DynamicsCRM;
            }
        }
    }
}
