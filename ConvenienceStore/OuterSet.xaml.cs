using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class OuterSet : Page
    {
        public OuterSet()
        {
            this.InitializeComponent();

        }

        private void ID_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Weight.IsEnabled = true;
                //SharedData.ID = ID.Text;
                //ID.Text = "";

                //frame.Navigate(typeof(), SharedData.ID);
            }

        }

        private void Exit_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Weight_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SharedData.weight = float.Parse(Weight.Text);
                Weight.Text = "";
            }

        }

        private void Card_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Bill_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }
    }
}
