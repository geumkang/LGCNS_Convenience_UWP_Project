using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace ConvenienceStore
{
    public sealed partial class Purchase : Page
    {
        double DiscountRate = 0.9;
        List<Product> products;
        List<ProductBind> productBinds;
        TotalInfo totalInfo;
        Boolean isDiscounted;

        public Purchase()
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
                    for(int i = 0; i < productBinds.Count; i++)
                    {
                        products.Add(new Product(productBinds[i].id, productBinds[i].name, productBinds[i].cost, productBinds[i].count, productBinds[i].weight, productBinds[i].discount));
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
                var cardNum = dialog.Text;
                // 멤버십 여부 확인
                string query = "select isDiscount from tradeHistory where to_days(tradeDate) = to_days(now()) and isDiscount = '" + (string)cardNum + "'";
                string haveDiscountHis = SqlManager.query(query, 1);

                if (haveDiscountHis.Length > 0)
                {
                    ContentDialog test = new ContentDialog
                    {
                        Title = "중복된 할인카드",
                        Content = "할인은 1일 1회만 가능합니다.",
                        CloseButtonText = "확인"
                    };

                    await test.ShowAsync();
                }
                else
                {
                    SharedData.discountCard = (string)cardNum;
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
            }

            this.IsEnabled = true;
        }

        private async void payBtn_Click(object sender, RoutedEventArgs e)
        {
            if (productBinds.Count != 0)
            {
                SelectJob.SELECTPAGE = 1;
                App.rootFrame.Navigate(typeof(WeightScale), productBinds);
                await ApplicationViewSwitcher.SwitchAsync(App.subViewId);
            }
        }

        private async void addProductTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
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
                        product.setTotalCost((int)(product.getTotalCost() * discount));
                        product.setDiscount(product.getCost() * product.getCount() - product.getTotalCost());

                        products.Add(product);
                        productBinds.Add(product.bindData());

                        totalInfo.totalCost += product.getTotalCost();
                        totalInfo.weight += product.getCount() * product.getWeight();
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
                addProductTxtBox.Text = "";


            }
        }

        private Product getProductInfo(string info)
        {
            char[] parameter = { '\n', '\t' };
            string[] infos = info.Split(parameter);
            Product product = new Product(infos[0], infos[5], int.Parse(infos[3]), 1, float.Parse(infos[6]));

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
