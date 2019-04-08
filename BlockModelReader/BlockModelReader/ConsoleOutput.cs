using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    static class ConsoleOutput
    {
        public static void ConsoleLoop(BlockModel blockModel)
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
                    Console.WriteLine(queryResult);
                }
                catch (Exception)
                {
                    Console.WriteLine("One or more arguments are not integers");
                }


            }
        }
    }
}
