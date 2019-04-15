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
            Console.Write("Expression for Weight: ");
            string weightExpression = Console.ReadLine().ToLower();
            Console.Write("Number of Minerals: ");
            int numberOfMinerals = int.Parse(Console.ReadLine().ToLower());
            string[] gradeNames = new string[numberOfMinerals];
            string[] gradeExpressions = new string[numberOfMinerals];
            for (int i = 0; i < numberOfMinerals; i++)
            {
                Console.Write("Mineral " + (i+1) +" Name : ");
                gradeNames[i] = Console.ReadLine();
                Console.Write("Expression for " + gradeNames[i] + " Grade : ");
                gradeExpressions[i] = Console.ReadLine().ToLower();
            }

            FileReader.ReadFile(blockModel, weightExpression, gradeNames, gradeExpressions, fileName + ".blocks");
            //ConsoleOutput.PrintStatistics(blockModel);
            ConsoleOutput.ConsoleLoop(blockModel, fileName);
            Console.ReadKey();
        }
    }
}
