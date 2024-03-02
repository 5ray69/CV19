using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CV19.Models
{
    internal class PlaceInfo
    {
        public string Name { get; set; }

        //Point-существующая структура в языке C#
        public Point Location { get; set; }

        public IEnumerable<ConfirmedCount> Count { get; set; }
    }
}
