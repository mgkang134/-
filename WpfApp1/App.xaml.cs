using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public static string filePath;

        private void Application_Startup(object sender, StartupEventArgs e)
        
        {
            
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moses of School");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                SQLiteConnection.CreateFile(Path.Combine(dir, "calendar.sqlite"));
            }

            //DB연결
            filePath = "Data Source =" + Path.Combine(dir, "calendar.sqlite") + "; Version = 3;";
            var conn = new SQLiteConnection(filePath);
            
            conn.Open();

            //테이블이 없으면 생성
            string sql = "create table if not exists Calendar (date text primary key, calendar text, report text,service text)";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();


            Window1 wnd = new Window1();
            wnd.Title = String.Format("학교의 노예 v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            MainWindow = wnd;

            //Uri iconUri = new Uri("pack://application:,,,/header_icon.ico", UriKind.RelativeOrAbsolute);
            //wnd.Icon = BitmapFrame.Create(iconUri);

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            wnd.Left = desktopWorkingArea.Right * 0.75f;
            wnd.Top = (desktopWorkingArea.Bottom - wnd.Height) * 0.5f;
            wnd.Show();
        }

    }
}
