using System.Collections.Generic;

namespace AdmixtureWeb.Models
{
    public class Cluster
    {
        public int Index { get; set; }

        public IEnumerable<string> Label { get; set; }

        public IEnumerable<double> X { get; set; }

        public IEnumerable<double> Y { get; set; }

        public IEnumerable<double> Z { get; set; }
    }
}