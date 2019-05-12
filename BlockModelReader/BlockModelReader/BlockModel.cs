using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    public class BlockModel
    {
        private List<Block> blocks;
        private int maxX;
        private int maxY;
        private int maxZ;


        public BlockModel()
        {
            maxX = 0;
            maxY = 0;
            maxZ = 0;
        }

        public void SetBlocks(List<Block> blocks)
        {
            this.blocks = blocks;
            maxX = blocks.Max(block => block.GetCoordinates()[0]);
            maxY = blocks.Max(block => block.GetCoordinates()[1]);
            maxZ = blocks.Max(block => block.GetCoordinates()[2]);
        }

        public List<Block> GetBlocks()
        {
            return blocks;
        }

        public Block SimpleIdQuery(int id)
        {
            IEnumerable<Block> filteringQuery =
            from block in blocks
            where block.GetId() == 3
            select block;
            return filteringQuery.First();
        }

        public List<Block> SimpleXQuery(int xCoordinate)
        {
            IEnumerable<Block> filteringQuery =
            from block in blocks
            where block.GetCoordinates()[0] == xCoordinate
            select block;
            return filteringQuery.ToList();

        }

        public List<Block> SimpleYQuery(int yCoordinate)
        {
            IEnumerable<Block> filteringQuery =
            from block in blocks
            where block.GetCoordinates()[0] == yCoordinate
            select block;
            return filteringQuery.ToList();

        }

        public List<Block> SimpleZQuery(int zCoordinate)
        {
            IEnumerable<Block> filteringQuery =
            from block in blocks
            where block.GetCoordinates()[0] == zCoordinate
            select block;
            return filteringQuery.ToList();
        }

        public Block SimpleCoordsQuery(int xCoordinate, int yCoordinate, int zCoordinate)
        {
            IEnumerable<Block> filteringQuery =
            from block in blocks
            where block.GetCoordinates()[0] == xCoordinate
            && block.GetCoordinates()[1] == yCoordinate
            && block.GetCoordinates()[2] == zCoordinate
            select block;
            if (filteringQuery.Count() == 0)
            {
                return new Block(0, 0, 0, 0, 0);
            }
            return filteringQuery.First();
        }

        public double TotalWeight(List<Block> blocks)
        {
            double totalWeight = blocks.Select(block => block.GetWeight()).Sum();
            return totalWeight;
        }

        public double AirBlocksPercentage(List<Block> blocks)
        {
            int totalBlocks = blocks.Count();
            double totalAirBlocks = blocks.Where(block => block.GetWeight() == 0).Select(
                block => block.GetWeight()).Sum();
            double airBlocksPercentage = totalAirBlocks / totalBlocks;
            return airBlocksPercentage;
        }

        public Dictionary<string, double> MineralWeights(List<Block> blocks)
        {
            Dictionary<string, double> mineralWeight = new Dictionary<string, double>();

            foreach (Block block in blocks)
            {
                Dictionary<string, double> grades = block.GetGrades();
                foreach (var grade in grades.Keys)
                {
                    double gradeWeight = grades[grade] * block.GetWeight();
                    if (!mineralWeight.ContainsKey(grade))
                    {
                        mineralWeight.Add(grade, gradeWeight);
                    }

                    else
                    {
                        mineralWeight[grade] += gradeWeight;
                    }
                }
            }
            return mineralWeight;
        }

        public void ReBlock(int xReblockDimension, int yReblockDimension, int zReblockDimension)
        {
            int blockCount = blocks.Count();
            int xSteps = ((maxX + 1) / xReblockDimension) + 1;
            int ySteps = ((maxY + 1) / yReblockDimension) + 1;
            int zSteps = ((maxZ + 1) / zReblockDimension) + 1;
            int[] numberOfIterarions = Enumerable.Range(0, xSteps * ySteps * zSteps).ToArray();

            if (xReblockDimension == 0 || yReblockDimension == 0 || zReblockDimension == 0
                || new int[] { xReblockDimension, yReblockDimension, zReblockDimension }.All(x => x == 1))
            {
                return;
            }
            blocks = numberOfIterarions.Select(i =>
            {
                int x = i / (ySteps * zSteps);
                int y = (i / zSteps) % ySteps;
                int z = i % zSteps;
                int cummulativeX = x * xReblockDimension;
                int cummulativeY = y * yReblockDimension;
                int cummulativeZ = z * zReblockDimension;
                List<Block> cluster = GenerateReblockCluster(xReblockDimension, yReblockDimension, zReblockDimension, cummulativeX, cummulativeY, cummulativeZ);
                double weight = TotalWeight(cluster);
                Dictionary<string, double> grades = CalculateClusterGrades(cluster, weight);
                return new Block(i, x, y, z, weight, grades);
            }).ToList();
        }

        public List<Block> GenerateReblockCluster(int xDimension, int yDimension, int zDimension, int startX, int startY, int startZ)
        {
            int[] numberOfIterarions = Enumerable.Range(0, xDimension * yDimension * zDimension).ToArray();
            List<Block> cluster = numberOfIterarions.Select(i =>
            {
                int x = i / (yDimension * zDimension);
                int y = (i / zDimension) % yDimension;
                int z = i % zDimension;
                return SimpleCoordsQuery(startX + x, startY + y, startZ + z);
            }).ToList();
            return cluster;
        }

        public Dictionary<string, double> CalculateClusterGrades(List<Block> cluster, double weight)
        {
            Dictionary<string, double> grades = MineralWeights(cluster);
            Dictionary<string, double> newGrades = new Dictionary<string, double>();
            newGrades = grades.Keys.ToList().Select(key =>
            {
                double gradeValue = grades[key] / weight;
                gradeValue = Math.Round(gradeValue, 6);
                return new KeyGradePair(key, gradeValue);
            }).ToDictionary(keyGradePair => keyGradePair.GetKey(), keyGradePair => keyGradePair.GetValue());
            return newGrades;
        }
    }
}
