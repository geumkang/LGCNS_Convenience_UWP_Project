﻿using System;
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
            Frame parentFrame = Window.Current.Content as Frame;
            parentFrame.Navigate(typeof(WeightScale), bind);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {   
            base.OnNavigatedTo(e);
            bind = (List<ProductBind>)e.Parameter;
        }
    }
}
