using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinDemo
{
    public class OrderInfo
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SumMoney { get; set; }
        public string Comment { get; set; }
        public bool Finished { get; set; }

        public int Add(int a, int b)
        {
            return a + b;
        }
    }

}
