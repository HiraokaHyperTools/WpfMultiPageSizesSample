using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMultiPageSizesSample.Models
{
    public class MyPageSize
    {
        /// <summary>
        /// A4, A3, B4 など
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 縦置き?
        /// </summary>
        public bool tate { get; set; }

        public override string ToString() => $"{name} {(tate ? "縦" : "横")}";
    }
}
