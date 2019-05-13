using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    class KeyGradePair
    {
        string key;
        double value;
        public KeyGradePair(string key, double value)
        {
            this.key = key;
            this.value = value;
        }
        public double GetValue()
        {
            return value;
        }
        public string GetKey()
        {
            return key;
        }
    }
}
