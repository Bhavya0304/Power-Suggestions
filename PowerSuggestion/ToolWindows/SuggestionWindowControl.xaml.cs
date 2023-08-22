using Microsoft.VisualStudio.TextManager.Interop;
using PowerSuggestion.Actions;
using PowerSuggestion.Models;
using PowerSuggestion.ToolWindows.Panels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using static PowerSuggestion.Helpers.Enums;

namespace PowerSuggestion
{
    public partial class SuggestionWindowControl : UserControl
    {
        public bool isPanelActive = false;
        private CRMSuggestionActions suggestionActions;
        private Guid PropertyId = Guid.Empty;
        private CurrentPage PageControl = CurrentPage.AllEntities;
        private double CurrentWindowHeight = 0;
        private double CurrentWindowWidth = 0;

        public SuggestionWindowControl()
        {
            suggestionActions = new CRMSuggestionActions();
            InitializeComponent();
            ShowHideOnConnection();
            this.SizeChanged += OnWindowSizeChanged;
            EntityPanel.CallNext = CallNext;

        }

        private void CallNext(string LogicalName)
        {
            AttributePanel.CurrentEntityName = LogicalName;
            PageControl = CurrentPage.AllAttributes;
            ShowAction();
        }

        private void OnReset(object sender, RoutedEventArgs e)
        {
            if (ShowHideOnConnection())
            {
                ShowAction();
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (PageControl == CurrentPage.AllAttributes)
            {
                PageControl = CurrentPage.AllEntities;
                ShowAction();
            }
        }
        private void ShowAction()
        {
            if (PageControl == CurrentPage.AllEntities)
            {
                EntityPanel.Visibility = Visibility.Visible;
                AttributePanel.Visibility = Visibility.Hidden;
                EntityPanel.OnReset();
            }
            else if (PageControl == CurrentPage.AllAttributes)
            {
                EntityPanel.Visibility = Visibility.Hidden;
                AttributePanel.Visibility = Visibility.Visible;
                AttributePanel.OnReset();
            }
        }
        private bool ShowHideOnConnection()
        {
            if (CRMService.Service == null)
            {
                SuggestionPanel.Visibility = Visibility.Hidden;
                ErrorPanel.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                ErrorPanel.Visibility = Visibility.Hidden;
                SuggestionPanel.Visibility = Visibility.Visible;
                isPanelActive = true;
                return true;
            }
        }

        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CurrentWindowHeight = e.NewSize.Height;
            CurrentWindowWidth = e.NewSize.Width;

        }

    }
}
