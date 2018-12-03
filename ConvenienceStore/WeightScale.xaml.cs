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

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace ConvenienceStore
{
    public sealed partial class WeightScale : Page
    {
        List<ProductBind> bind;
        static float totalWeight = 0;

        public WeightScale()
        {
            this.InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;
            if (MainPage.SELECTPAGE == 1)
                parentFrame.Navigate(typeof(Purchase), bind);
            else if (MainPage.SELECTPAGE == 2)
                parentFrame.Navigate(typeof(Refund), bind);
            else
                Console.Write("SELECTPAGE ERROR : Back Btn : WeightScale");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bind = (List<ProductBind>)e.Parameter;
        }

        async void OnCreate(object sender, RoutedEventArgs e)
        {
            CoreApplicationView newCoreView = CoreApplication.CreateNewView();

            ApplicationView newAppView = null;
            int mainViewId = ApplicationView.GetApplicationViewIdForWindow(
              CoreApplication.MainView.CoreWindow);

            await newCoreView.Dispatcher.RunAsync(
              CoreDispatcherPriority.Normal,
              () =>
              {
                  newAppView = ApplicationView.GetForCurrentView();
                  Window.Current.Content = new OuterSet();
                  Window.Current.Activate();
              });

            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
              newAppView.Id,
              ViewSizePreference.UseHalf,
              mainViewId,
              ViewSizePreference.UseHalf);

            foreach(ProductBind product in bind)
            { 
                totalWeight += product.weight * product.count;
            }
            CheckInputWeight();
        }

        static void CheckInputWeight()
        {
            while(true)
            {
                if (SharedDataValue.weight != 0)
                {
                    if (SharedDataValue.weight <= totalWeight * 1.01f
                        && SharedDataValue.weight >= totalWeight * 0.99f)
                    {
                        SharedDataValue.initWeight();
                        Frame parentFrame = Window.Current.Content as Frame;
                        parentFrame.Navigate(typeof(CardInsert));

                        break;
                    }
                    SharedDataValue.initWeight();
                }
            }
        }
    }
}
