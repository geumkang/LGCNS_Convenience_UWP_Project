using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore
{
    static class SharedDataValue
    {
        public static float weight = 0f;
        public static bool userState = false;
        public static string cardNum = null;

        public static void initWeight() { weight = 0f; }
        public static void changeUserState() { userState = !userState; }
    }
}
