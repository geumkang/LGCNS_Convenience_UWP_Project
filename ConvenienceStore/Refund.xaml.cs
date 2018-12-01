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
    public sealed partial class Refund : Page
    {
        double DiscountRate = 0.9;
        List<Product> products;
        List<ProductBind> productBinds;
        TotalInfo totalInfo;
        Boolean isDiscounted;

        public Refund()
        {

            this.InitializeComponent();

            products = new List<Product>();
            productBinds = new List<ProductBind>();
            totalInfo = new TotalInfo() { totalCost = 0, weight = 0f };

            isDiscounted = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                productBinds = (List<ProductBind>)e.Parameter;
                if (productBinds.Count > 0)
                {
                    if (productBinds[0].discount != 0)
                    {
                        isDiscounted = true;
                        discountBtn.IsEnabled = false;
                    }
                    for (int i = 0; i < productBinds.Count; i++)
                    {
                        products.Add(new Product(productBinds[i].name, productBinds[i].cost, productBinds[i].count, productBinds[i].weight, productBinds[i].discount));
                        totalInfo.totalCost += productBinds[i].totalCost;
                        totalInfo.weight += productBinds[i].weight * productBinds[i].count;
                    }
                    resetBindData();
                }
            }
        }

        private async void discountBtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            var dialog = new AddDiscount { Text = "" };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // 멤버십 여부 확인


                for (int i = 0; i < products.Count; i++)
                {
                    products[i].setTotalCost((int)(products[i].getTotalCost() * DiscountRate));
                    productBinds[i].totalCost = products[i].getTotalCost();

                    products[i].setDiscount(products[i].getCost() * products[i].getCount() - products[i].getTotalCost());
                    productBinds[i].discount = products[i].getDiscount();
                }

                totalInfo.totalCost = (int)(totalInfo.totalCost * DiscountRate);

                resetBindData();

                isDiscounted = true;
                discountBtn.IsEnabled = false;
            }

            this.IsEnabled = true;
        }

        private void payBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPage.SELECTPAGE = 2;
            Frame parentFrame = Window.Current.Content as Frame;
            parentFrame.Navigate(typeof(WeightScale), productBinds);
        }

        private void addProductTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // 서버한테 정보 받아오기
                int i;
                Boolean isExist = false;
                for (i = 0; i < productBinds.Count; i++)
                {
                    if (productBinds[i].name == addProductTxtBox.Text)
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

                    products[i].setCount(products[i].getCount() + 1);
                    products[i].setTotalCost(products[i].getCount() * (int)(cost * discount));
                    //products[i].setDiscount(products[i].getCount() * (int)(cost * (1 - discount)));
                    products[i].setDiscount(products[i].getCost() * products[i].getCount() - products[i].getTotalCost());

                    productBinds[i].count = products[i].getCount();
                    productBinds[i].totalCost = products[i].getTotalCost();
                    productBinds[i].discount = products[i].getDiscount();

                    totalInfo.totalCost += (int)(cost * discount);
                    totalInfo.weight += products[i].getWeight();
                }
                // 없을 때
                else
                {
                    Product p = new Product(addProductTxtBox.Text, 1000, 2, 5.5f);
                    p.setTotalCost((int)(p.getTotalCost() * discount));
                    p.setDiscount(p.getCost() * p.getCount() - p.getTotalCost());

                    products.Add(p);
                    productBinds.Add(p.bindData());

                    totalInfo.totalCost += p.getTotalCost();
                    totalInfo.weight += p.getCount() * p.getWeight();
                }

                resetBindData();

                addProductTxtBox.Text = "";


            }
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
            Frame parentFrame = Window.Current.Content as Frame;
            parentFrame.Navigate(typeof(SelectJob));
        }
    }
}
