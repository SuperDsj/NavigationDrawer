using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace NavigationDrawerPopUpMenu2
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        private ImageSource createQRCode(string content, int width, int height)
        {
            EncodingOptions options;
            //包含一些编码、大小等的设置
            //BarcodeWriter :一个智能类来编码一些内容的条形码图像
            BarcodeWriter write = null;
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
                Margin = 0
            };
            write = new BarcodeWriter();
            //设置条形码格式
            write.Format = BarcodeFormat.QR_CODE;
            //获取或设置选项容器的编码和渲染过程。
            write.Options = options;
            //对指定的内容进行编码，并返回该条码的呈现实例。渲染属性渲染实例使用，必须设置方法调用之前。
            Bitmap bitmap = write.Write(content);
            IntPtr bmpHandle = bitmap.GetHbitmap();
            //从GDI+ Bitmap创建GDI位图对象
            //Imaging.CreateBitmapSourceFromHBitmap方法，基于所提供的非托管位图和调色板信息的指针，返回一个托管的BitmapSource
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmpHandle, IntPtr.Zero, Int32Rect.Empty,
                                            BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(bmpHandle);

            return bitmapSource;
        }
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }

        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (Agent_account.Text == "1001" && Agent_pass.Password == "mycomm123")
            {
                this.DialogResult = Convert.ToBoolean(1);
                this.Close();
            }
            else
            {
                this.txtmessage.Content="账号或密码错误!";
            }
        }

        private void LoginRegister_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("iexplore.exe", " www.mycomm.cn");
        }

        private void LoginOther_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Process.Start("iexplore.exe", " www.mycomm.cn");
            string LoginTime = GetTimeStamp(DateTime.Now);
            if (LoginOther.Text == "账号登陆")
            {
                LoginOther.Text = "更多登陆方式";
                this.LoginRectangle.Visibility = Visibility.Visible;
                this.LoginGrid.Visibility = Visibility.Visible;
                this.BtnLogin.Visibility = Visibility.Visible;
                this.Login2Image.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginOther.Text = "账号登陆";
                this.LoginRectangle.Visibility = Visibility.Hidden;
                this.LoginGrid.Visibility = Visibility.Hidden;
                this.BtnLogin.Visibility = Visibility.Hidden;
                this.Login2Image.Visibility = Visibility.Visible;
                this.Login2Image.Source = createQRCode("通过手机登陆，还需要添加登陆的接口页面,当前时间戳:"+ LoginTime, 250, 250);
            }
           
        }

        private void LoginContactUs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("iexplore.exe", " www.mycomm.cn");
        }
    }
}
