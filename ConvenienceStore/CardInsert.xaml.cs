using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace ConvenienceStore
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class CardInsert : Page
    {
        List<ProductBind> bind;

        public CardInsert()
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

        public static void CheckInputCard()
        {
            if (SharedData.cardNum != null)
            {
                SharedData.initCardNum();
                //결제 완료 페이지로 이동 + 몇초 있다가 메인으로 ㄱㄱ
                //App.rootFrame.Navigate(typeof(Refund));
            }
        }
    }
}
