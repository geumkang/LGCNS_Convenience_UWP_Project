using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ConvenienceStore
{
    static class SharedData
    {
        public static string storeID = "storeID1";
        public static float weight = 0f;
        public static bool userState = false;
        public static string cardNum = null;
        public static string billNum = null;
        public static string insertTime = null;
        public static string memberID = null;
        public static int age = 0;
        public static string discountCard = null;
        public static string membershipPoint = null;
        public static int point = 0;

        public static void initWeight() { weight = 0f; }
        public static void initUserState() { userState = false; }
        public static void initCardNum() { cardNum = null; }
        public static void initBillNum() { billNum = null; }
        public static void changeUserState() { userState = !userState; }
        public static void initMemberInfo()
        {
            memberID = null;
            insertTime = null;
            age = 0;
            discountCard = null;
            membershipPoint = null;
            point = 0;
        }

        public static string isDiscount()
        {
            if(discountCard == null)
            {
                return "0";
            }
            return discountCard;
        }

        public static string isMembership()
        {
            if(membershipPoint == null)
            {
                return "0";
            }
            return "1";
        }
    }
}
