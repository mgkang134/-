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

namespace WpfApp1
{
    /// <summary>
    /// PrintOverview.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PrintOverview : Window
    {
        public PrintOverview(Data data)
        {
            InitializeComponent();


            Grid01.Text = data.Date.ToString("yyyy-MM-dd (ddd)");
            Grid11.Text = data.calendarText;
            Grid21.Text = data.reportText;
            Grid31.Text = data.serviceText;



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(grid, "My First Print Job");
            }
        }
    }
}
