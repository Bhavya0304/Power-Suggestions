using Microsoft.VisualStudio.TextManager.Interop;
using PowerSuggestion.Actions;
using PowerSuggestion.Models;
using PowerSuggestion.ToolWindows.Panels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using static PowerSuggestion.Helpers.Enums;
using XamlAnimatedGif;


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
            AnimationBehavior.SetRepeatBehavior(BusyIcon, System.Windows.Media.Animation.RepeatBehavior.Forever);
            OnReset();
            this.SizeChanged += OnWindowSizeChanged;
            EntityPanel.CallNext = CallNext;
            EntityPanel.CallNextEntityPanel = CallNextEntityPanel;
            AttributePanel.CallNext = CallNextToProperty;
            EntityPanel.ShowLoader = ShowLoader;
            EntityPanel.HideLoader = HideLoader;
            AttributePanel.ShowLoader = ShowLoader;
            AttributePanel.HideLoader = HideLoader;

            SingleAttributePanel.ShowLoader = ShowLoader;
            SingleAttributePanel.HideLoader = HideLoader;

            SingleEntityPanel.ShowLoader = ShowLoader;
            SingleEntityPanel.HideLoader = HideLoader;

        }

        public void ShowLoader()
        {
            MainContent.Visibility = Visibility.Hidden;
            Loader.Visibility = Visibility.Visible;
        }
        public void HideLoader()
        {
            MainContent.Visibility = Visibility.Visible;
            Loader.Visibility = Visibility.Hidden;
        }

        private void CallNext(string LogicalName)
        {
            AttributePanel.CurrentEntityName = LogicalName;
            PageControl = CurrentPage.AllAttributes;
            ShowAction();
        }

        private void CallNextEntityPanel(string LogicalName)
        {
            SingleEntityPanel.CurrentEntityName = LogicalName;
            PageControl = CurrentPage.SingleEntity;
            ShowAction();
        }
        private void CallNextToProperty(string LogicalName, string EnitityName)
        {
            SingleAttributePanel.CurrentEntityName = EnitityName;
            SingleAttributePanel.CurrentAttributeName = LogicalName;
            PageControl = CurrentPage.SingleAttributes;
            ShowAction();
        }

        private void OnReset(object sender = null, RoutedEventArgs e = null)
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
            if (PageControl == CurrentPage.SingleAttributes)
            {
                PageControl = CurrentPage.AllAttributes;
                ShowAction();
            }

            if (PageControl == CurrentPage.SingleEntity)
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
                SingleAttributePanel.Visibility = Visibility.Hidden;
                SingleEntityPanel.Visibility = Visibility.Hidden;


                EntityPanel.OnReset();
            }
            else if (PageControl == CurrentPage.AllAttributes)
            {
                EntityPanel.Visibility = Visibility.Hidden;
                AttributePanel.Visibility = Visibility.Visible;
                SingleAttributePanel.Visibility = Visibility.Hidden;
                SingleEntityPanel.Visibility = Visibility.Hidden;


                AttributePanel.OnReset();
            }
            else if (PageControl == CurrentPage.SingleAttributes)
            {
                EntityPanel.Visibility = Visibility.Hidden;
                AttributePanel.Visibility = Visibility.Hidden;
                SingleAttributePanel.Visibility = Visibility.Visible;
                SingleEntityPanel.Visibility = Visibility.Hidden;

                SingleAttributePanel.OnReset();
            }
            else if (PageControl == CurrentPage.SingleEntity)
            {
                EntityPanel.Visibility = Visibility.Hidden;
                AttributePanel.Visibility = Visibility.Hidden;
                SingleAttributePanel.Visibility = Visibility.Hidden;
                SingleEntityPanel.Visibility = Visibility.Visible;
                SingleEntityPanel.OnReset();
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
