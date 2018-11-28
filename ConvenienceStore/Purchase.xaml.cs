﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace ConvenienceStore
{
    public sealed partial class Purchase : Page
    {
        List<Product> products;
        List<ProductBind> productBinds;

        TotalInfo totalInfo;
        
        public Purchase()
        {

            this.InitializeComponent();

            products = new List<Product>();
            productBinds = new List<ProductBind>();
            //PurchaseProductList.ItemsSource = productBinds;

            totalInfo = new TotalInfo() { totalCost = 0, weight = 0f };
            //totalCostTxtBlock.DataContext = totalInfo;
            //weightTxtBlock.DataContext = totalInfo;

            //string connStr = "server=localhost;user=root;database=mycontacts;port=3306;password=123";
            //StringBuilder sb = new StringBuilder();
            //MySqlConnection conn = new MySqlConnection(connStr);
            //conn.Open();
            //string sql = "SELECT * FROM info";
            //MySqlCommand cmd = new MySqlCommand(sql, conn);
            //MySqlDataReader rdr = cmd.ExecuteReader();

            //while (rdr.Read())
            //{
            //    sb.AppendLine(rdr[1].ToString());
            //}
            
            //conn.Close();
        }

        private async void addProductBtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            var dialog = new AddProduct { Text = "" };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {

            }

            this.IsEnabled = true;
        }

        private async void discountBtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            var dialog = new AddDiscount { Text = "" };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {

            }
            
            this.IsEnabled = true;
        }

        private void payBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;
            parentFrame.Navigate(typeof(Purchase)); 
        }

        private void addProductTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
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
                    
                        

                // 같은 이름 상품이 이미 등록됬을 때
                if(isExist)
                {
                    products[i].setCount(products[i].getCount() + 1);
                    products[i].setTotalCost(products[i].getCount() * products[i].getCost());
                    productBinds[i].count = products[i].getCount();
                    productBinds[i].totalCost = products[i].getTotalCost();

                    totalInfo.totalCost += products[i].getCost();
                    totalInfo.weight += products[i].getWeight();
                }
                // 없을 때
                else
                {
                    Product p = new Product(addProductTxtBox.Text, 1000, 2, 5.5f);
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
    }
}
