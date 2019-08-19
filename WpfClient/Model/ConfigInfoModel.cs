using System.Configuration;
using System.Windows.Forms;
using System.Windows.Media;

namespace Model
{
    public class ConfigInfoModel
    {
        public ConfigInfoModel()
        {

        }

        private static int _winScreenWidth;
        public static int WinScreenWidth
        {
            get
            {
                if (_winScreenWidth.Equals(0))
                {
                    _winScreenWidth = Screen.PrimaryScreen.Bounds.Width;
                }
                return _winScreenWidth;
            }
        }

        private static int _winScreenHeight;
        public static int WinScreenHeight
        {
            get
            {
                if (_winScreenHeight.Equals(0))
                {
                    _winScreenHeight = Screen.PrimaryScreen.Bounds.Height;
                }
                return _winScreenHeight;
            }
        }

        private static string _api = ConfigurationManager.AppSettings["API"].ToString();

        private static string _ftpUid = ConfigurationManager.AppSettings["FtpUid"].ToString();

        private static string _ftpPwd = ConfigurationManager.AppSettings["FtpPwd"].ToString();

        private static string _ftpSite = ConfigurationManager.AppSettings["FtpSite"].ToString();

        //private static string _api = "120.79.95.158:80";

        private static string _docDirName = ConfigurationManager.AppSettings["DocDirName"].ToString();

        private static string _serialPortName = ConfigurationManager.AppSettings["SerialPortName"].ToString();

        private static string _printMachineName = ConfigurationManager.AppSettings["PrintMachineName"].ToString();


        /// <summary>
        /// API地址
        /// </summary>
        //public static string API { get; set; } = "127.0.0.1";
        public static string API
        {
            get { return _api; }
            set { _api = value; }
        }

        /// <summary>
        /// FTP账号
        /// </summary>
        public static string FtpUid
        {
            get { return _ftpUid; }
            set { _ftpUid = value; }
        }

        /// <summary>
        /// FTP密码
        /// </summary>
        public static string FtpPwd
        {
            get { return _ftpPwd; }
            set { _ftpPwd = value; }
        }

        /// <summary>
        /// FTP站点
        /// </summary>
        public static string FtpSite
        {
            get { return _ftpSite; }
            set { _ftpSite = value; }
        }

        public enum UserKeyValue
        {
            /// <summary>
            /// Tab
            /// </summary>
            UKeyTab = 3,
            /// <summary>
            /// Enter
            /// </summary>
            UKeyEnter = 6,
            /// <summary>
            /// 左
            /// </summary>
            UKeyLeft = 23,
            /// <summary>
            /// 上
            /// </summary>
            UKeyTop = 24,
            /// <summary>
            /// 右
            /// </summary>
            UKeyRight = 25,
            /// <summary>
            /// 下
            /// </summary>
            UKeyBottom = 26,
            /// <summary>
            /// ＋
            /// </summary>
            UKeyPlus = 141,
            /// <summary>
            /// -
            /// </summary>
            UKeySubtract = 143,
            /// <summary>
            /// 左Ctrl
            /// </summary>
            UKeyLCtrl = 118,
            /// <summary>
            /// 右Ctrl
            /// </summary>
            UKeyRCtrl = 119
        }


        /// <summary>
        /// 下载图纸存放的文件夹名
        /// </summary>
        public static string DocDirName
        {
            get { return _docDirName; }
            set { _docDirName = value; }
        }

        /// <summary>
        /// 串口名
        /// </summary>
        public static string SerialPortName
        {
            get { return _serialPortName; }
            set { _serialPortName = value; }
        }

        /// <summary>
        /// 打印机名称
        /// </summary>
        public static string PrintMachineName
        {
            get { return _printMachineName; }
            set { _printMachineName = value; }
        }

        public static class ColorSetting
        {
            private static Color _ban = Color.FromRgb(220, 97, 65);
            private static Color _notice = Color.FromRgb(165, 165, 65);
            private static Color _instruct = Color.FromRgb(66, 105, 165);
            private static Color _safety = Color.FromRgb(57, 130, 90);
            private static Color _txtColorA = Color.FromRgb(240, 240, 240);


            /// <summary>
            /// 红色-禁止、停止、消防和危险
            /// </summary>
            //public static Color Ban { get; set; } = Color.FromRgb(255, 200, 200);
            public static Color Ban { get { return _ban; } set { _ban = value; } }

            /// <summary>
            /// 黄色-注意、警告
            /// </summary>
            //public static Color Notice { get; set; } = Color.FromRgb(255, 255, 200);
            public static Color Notice { get { return _notice; } set { _notice = value; } }

            /// <summary>
            /// 蓝色-指令、必须遵守的规定
            /// </summary>
            //public static Color Instruct { get; set; } = Color.FromRgb(200, 200, 255);
            public static Color Instruct { get { return _instruct; } set { _instruct = value; } }

            /// <summary>
            /// 绿色-通行、安全和提供信息
            /// </summary>
            //public static Color Safety { get; set; } = Color.FromRgb(200, 255, 200);
            public static Color Safety { get { return _safety; } set { _safety = value; } }


            //public static Color txtColorA { get; set; } = Color.FromRgb(128, 128, 255);
            public static Color TxtColorA { get { return _txtColorA; } set { _txtColorA = value; } }

        }
    }
}
