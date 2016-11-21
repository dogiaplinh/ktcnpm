using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class YearItem : BindableBase
    {
        private int year;

        public int Year
        {
            get { return year; }
            set { SetProperty(ref year, value); }
        }
    }
}
