using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace ConvenienceStore
{
    public sealed partial class MainPage : Page
    {
        public static int SELECTPAGE = 0;
        public static int mainViewId  = 0;
        public MainPage()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(Login));
            
            //rootFrame = new Frame();
            //Window.Current.Content = rootFrame;
            //rootFrame.Navigate(typeof(Login));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
//            CoreApplicationView newCoreView = CoreApplication.CreateNewView();
//;
//            ApplicationView newAppView = null;
//            mainViewId = ApplicationView.GetApplicationViewIdForWindow(
//              CoreApplication.MainView.CoreWindow);

//            await newCoreView.Dispatcher.RunAsync(
//              CoreDispatcherPriority.Normal,
//              () =>
//              {
//                  newAppView = ApplicationView.GetForCurrentView();
//                  SharedData.settingFrame = new Frame();
//                  Window.Current.Content = SharedData.settingFrame;
//                  SharedData.settingFrame.Navigate(typeof(OuterSet));
//                  Window.Current.Activate();
//                  //Window.Current.Content = new OuterSet();
//                  //Window.Current.Activate();
//              });

//            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
//              newAppView.Id,
//              ViewSizePreference.UseHalf,
//              mainViewId,
//              ViewSizePreference.UseHalf);
        }
    }
}
