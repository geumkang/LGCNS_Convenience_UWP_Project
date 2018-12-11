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
    public sealed partial class BillRegister : Page
    {
        static List<ProductBind> productBinds;
        public BillRegister()
        {
            productBinds = new List<ProductBind>();
            this.InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
             App.rootFrame.Navigate(typeof(SelectJob));
        }

        public static async void CheckInputBill()
        {
            if (SharedData.billNum != null)
            {
                // DB 검색
                string query = "select tradeHistoryID from tradehistory where tradeHistoryID = '" + SharedData.billNum + "'";
                string temp = SqlManager.query(query, 1);
                if(temp.Length > 2)
                {
                    query = "select isDiscount from tradehistory where tradeHistoryID = '" + SharedData.billNum + "'";
                    string discountCard = SqlManager.query(query, 1);
                    discountCard = discountCard.Substring(0, discountCard.Length - 2);
                    SharedData.discountCard = discountCard;
                    query = "select * from tradedetail where tradeHistoryID = '" + SharedData.billNum + "'";
                    temp = SqlManager.query(query, 4);
                    makeBinds(temp);
                    
                    App.rootFrame.Navigate(typeof(Refund), productBinds);
                }
                else
                {
                    ContentDialog test = new ContentDialog
                    {
                        Title = "미확인 구매내역",
                        Content = "확인할 수 없는 구매내역 입니다. 점포명을 확인해 주세요",
                        CloseButtonText = "확인"
                    };

                    ContentDialogResult Result = await test.ShowAsync();
                }
            }
        }

        private static void makeBinds(string text)
        {
            char[] parameter = { '\n', '\t' };
            string[] temp = text.Split('\n');

            foreach(string one in temp)
            {
                if (one != "")
                {
                    string[] info = one.Split('\t');
                    string query = "select * from product where productID = '" + info[0] + "'";
                    string productInfo = SqlManager.query(query, 7);
                    string[] infos = productInfo.Split(parameter);

                    Product product = new Product(infos[0], infos[5], int.Parse(info[3]), int.Parse(info[2]), float.Parse(infos[6]));
                    productBinds.Add(product.bindData());
                }
            }
        }
    }
}
