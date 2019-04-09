using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    static class ConsoleOutput
    {
        public static void ConsoleLoop(BlockModel blockModel, string fileName)
        {
            while (true)
            {
                int xCoordinate;
                int yCoordinate;
                int zCoordinate;
                Console.WriteLine("x coordinate of the block");
                string xCoordinateString = Console.ReadLine();
                Console.WriteLine("x coordinate of the block");
                string yCoordinateString = Console.ReadLine();
                Console.WriteLine("x coordinate of the block");
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
