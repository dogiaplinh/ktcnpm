using System;
using System.Linq;

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