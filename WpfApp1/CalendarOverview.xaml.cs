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
    /// CalendarOverview.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CalendarOverview : Window
    {
        public CalendarOverview(Window owner)
        {
            InitializeComponent();
            Owner = owner;
            Left = Owner.Left - Width;
            Top = Owner.Top + Owner.Height - Height - 8;
            owner.LocationChanged += (object sender, EventArgs e) => 
            {
                Left = Owner.Left - Width;
                Top = Owner.Top + Owner.Height - Height - 8;
            };
            ShowActivated = false;
            
            
            
        }
    }
}
