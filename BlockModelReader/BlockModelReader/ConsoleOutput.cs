using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    public static class ConsoleOutput
    {

        public static void PrintStatistics(BlockModel blockModel)
        {
            List<IReblockable> blocks = blockModel.GetBlocks();
            int totalBlocks = blocks.Count;
            double totalWeight = blockModel.GetTotalWeight(blocks);
            Dictionary<string, double> mineralWeight = blockModel.CalculateMineralWeights(blocks);
            double airBlocksPercentage = blockModel.CalculateAirBlocksPercentage(blocks);
            Console.WriteLine("Statistics");
            Console.WriteLine("1.-Number of Blocks: " + totalBlocks);
            Console.WriteLine("2.-Total Weight of the Deposit: " + totalWeight);
            Console.WriteLine("3.-Total Mineral Weight: ");
            List<string> mineralWeightKeys = mineralWeight.Keys.ToList();
            mineralWeightKeys.ForEach(
                mineral => Console.WriteLine("    *" + mineral + ": " + mineralWeight[mineral]
                ));
            Console.WriteLine("4.-Air Blocks percentage: " + airBlocksPercentage + "%");

        }
        public static void ConsoleLoop(BlockModel blockModel, string fileName)
        {
            while (true)
            {
                int xCoordinate;
                int yCoordinate;
                int zCoordinate;
                string[] inputCoordinates = SetInputCoordinates();
                try
                {
                    xCoordinate = int.Parse(inputCoordinates[0]);
                    yCoordinate = int.Parse(inputCoordinates[1]);
                    zCoordinate = int.Parse(inputCoordinates[2]);
                    IReblockable queryResult = blockModel.SimpleCoordsQuery(xCoordinate, yCoordinate, zCoordinate);
                    PrintBlockSpecifications(queryResult);
                }
                catch (Exception)
                {
                    Console.WriteLine("One or more arguments are not integers");
                }
            }
        }

        public static string[] SetInputCoordinates()
        {
            string[] inputCoordinates =  new string[3];
            Console.WriteLine("x coordinate of the block");
            inputCoordinates[0] = Console.ReadLine();
            Console.WriteLine("y coordinate of the block");
            inputCoordinates[1] = Console.ReadLine();
            Console.WriteLine("z coordinate of the block");
            inputCoordinates[2] = Console.ReadLine();
            return inputCoordinates;
        } 

        public static void PrintBlockSpecifications(IReblockable queryResult)
        {
            int[] coordinates = queryResult.GetCoordinates();
            Console.WriteLine("Block specifications:\n" +
                        "Coordinate x: " + coordinates[0] + "\n" +
                        "Coordinate y: " + coordinates[1] + "\n" +
                        "Coordintae z: " + coordinates[2] + "\n" +
                        "Block Weight: " + queryResult.GetWeight());
            Dictionary<string, double> gradesDictionary = queryResult.GetGrades();
            List<string> gradesDictionaryKeys = gradesDictionary.Keys.ToList();
            gradesDictionaryKeys.ForEach(
                grade => Console.WriteLine(grade + " grade: " + gradesDictionary[grade])
                );
        }
    }
}
