using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class PayComplete : Page
    {
        List<ProductBind> bind;
        private bool isMember = true;
        private int point = 0;
        private int totalCost = 0;
        public PayComplete()
        {
            this.InitializeComponent();

            isMember = true;
            point = 0;
            totalCost = 0;

            if (SelectJob.SELECTPAGE == 1)
                Complete.Text = "결제가 완료되었습니다.";
            else
                Complete.Text = "환불이 완료되었습니다.";

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bind = (List<ProductBind>)e.Parameter;
            checkPoint();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(SelectJob));
        }

        private void checkPoint()
        {
            // Point 있는지 검사하고 point bool 값 변경
            // 잔여 point 얼마인지
            // 구매 절차
            if (SelectJob.SELECTPAGE == 1)
            {
                if (SharedData.isMembership() == "1")
                {

                    Point.Text = SharedData.point + " 포인트가 적립되었습니다\n잔여 포인트는 " + 
                        (int.Parse(SharedData.membershipPoint) + SharedData.point) + "점 입니다";
                }
                else
                {
                    Point.Text = "1%씩 적립받을 수 있는\n멤버십에 가입해 보세요";
                }
            }
            // 환불 절차
            else
            {
                if (SharedData.isMembership() == "1")
                {
                    Point.Text = SharedData.point + " 포인트가 차감되었습니다\n잔여 포인트는 " +
                        (int.Parse(SharedData.membershipPoint) - SharedData.point) + "점 입니다";
                }
                else
                {
                    Point.Text = "1%씩 적립받을 수 있는\n멤버십에 가입해 보세요";
                }
            }
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(SelectJob));
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(SelectJob));
        }
    }
}
