using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class OuterSet : Page
    {
        public OuterSet()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }
        
        private async void ID_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if(SharedData.userState == true)
                {
                    ContentDialog test = new ContentDialog
                    {
                        Title = "입장 불가",
                        Content = "매장 안에 손님이 있습니다.",
                        CloseButtonText = "확인"
                    };
                    ContentDialogResult Result = await test.ShowAsync();
                    
                }
                else
                {
                    string temp = "select now() from member";
                    string now = SqlManager.query(temp, 1);
                    string[] time = now.Split('\t');
                    SharedData.insertTime = time[0];
                    // DB에서 User 정보 검색
                    string memberID = SqlManager.query("select memberID from member where memberID = '" + ID.Text + "'", 1);
                    if(memberID.Length > 0)
                    {
                        memberID = memberID.Substring(0, memberID.Length - 2);
                        SharedData.memberID = memberID;
                        string query = "select date_format(now(), '%Y') - date_format(birthDate, '%Y') + 1 from member where memberID = '" + memberID + "'";
                        string age = SqlManager.query(query, 1);
                        age = age.Substring(0, age.Length - 2);
                        SharedData.age = int.Parse(age);
                        query = "select membershipPoint from member where memberID = '" + memberID + "'";
                        string membershipPoint = SqlManager.query(query, 1);
                        if(membershipPoint.Length > 0)
                        {
                            membershipPoint = membershipPoint.Substring(0, membershipPoint.Length - 2);
                            SharedData.membershipPoint = membershipPoint;
                        }
                        if (int.Parse(age) < 20)
                        {
                            ContentDialog test = new ContentDialog
                            {
                                Title = "미성년자 알림",
                                Content = "미성년자 입니다. 성인용품 물품함을 잠급니다.",
                                CloseButtonText = "확인"
                            };

                            ContentDialogResult Result = await test.ShowAsync();
                        }
                        
                        query = "insert into enterLog values('" + CurrentTimeMillis().ToString() +
                            "', '" + SharedData.memberID + "', '" + SharedData.storeID + "', now(), null)";
                        SqlManager.insertQuery(query);

                        // 멤버 객체 생성
                        // Log 작성
                        SharedData.userState = true;

                        Usable.Text = "사용 중";
                        Usable.Foreground = new SolidColorBrush(Colors.Red);
                    } else
                    {
                        ContentDialog test = new ContentDialog
                        {
                            Title = "회원 불일치",
                            Content = "일치하는 회원이 없습니다.",
                            CloseButtonText = "확인"
                        };

                        ContentDialogResult Result = await test.ShowAsync();
                    }
                    
                }

                ID.Text = "";
            }
        }

        private void Exit_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter )
            {
                if (SharedData.userState == true)
                {
                    // DB에서 User 정보 검색
                    SharedData.userState = false;

                    //DB
                    string query = "update enterlog set outTime = now() where outTime is null";
                    SqlManager.insertQuery(query);

                    SharedData.initMemberInfo();

                    Usable.Text = "사용 가능";
                    Usable.Foreground = new SolidColorBrush(Color.FromArgb(255,0,120,212));
                }
                Exit.Text = "";
            }
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private async void Weight_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && Weight.Text != "")
            {
                SharedData.weight = float.Parse(Weight.Text);
                Weight.Text = "";

                await ApplicationViewSwitcher.SwitchAsync(App.mainViewId);
                await CoreApplication.MainView.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        Frame frame = Window.Current.Content as Frame;
                        if(frame.SourcePageType == typeof(WeightScale))
                        {
                            WeightScale.CheckInputWeight();
                        }
                    });
            }
        }

        private async void Card_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SharedData.cardNum = Card.Text;
                Card.Text = "";

                await ApplicationViewSwitcher.SwitchAsync(App.mainViewId);
                await CoreApplication.MainView.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        Frame frame = Window.Current.Content as Frame;
                        if (frame.SourcePageType == typeof(CardInsert))
                        {
                            // DB에서 카드 정보 + 적립 ~
                            CardInsert.CheckInputCard();
                        }
                    });
            }
        }

        private async void Bill_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SharedData.billNum = Bill.Text;
                Bill.Text = "";

                await ApplicationViewSwitcher.SwitchAsync(App.mainViewId);
                await CoreApplication.MainView.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        Frame frame = Window.Current.Content as Frame;
                        if (frame.SourcePageType == typeof(BillRegister))
                        {
                            // DB에서 구매 이력 + 영수증 번호 비교

                            BillRegister.CheckInputBill();
                        }
                    });
            }
        }
    }
}
