using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    public class BlockModel
    {
        private List<IReblockable> blocks;
        private int maxX;
        private int maxY;
        private int maxZ;
        private int id;

        public BlockModel(int id)
        {
            maxX = 0;
            maxY = 0;
            maxZ = 0;
            this.id = id;
        }

        public int GetId()
        {
            return id;
        }

        public void SetBlocks(List<IReblockable> blocks)
        {
            this.blocks = blocks;
            maxX = blocks.Max(block => block.GetCoordinates()[0]);
            maxY = blocks.Max(block => block.GetCoordinates()[1]);
            maxZ = blocks.Max(block => block.GetCoordinates()[2]);
        }

        public List<IReblockable> GetBlocks()
        {
            return blocks;
        }

        public IReblockable SimpleIdQuery(int id)
        {
            IEnumerable<IReblockable> filteringQuery =
            from block in blocks
            where block.GetId() == 3
            select block;
            return filteringQuery.First();
        }

        public IReblockable SimpleCoordsQuery(int xCoordinate, int yCoordinate, int zCoordinate)
        {
            IEnumerable<IReblockable> filteringQuery =
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

        public double GetTotalWeight(List<IReblockable> blocks)
        {
            double totalWeight = blocks.Select(block => block.GetWeight()).Sum();
            return totalWeight;
        }

        public double CalculateAirBlocksPercentage(List<IReblockable> blocks)
        {
            int totalBlocks = blocks.Count();
            double totalAirBlocks = blocks.Where(block => block.GetWeight() == 0).Select(
                block => block.GetWeight()).Sum();
            double airBlocksPercentage = totalAirBlocks / totalBlocks;
            return airBlocksPercentage;
        }

        public Dictionary<string, double> CalculateMineralWeights(List<IReblockable> blocks)
        {
            Dictionary<string, double> mineralWeight = new Dictionary<string, double>();

            foreach (IReblockable block in blocks)
            {
                Dictionary<string, double> grades = block.GetGrades();
                grades = grades.Keys.ToList().Select(grade =>
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
                    return new KeyGradePair(grade, gradeWeight);
                }).ToDictionary(KeyGradePair => KeyGradePair.GetKey(), KeyGradePair => KeyGradePair.GetValue());
            }
            return mineralWeight;
        }

        public void ReBlock(int xReblockDimension, int yReblockDimension, int zReblockDimension, bool isVirtual)
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
                int y = i / zSteps % ySteps;
                int z = i % zSteps;
                int cummulativeX = x * xReblockDimension;
                int cummulativeY = y * yReblockDimension;
                int cummulativeZ = z * zReblockDimension;
                List<IReblockable> cluster = GenerateReblockCluster(xReblockDimension, yReblockDimension, zReblockDimension, cummulativeX, cummulativeY, cummulativeZ);
                if (isVirtual)
                {
                    return (IReblockable)new BlockGroup(i, x, y, z, cluster);
                }
                else
                {
                    double weight = GetTotalWeight(cluster);
                    Dictionary<string, double> grades = CalculateClusterGrades(cluster, weight);
                    return (IReblockable)new Block(i, x, y, z, weight, grades);
                }
            }).ToList();
        }

        public List<IReblockable> GenerateReblockCluster(int xDimension, int yDimension, int zDimension, int startX, int startY, int startZ)
        {
            int[] numberOfIterarions = Enumerable.Range(0, xDimension * yDimension * zDimension).ToArray();
            List<IReblockable> cluster = numberOfIterarions.Select(i =>
            {
                int x = i / (yDimension * zDimension);
                int y = (i / zDimension) % yDimension;
                int z = i % zDimension;
                return SimpleCoordsQuery(startX + x, startY + y, startZ + z);
            }).ToList();
            return cluster;
        }

        public Dictionary<string, double> CalculateClusterGrades(List<IReblockable> cluster, double weight)
        {
            Dictionary<string, double> grades = CalculateMineralWeights(cluster);
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
