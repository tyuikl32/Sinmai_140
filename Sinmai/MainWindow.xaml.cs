using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sinmai
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isStartedUp = false;
        Uri Startup;
        Uri BUDDiES;
        public MainWindow()
        {
            InitializeComponent();
            // 获取嵌入的资源的流
            Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Sinmai.startup.mp4");

            // 获取一个临时文件的路径
            string tempFilePath = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".mp4");


            // 将资源流写入到这个临时文件中
            using (FileStream fileStream = File.Create(tempFilePath))
            {
                resourceStream.CopyTo(fileStream);
            }

            // 创建一个指向这个临时文件的URI
            Startup = new Uri(tempFilePath);

            resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Sinmai.BUDDiES.mp4");

            // 获取一个临时文件的路径
            tempFilePath = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".mp4"); 

            // 将资源流写入到这个临时文件中
            using (FileStream fileStream = File.Create(tempFilePath))
            {
                resourceStream.CopyTo(fileStream);
            }

            // 创建一个指向这个临时文件的URI
            BUDDiES = new Uri(tempFilePath);
        }

        private void OnLateInitialized(object sender, EventArgs e)
        {


            LeftMonitor.Height = this.ActualHeight;
            LeftMonitor.Width = LeftMonitor.Height * 0.5625;
            RightMonitor.Height = this.ActualHeight;
            RightMonitor.Width = RightMonitor.Height * 0.5625;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            LeftMonitor.Height = this.ActualHeight - 40;
            LeftMonitor.Width = LeftMonitor.Height * 0.5625;
            RightMonitor.Height = this.ActualHeight - 40;
            RightMonitor.Width = RightMonitor.Height * 0.5625;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LeftMonitor.Source = Startup;
            RightMonitor.Source = Startup;
            
            if (!File.Exists("config_common.json"))
            {
                LeftMonitor.Source=new Uri("",UriKind.Relative);
                RightMonitor.Source = new Uri("", UriKind.Relative);
                LeftMonitor.Stop();
                RightMonitor.Stop();
                return;
            }
            string processName = "amdaemon"; // 这里以Chrome浏览器为例

            // 使用GetProcessesByName方法获取指定名称的进程
            Process[] processes = Process.GetProcessesByName(processName);

            // 如果返回的Process数组长度大于0，说明该进程存在
            if (processes==null)
            {
                LeftMonitor.Source = new Uri("", UriKind.Relative);
                RightMonitor.Source = new Uri("", UriKind.Relative);
                LeftMonitor.Stop();
                RightMonitor.Stop();
                return;
            }
            LeftMonitor.Play();
            RightMonitor.Play();
        }

        private void LeftMonitor_MediaEnded(object sender, RoutedEventArgs e)
        {
            if(isStartedUp)
            {
            LeftMonitor.Position = TimeSpan.Zero;
            LeftMonitor.Play();
                RightMonitor.Position = TimeSpan.Zero;
                RightMonitor.Play();
            }
            else
            {
                isStartedUp = true;
                LeftMonitor.Source = BUDDiES;
                RightMonitor.Source = BUDDiES;
                LeftMonitor.Position = TimeSpan.Zero;
                LeftMonitor.Play();
                RightMonitor.Position = TimeSpan.Zero;
                RightMonitor.Play();
            }
           
        }

        private void RightMonitor_MediaEnded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
