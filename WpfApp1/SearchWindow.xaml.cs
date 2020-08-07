using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace WpfApp1
{
    /// <summary>
    /// SearchWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchWindow : Window
    {
        private Window1 _window;
        private bool yearOption = true;

        public SearchWindow(Window1 window)
        {
            _window = window;
            InitializeComponent();
            //json파일로부터 사용자 옵션 설정내용(yearOption)을 가져와서 checkbox에 기본값으로 줌.
            searchBox.Focus();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            //이전 데이터 제거
            searchTable.Children.RemoveRange(8, searchTable.Children.Count - 8);
            String sql;

            if (yearOption)
            {
                sql = String.Format("SELECT * from (SELECT date, '일정' classify, calendar content FROM calendar WHERE calendar LIKE '%{0}%' UNION ", searchBox.Text);
                sql += String.Format("SELECT date, '보고공문' classify, report content FROM calendar WHERE report LIKE '%{0}%' UNION ", searchBox.Text);
                sql += String.Format("SELECT date, '복무' classify, service content FROM calendar WHERE service LIKE '%{0}%') WHERE date BETWEEN date('now', '-1 year') AND date('now', '+1 year')", searchBox.Text);
            }
            else
            {
                sql = String.Format("SELECT date, '일정' classify, calendar content FROM calendar WHERE calendar LIKE '%{0}%' UNION ", searchBox.Text);
                sql += String.Format("SELECT date, '보고공문' classify, report content FROM calendar WHERE report LIKE '%{0}%' UNION ", searchBox.Text);
                sql += String.Format("SELECT date, '복무' classify, service content FROM calendar WHERE service LIKE '%{0}%'", searchBox.Text);
            }

            
            var conn = new SQLiteConnection(App.filePath);

            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            //검색 결과 수 만큼 행 생성

            int rowCount = 1;

            while (rdr.Read())
            {
                RowDefinition gridRow = new RowDefinition();
                //gridRow.Height = new GridLength(50);
                searchTable.RowDefinitions.Add(gridRow);

                //경계 선 만들기
                Border border1 = new Border();
                Grid.SetRow(border1, rowCount);
                Grid.SetColumn(border1, 0);
                border1.BorderThickness= new Thickness(1, 0, 0, 1);
                border1.Background = Brushes.Transparent;
                border1.BorderBrush = Brushes.Black;
                searchTable.Children.Add(border1);

                Border border2 = new Border();
                Grid.SetRow(border2, rowCount);
                Grid.SetColumn(border2, 1);
                border2.BorderThickness = new Thickness(1, 0, 0, 1);
                border2.Background = Brushes.Transparent;
                border2.BorderBrush = Brushes.Black;
                searchTable.Children.Add(border2);

                Border border3 = new Border();
                Grid.SetRow(border3, rowCount);
                Grid.SetColumn(border3, 2);
                border3.BorderThickness = new Thickness(1, 0, 0, 1);
                border3.Background = Brushes.Transparent;
                border3.BorderBrush = Brushes.Black;
                searchTable.Children.Add(border3);

                Border border4 = new Border();
                Grid.SetRow(border4, rowCount);
                Grid.SetColumn(border4, 3);
                border4.BorderThickness = new Thickness(1, 0, 1, 1);
                border4.Background = Brushes.Transparent;
                border4.BorderBrush = Brushes.Black;
                searchTable.Children.Add(border4);

                //텍스트 만들기

                TextBlock dateText = new TextBlock();
                DateTime tempDateTime = Convert.ToDateTime(rdr["date"].ToString());
                dateText.Text = tempDateTime.ToString("yyyy-MM-dd (ddd)");
                dateText.Padding = new Thickness(5);
                dateText.TextAlignment = TextAlignment.Center;
                dateText.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(dateText, rowCount);
                Grid.SetColumn(dateText, 0);

                TextBlock classText = new TextBlock();
                classText.Text = rdr["classify"].ToString();
                classText.Padding = new Thickness(5);
                classText.TextAlignment = TextAlignment.Center;
                classText.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(classText, rowCount);
                Grid.SetColumn(classText, 1);

                TextBlock CoincideText = new TextBlock();
                CoincideText.Text = rdr["content"].ToString();
                CoincideText.Padding = new Thickness(5);
                CoincideText.TextWrapping = TextWrapping.Wrap;
                CoincideText.TextTrimming = TextTrimming.CharacterEllipsis;
                Grid.SetRow(CoincideText, rowCount);
                Grid.SetColumn(CoincideText, 2);

                Button linkBtn = new Button();

                ////리소스에서 이미지 가져오기
                //System.Drawing.Bitmap image = Properties.Resources._2268137;
                //MemoryStream imgStream = new MemoryStream();
                //image.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
                //imgStream.Seek(0, SeekOrigin.Begin);
                //BitmapFrame newimg = BitmapFrame.Create(imgStream);
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#673AB7");
                
                linkBtn.Content = new PackIcon { Kind = PackIconKind.Launch, Foreground = brush, Background = System.Windows.Media.Brushes.Transparent};
                linkBtn.HorizontalAlignment = HorizontalAlignment.Center;
                linkBtn.VerticalAlignment = VerticalAlignment.Center;
                linkBtn.Background = System.Windows.Media.Brushes.Transparent;
                linkBtn.BorderThickness = new Thickness(0);

                linkBtn.Click += (object eventSender, RoutedEventArgs eventArgs) => 
                {
                    int index = 0;
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime dtDate = tempDateTime;
                    _window.calendar.SelectedDate = dtDate;
                    _window.calendar.DisplayDate = dtDate;
                    switch (classText.Text)
                    {
                        case "일정":
                            index = 0;
                            break;
                        case "보고공문":
                            index = 1;
                            break;
                        case "복무":
                            index = 2;
                            break;
                    }
                    _window.ComboBox.SelectedIndex = index;
                };
                Grid.SetRow(linkBtn, rowCount);
                Grid.SetColumn(linkBtn, 3);


                searchTable.Children.Add(dateText);
                searchTable.Children.Add(CoincideText);
                searchTable.Children.Add(classText);
                searchTable.Children.Add(linkBtn);

                rowCount++;

            }

            rdr.Close();
            conn.Close();
            
        }

        

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                SearchBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            yearOption = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            yearOption = false;
        }
    }
}
