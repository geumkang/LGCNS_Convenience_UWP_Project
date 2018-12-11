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
    public sealed partial class Refund : Page
    {
        double DiscountRate = 1.0;
        List<Product> products;
        List<ProductBind> productBinds;
        List<ProductBind> buyHistory;
        TotalInfo totalInfo;
        Boolean isDiscounted;
        Boolean pass;

        public Refund()
        {

            this.InitializeComponent();

            isDiscounted = false;

            products = new List<Product>();
            productBinds = new List<ProductBind>();
            buyHistory = new List<ProductBind>();
            if(SharedData.discountCard != null)
            {
                isDiscounted = true;
                DiscountRate = 0.9;
            }
            totalInfo = new TotalInfo() { totalCost = 0, weight = 0f };

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                buyHistory = (List<ProductBind>)e.Parameter;
            }
        }
        
        private async void payBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectJob.SELECTPAGE = 2;
            App.rootFrame.Navigate(typeof(WeightScale), productBinds);
            await ApplicationViewSwitcher.SwitchAsync(App.subViewId);
        }

        private async void addProductTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                for (int i = 0; i < buyHistory.Count; i++)
                {
                    if(buyHistory[i].id == addProductTxtBox.Text)
                    {
                        pass = true;
                        break;
                    }
                    pass = false;
                }
                if(!pass)
                {
                    ContentDialog test = new ContentDialog
                    {
                        Title = "구매내역 없음",
                        Content = "입력한 상품을 구매내역에서 찾을 수 없습니다.",
                        CloseButtonText = "확인"
                    };

                    await test.ShowAsync();
                }
                else
                {
                    // 서버한테 정보 받아오기
                    string query = "select * from product where productID = '" + addProductTxtBox.Text + "'";
                    string productInfo = SqlManager.query(query, 7);
                    if (productInfo.Length > 0)
                    {
                        Product product = getProductInfo(productInfo);
                        int i;
                        Boolean isExist = false;
                        for (i = 0; i < productBinds.Count; i++)
                        {
                            if (productBinds[i].name == product.getName())
                            {
                                isExist = true;
                                break;
                            }
                        }

                        double discount = 1.0;
                        if (isDiscounted)
                            discount = DiscountRate;

                        // 같은 이름 상품이 이미 등록됬을 때
                        if (isExist)
                        {
                            int cost = products[i].getCost();
                            // 수량 갯수 파악하기
                            foreach(ProductBind bind in buyHistory)
                            {
                                if(bind.id == products[i].getID())
                                {
                                    if(products[i].getCount() * -1 == bind.count)
                                    {
                                        ContentDialog test = new ContentDialog
                                        {
                                            Title = "수량초과",
                                            Content = "구매한 상품 이상을 반품할 수 없습니다",
                                            CloseButtonText = "확인"
                                        };

                                        await test.ShowAsync();
                                        return;
                                    }
                                }
                            }

                            products[i].setCount(products[i].getCount() - 1);
                            products[i].setTotalCost(products[i].getCount() * (int)(cost * discount));
                            //products[i].setDiscount(products[i].getCount() * (int)(cost * (1 - discount)));
                            products[i].setDiscount(products[i].getCost() * products[i].getCount() - products[i].getTotalCost());

                            productBinds[i].count--;
                            productBinds[i].totalCost = products[i].getTotalCost();
                            productBinds[i].discount = products[i].getDiscount();

                            totalInfo.totalCost -= (int)(cost * discount);
                            totalInfo.weight += products[i].getWeight();
                        }
                        // 없을 때
                        else
                        {
                            product.setTotalCost((int)(product.getTotalCost() * discount));
                            product.setDiscount(product.getCost() * product.getCount() - product.getTotalCost());

                            products.Add(product);
                            productBinds.Add(product.bindData());

                            totalInfo.totalCost += product.getTotalCost();
                            totalInfo.weight += product.getCount() * product.getWeight() * -1;
                        }

                        resetBindData();
                    }
                    else
                    {
                        ContentDialog test = new ContentDialog
                        {
                            Title = "상품조회 실패",
                            Content = "입력한 상품을 찾을 수 없습니다.",
                            CloseButtonText = "확인"
                        };

                        await test.ShowAsync();
                    }
                }
                addProductTxtBox.Text = "";


            }
        }

        private Product getProductInfo(string info)
        {
            char[] parameter = { '\n', '\t' };
            string[] infos = info.Split(parameter);
            Product product = new Product(infos[0], infos[5], int.Parse(infos[3]), -1, float.Parse(infos[6]));

            return product;
        }

        private void deleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductBind temp = (ProductBind)(sender as FrameworkElement).DataContext;
            removeProductBind(temp.name);

            this.PurchaseProductList.SelectedItem = (sender as FrameworkElement).DataContext;

        }

        private void removeProductBind(string name)
        {
            int i;
            for (i = 0; i < productBinds.Count; i++)
                if (productBinds[i].name == name)
                    break;

            totalInfo.totalCost -= products[i].getTotalCost();
            totalInfo.weight -= products[i].getWeight() * products[i].getCount(); ;

            products.RemoveAt(i);
            productBinds.RemoveAt(i);

            resetBindData();
        }

        private void resetBindData()
        {
            PurchaseProductList.ItemsSource = null;
            PurchaseProductList.ItemsSource = productBinds;
            totalCostTxtBlock.DataContext = null;
            weightTxtBlock.DataContext = null;
            totalCostTxtBlock.DataContext = totalInfo;
            weightTxtBlock.DataContext = totalInfo;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(SelectJob));
        }
    }
}
