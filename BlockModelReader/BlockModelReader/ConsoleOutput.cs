using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    static class ConsoleOutput
    {

        public static void PrintStatistics(BlockModel blockModel)
        {
            int totalBlocks = blockModel.blocks.Count;
            double totalWeight = 0;
            int airBlocks = 0;
            double totalMineralWeight = 0;
            foreach (var block in blockModel.blocks)
            {
                Dictionary<string, double> grades = block.GetGrades();
                totalWeight += block.GetWeight();
                if (block.GetWeight() == 0) airBlocks++;
                foreach (var grade in block.GetGrades().Values)
                {
                    totalMineralWeight += grade*block.GetWeight();
                }
            }
            double airBlocksPercentage = airBlocks / totalBlocks;
            Console.WriteLine("Statistics");
            Console.WriteLine("1.-Number of Blocks: " + totalBlocks);
            Console.WriteLine("2.-Total Weight of the Deposit: " + totalWeight);
            Console.WriteLine("3.-Total Mineral Weight: " + totalMineralWeight);
            Console.WriteLine("4.-Air Blocks percentage: " + airBlocksPercentage);

        }
        public static void ConsoleLoop(BlockModel blockModel, string fileName)
        {
            while (true)
            {
                int xCoordinate;
                int yCoordinate;
                int zCoordinate;
                Console.WriteLine("x coordinate of the block");
                string xCoordinateString = Console.ReadLine();
                Console.WriteLine("y coordinate of the block");
                string yCoordinateString = Console.ReadLine();
                Console.WriteLine("z coordinate of the block");
                string zCoordinateString = Console.ReadLine();
                try
                {
                    xCoordinate = int.Parse(xCoordinateString);
                    yCoordinate = int.Parse(yCoordinateString);
                    zCoordinate = int.Parse(zCoordinateString);
                    Block queryResult = blockModel.SimpleCoordsQuery(xCoordinate, yCoordinate, zCoordinate);
                    Console.WriteLine("Block specifications:\n" +
                        "Coordinate x: " + xCoordinate + "\n" +
                        "Coordinate y: " + yCoordinate + "\n" +
                        "Coordintae z: " + zCoordinate + "\n" +
                        "Block Weight: " + queryResult.GetWeight());
                    switch (fileName)
                    {
                        case "marvin":
                            Console.WriteLine("Gold(%): " + queryResult.GetGrades()["Au"] + "\n" +
                            "Copper(%): " + queryResult.GetGrades()["Cu"]);
                            break;
                        case "zuck_small":
                            Console.WriteLine("Ore(%): " + queryResult.GetGrades()["Ore"]);
                            break;
                    }
   
                }
                catch (Exception)
                {
                    Console.WriteLine("One or more arguments are not integers");
                }

            }
        }
    }
}
