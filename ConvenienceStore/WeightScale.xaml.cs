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
        static List<ProductBind> bind;
        static float totalWeight = 0;

        public WeightScale()
        {
            this.InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectJob.SELECTPAGE == 1)
                App.rootFrame.Navigate(typeof(Purchase), bind);
            else if (SelectJob.SELECTPAGE == 2)
                App.rootFrame.Navigate(typeof(Refund), bind);
            else
                Console.Write("SELECTPAGE ERROR : Back Btn : WeightScale");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bind = (List<ProductBind>)e.Parameter;
        }

        void OnCreate(object sender, RoutedEventArgs e)
        {
            totalWeight = 0;
        
            foreach (ProductBind product in bind)
            { 
                totalWeight += product.weight * product.count;
            }
            if (SelectJob.SELECTPAGE == 2) totalWeight *= -1;
        }

        public static async void CheckInputWeight()
        {
            if (SharedData.weight != 0)
            {
                if (SharedData.weight <= totalWeight * 1.01f
                    && SharedData.weight >= totalWeight * 0.99f)
                {
                    SharedData.initWeight();
                    totalWeight = 0;
                    App.rootFrame.Navigate(typeof(CardInsert), bind);
                    await ApplicationViewSwitcher.SwitchAsync(App.subViewId);
                }
                else
                {
                    ContentDialog test = new ContentDialog
                    {
                        Title = "무게 불일치",
                        Content = "상품 목록의 무게와 일치하지 않습니다.\n 상품을 바르게 올려주세요.",
                        CloseButtonText = "확인"
                    };

                    await test.ShowAsync();
                }
            }
        }
    }
}
