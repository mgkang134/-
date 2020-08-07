using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
using System.Windows.Threading;
using Path = System.IO.Path;

public enum EComboButton
{
    Calendar,
    Report,
    Service

}

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        public FileStream fileStream;
        private CalendarOverview subWindow;
        public string now;
        public string path;
        public const int autoSaveInterval = 200;     //miliseconds


        DispatcherTimer dtClockTime;

        public Window1()
        {
            InitializeComponent();

            

            //타이머 초기화 및 세팅
            dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 0, 0, autoSaveInterval);
            dtClockTime.Tick += dtTimeClock_Tick;

            //캘린더의 오늘을 선택
            calendar.SelectedDate = DateTime.Now;

            

        }

        private EComboButton GetCurrentSelectedChoice()
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    return EComboButton.Calendar;
                case 1:
                    return EComboButton.Report;
                case 2:
                    return EComboButton.Service;
                default:
                    return EComboButton.Calendar;
            }
        }

        private void LoadSubWindowData(DateTime targetDate)
        {
            String sql = String.Format("SELECT * FROM calendar WHERE date = '{0}'", targetDate.ToString("yyyy-MM-dd"));

            var conn = new SQLiteConnection(App.filePath);

            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                subWindow.CalendarTextBox.Text = rdr["calendar"].ToString();
                subWindow.ReportTextBox.Text = rdr["report"].ToString();
                subWindow.ServiceTextBox.Text = rdr["service"].ToString();
            }

            rdr.Close();
            conn.Close();
        }


        private void ReadDB(DateTime targetDate, EComboButton choice)
        {
            String sql = String.Format("SELECT * FROM calendar WHERE date = '{0}'", targetDate.ToString("yyyy-MM-dd"));

            var conn = new SQLiteConnection(App.filePath);

            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            string content = null;


            //처음 작성하는 날짜의 경우에는 DB검색 결과가 없다. 그러므로 서브윈도우를 무조건적으로 공백으로
            //초기화 해주어 새로고침 효과를 낸다.
            if (subWindow != null)
            {
                subWindow.CalendarTextBox.Text = "";
                subWindow.ReportTextBox.Text = "";
                subWindow.ServiceTextBox.Text = "";
            }

            while (rdr.Read())
            {
                switch (choice)
                {
                    case EComboButton.Calendar:
                        if(subWindow != null)
                        {
                            subWindow.CalendarTextBox.Text = rdr["calendar"].ToString();
                            subWindow.ReportTextBox.Text = rdr["report"].ToString();
                            subWindow.ServiceTextBox.Text = rdr["service"].ToString();
                        }
                        
                            content = rdr["calendar"].ToString();
                        break;
                    case EComboButton.Report:
                        if (subWindow != null)
                        {
                            subWindow.CalendarTextBox.Text = rdr["calendar"].ToString();
                            subWindow.ReportTextBox.Text = rdr["report"].ToString();
                            subWindow.ServiceTextBox.Text = rdr["service"].ToString();
                        }
                        content = rdr["report"].ToString();
                        break;
                    case EComboButton.Service:
                        if (subWindow != null)
                        {
                            subWindow.CalendarTextBox.Text = rdr["calendar"].ToString();
                            subWindow.ReportTextBox.Text = rdr["report"].ToString();
                            subWindow.ServiceTextBox.Text = rdr["service"].ToString();
                        }
                        content = rdr["service"].ToString();
                        break;
                }
            }

            rdr.Close();
            conn.Close();

            textBox.Text = content;
           
        }

        private Data MakeData(DateTime targetDate)
        {
            string dateStr = targetDate.ToString("yyyy/MM/dd");
            string calendarString = "";
            string reportString = "";
            string serviceString = "";

            String sql = String.Format("SELECT * FROM Calendar WHERE date = '{0}'", ((DateTime)(targetDate)).ToString("yyyy-MM-dd"));
            var conn = new SQLiteConnection(App.filePath);

            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                calendarString = rdr["calendar"].ToString();
                reportString = rdr["report"].ToString();
                serviceString = rdr["service"].ToString();
            }

            rdr.Close();
            conn.Close();

            Data data = new Data(targetDate, calendarString, reportString, serviceString);
            return data;
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
            DateTime dateTime = (DateTime)e.AddedItems[0];
            ComboBox.SelectedIndex = 0;

            ReadDB(dateTime, GetCurrentSelectedChoice());

        }


        private void dtTimeClock_Tick(object sender, EventArgs e)
        {
            Task t1 = new Task(() =>
            {
                string sql = null;
                string textValue = "";
                EComboButton choice = EComboButton.Calendar;
                string dateString = null;
                DateTime datetime = DateTime.Now;

                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    textValue = textBox.Text;

                    choice = GetCurrentSelectedChoice();
                    datetime = (DateTime)calendar.SelectedDate;
                    dateString = datetime.ToString("yyyy-MM-dd");
                }));

                switch (choice)
                {
                    case EComboButton.Calendar:
                        sql = String.Format("INSERT INTO calendar (" +
                    "date, calendar) values ('{0}', '{1}') ON CONFLICT(date) DO UPDATE SET calendar = '{2}'", dateString, textValue, textValue);
                        break;
                    case EComboButton.Report:
                        sql = String.Format("INSERT INTO calendar (" +
                    "date, report) values ('{0}', '{1}') ON CONFLICT(date) DO UPDATE SET report = '{2}'", dateString, textValue, textValue);
                        break;
                    case EComboButton.Service:
                        sql = String.Format("INSERT INTO calendar (" +
                    "date, service) values ('{0}', '{1}') ON CONFLICT(date) DO UPDATE SET service = '{2}'", dateString, textValue, textValue);
                        break;
                }

                var conn = new SQLiteConnection(App.filePath);
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();

                conn.Close();

                //저장 이후 타이머 중지
                dtClockTime.Stop();

                //변경된 데이터를 DB에 저장하였으므로 SubWindow또한 변경된 데이터로 갱신한다.
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    switch (choice)
                    {
                        case EComboButton.Calendar:
                            subWindow.CalendarTextBox.Text = textValue;
                            break;
                        case EComboButton.Report:
                            subWindow.ReportTextBox.Text = textValue;
                            break;
                        case EComboButton.Service:
                            subWindow.ServiceTextBox.Text = textValue;
                            break;
                    }
                }));

            });

            t1.Start();
        }


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //텍스트가 변화하고 나서 0.5초간 아무런 응답이 없으면 (백그라운드로)저장한다.
            //텍스트가 변화할때 마다 타이머의 시간을 0초로 초기화.
            if (dtClockTime == null)
            {
                return;
            }

            dtClockTime.Stop();
            dtClockTime.Start();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate == null)
                return;

            ReadDB((DateTime)calendar.SelectedDate, GetCurrentSelectedChoice());
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            Data data = MakeData((DateTime)calendar.SelectedDate);
            var newWindow = new PrintOverview(data);

            //창이 뜨는 위치 조정
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            newWindow.Left = (desktopWorkingArea.Right - newWindow.Width) * 0.5f;
            newWindow.Top = Top;

            newWindow.Show();

            
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new SearchWindow(this);
            //창이 뜨는 위치 조정
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            newWindow.Left = (desktopWorkingArea.Right - newWindow.Width) * 0.5f;
            newWindow.Top = Top;

            newWindow.Show();
        }

        private void todayBtn_Click(object sender, RoutedEventArgs e)
        {
            calendar.SelectedDate = DateTime.Now;
            calendar.DisplayDate = DateTime.Now;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //일정 내용을 보여줄 서브 윈도우를 만든다.
            subWindow = new CalendarOverview(this);
            subWindow.Owner = this;

            //현재 날짜의 choice에 해당하는 내용을 DB에서 읽어옴
            ReadDB(DateTime.Now, GetCurrentSelectedChoice());

            subWindow.Show();
        }

        private void ShowHideButton_Click(object sender, RoutedEventArgs e)
        {
            //서브윈도우가 없는 상태에서 버튼을 눌렀을 경우
            if(subWindow.Visibility == Visibility.Hidden)
            {
                subWindow.Show();
                ((PackIcon)ShowHideBtn.Content).Kind = PackIconKind.ArrowRightBold;
            }
            else
            {
                subWindow.Hide();
                ((PackIcon)ShowHideBtn.Content).Kind = PackIconKind.ArrowLeftBold;
            }

        }
    }
}
