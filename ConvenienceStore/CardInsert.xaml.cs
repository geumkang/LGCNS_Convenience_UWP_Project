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
        static List<ProductBind> bind;

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

                string purchaseID = SharedData.storeID + "_" + CurrentTimeMillis().ToString();

                // 입력 DB를 넣어라
                string query = "insert into tradeHistory values('" + purchaseID + 
                    "', '" + SharedData.memberID + "', '" + SharedData.storeID + "', '" + SharedData.isDiscount() + "', '" +
                    SharedData.isMembership() + "', now() , '" + SharedData.cardNum + "')";
                SqlManager.insertQuery(query);

                for(int i = 0; i < bind.Count; i++)
                {
                    query = "insert into tradeDetail set productID = '" + bind[i].id + "', tradeHistoryID = '" + purchaseID
                        + "', amount = " + bind[i].count + ", unitPrice = (select price from product where productID = '"
                        + bind[i].id + "')";
                    SqlManager.insertQuery(query);
                }

                if (SharedData.membershipPoint != null)
                {
                    int point = 0;
                    foreach (ProductBind product in bind)
                    {
                        point += product.totalCost;
                    }
                    point = (int)(point * 0.01);
                    SharedData.point = point;
                    query = "insert into pointHistory set tradeHistoryId = '" + purchaseID + "', memberID = '" 
                        + SharedData.memberID + "', point = " + point;
                    SqlManager.insertQuery(query);
                    query = "update member set membershipPoint = membershipPoint + " + point + " where memberID = '" + SharedData.memberID 
                        + "'";
                    SqlManager.insertQuery(query);
                }

                SharedData.initCardNum();
                //결제 완료 페이지로 이동 + 몇초 있다가 메인으로 ㄱㄱ
                App.rootFrame.Navigate(typeof(PayComplete), bind);
            }
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}
