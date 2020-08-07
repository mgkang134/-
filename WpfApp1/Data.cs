using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WpfApp1
{
    public class Data
    {
        public DateTime Date;
        public string calendarText;
        public string reportText;
        public string serviceText;

        public Data(DateTime date, string calendarText, string reportText, string serviceText)
        {
            Date = date;
            this.calendarText = calendarText;
            this.reportText = reportText;
            this.serviceText = serviceText;
        }
    }
}
