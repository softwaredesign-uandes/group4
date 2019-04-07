using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    class Program
    {
        static void Main(string[] args)
        {
            BlockModel blockModel = new BlockModel();
            blockModel.ReadFile("marvin", "marvin.blocks");
        }
    }
}
