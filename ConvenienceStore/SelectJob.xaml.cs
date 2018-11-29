﻿using System;
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
    public sealed partial class SelectJob : Page
    {
        public SelectJob()
        {
            this.InitializeComponent();
            time T = new time();
            T.Date = System.DateTime.Now.ToString("yyyy년 MM월 dd일 hh시 mm분");

            time.DataContext = T;
        }

        private void Purchase_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Purchase));
            //Frame parentFrame = Window.Current.Content as Frame;
            //parentFrame.Navigate(typeof(Purchase));
        }

        private void Refund_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Refund));
        }
    }

    
}
class time
{
    public string Date { get; set; }
}
