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
            Console.WriteLine("Block Model Loader");
            Console.Write("Name of the file: ");
            string fileName = Console.ReadLine().ToLower();
            FileReader.ReadFile(blockModel, fileName, fileName+".blocks");
            ConsoleOutput.ConsoleLoop(blockModel, fileName);
        }
    }
}
