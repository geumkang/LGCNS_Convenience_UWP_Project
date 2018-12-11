using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class SelectJob : Page
    {
        public static int SELECTPAGE = 0;
        public SelectJob()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(480, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            SqlManager.connectDB();
            time T = new time();
            T.Date = System.DateTime.Now.ToString("yyyy년 MM월 dd일 hh시 mm분");

            time.DataContext = T;
        }

        private void Purchase_Click(object sender, RoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(Purchase));
        }

        private async void Refund_Click(object sender, RoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(BillRegister));
            await ApplicationViewSwitcher.SwitchAsync(App.subViewId);
        }
    }
}
class time
{
    public string Date { get; set; }
}
