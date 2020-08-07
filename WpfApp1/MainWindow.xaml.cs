using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public SomeObjectClass obj = new SomeObjectClass();

        public MainWindow()
        {
            InitializeComponent();
            Clock.DataContext = obj;
            obj.CurrentTimeString = DateTime.Now.ToString("yyyy년 MMMM dd일 dddd\n\n tt hh:mm");

            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                obj.CurrentTimeString = DateTime.Now.ToString("yyyy년 MMMM dd일 dddd\n\n tt hh:mm");
            }, this.Dispatcher);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var newWindow = new Window1();
            newWindow.Show();
            this.Close();
        }
    }

    public class SomeObjectClass : INotifyPropertyChanged
    {
        private string currentTimeString = "";
        public string CurrentTimeString
        {
            get
            {
                return currentTimeString;
            }
            set
            {
                currentTimeString = value;
                OnPropertyChanged("CurrentTimeString");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
