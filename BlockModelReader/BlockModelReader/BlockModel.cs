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
            blocks.Where(block => block.GetCoordinates()[0] < maxX).
                Select(block => block.GetCoordinates()[0]);
            blocks.Where(block => block.GetCoordinates()[1] < maxX).
                Select(block => block.GetCoordinates()[1]);
            blocks.Where(block => block.GetCoordinates()[2] < maxX).
                 Select(block => block.GetCoordinates()[2]);
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
            double totalAirBlocks = 0;
            int totalBlocks = blocks.Count();        
            foreach (Block block in blocks)
            {
                if (block.GetWeight() == 0) totalAirBlocks++;
            }
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

        public void ReBlock(int xAmount, int yAmount, int zAmount)
        {
            int id = 0;
            int blockCount = blocks.Count();
            int iCount = 0;
            for (int i = 0; i < maxX + 1; i+= xAmount)
            {
                int jCount = 0;
                for (int j = 0; j < maxY + 1; j += yAmount)
                {
                    int kCount = 0;
                    for (int k = 0; k < maxZ + 1; k += zAmount)
                    {
                        List<Block> cluster = GenerateReblockCluster(xAmount, yAmount, zAmount, i, j, k);

                        double weight = TotalWeight(cluster);
                        Dictionary<string, double> grades = CalculateClusterGrades(cluster, weight);
                        blocks.Add(new Block(id, iCount, jCount, kCount, weight, grades));
                        id++;
                        kCount++;
                    }
                    jCount++;
                }
                iCount++;
            }
            blocks.RemoveRange(0, blockCount);
        }

        private List<Block> GenerateReblockCluster(int xAmount, int yAmount, int zAmount, int startX, int startY, int startZ)
        {
            List<Block> cluster = new List<Block>();
            for (int iStep = 0; iStep < xAmount; iStep++)
            {
                for (int jStep = 0; jStep < yAmount; jStep++)
                {
                    for (int kStep = 0; kStep < xAmount; kStep++)
                    {
                        cluster.Add(SimpleCoordsQuery(startX + iStep, startY + jStep, startZ + kStep));
                    }
                }
            }
            return cluster;
        }

        public Dictionary<string, double> CalculateClusterGrades(List<Block> cluster, double weight)
        {
            Dictionary<string, double> grades = new Dictionary<string, double>();
            foreach (Block b in cluster)
            {
                if (b.GetGrades() != null)
                {
                    foreach (string key in b.GetGrades().Keys)
                    {
                        if (grades.Keys.Contains(key))
                        {
                            grades[key] += b.GetWeight() * b.GetGrades()[key];
                        }
                        else
                        {
                            grades[key] = b.GetWeight() * b.GetGrades()[key];
                        }
                    }
                }
            }

            foreach (string key in grades.Keys)
            {
                grades[key] /= weight;
                grades[key] = Math.Round(grades[key], 6);
            }

            return grades;
        }
    }
}
