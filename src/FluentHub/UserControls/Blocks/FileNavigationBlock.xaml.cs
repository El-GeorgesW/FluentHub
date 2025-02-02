﻿using FluentHub.ViewModels.Repositories;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using muxc = Microsoft.UI.Xaml.Controls;

namespace FluentHub.UserControls.Blocks
{
    public sealed partial class FileNavigationBlock : UserControl
    {
        #region propdp
        public static readonly DependencyProperty ContextViewModelProperty =
            DependencyProperty.Register(
                nameof(ContextViewModel),
                typeof(RepoContextViewModel),
                typeof(FileNavigationBlock),
                new PropertyMetadata(null));

        public RepoContextViewModel ContextViewModel
        {
            get { return (RepoContextViewModel)GetValue(ContextViewModelProperty); }
            set
            {
                SetValue(ContextViewModelProperty, value);
                ViewModel.ContextViewModel = ContextViewModel;
            }
        }
        #endregion

        public FileNavigationBlock() => InitializeComponent();

        #region chevronamination
        private void OnCloneButtonLoaded(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            button.AddHandler(PointerPressedEvent, new PointerEventHandler(OnCloneButtonPointerPressed), true);
            button.AddHandler(PointerReleasedEvent, new PointerEventHandler(OnCloneButtonPointerReleased), true);
        }

        private void OnCloneButtonPointerPressed(object sender, PointerRoutedEventArgs e) => SetState(sender as UIElement, "Pressed");

        private void OnCloneButtonPointerReleased(object sender, PointerRoutedEventArgs e) => SetState(sender as UIElement, "Normal");       

        public void SetState(UIElement target, string state)
        {
            if (target != null)
            {                
                muxc.AnimatedIcon.SetState(target, state);
            }
        }
        #endregion

        private void OnBranchSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
