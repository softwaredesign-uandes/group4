using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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
            foreach(Block b in blocks)
            {
                if (maxX < b.GetCoordinates()[0]) maxX = b.GetCoordinates()[0];
                if (maxY < b.GetCoordinates()[1]) maxY = b.GetCoordinates()[1];
                if (maxZ < b.GetCoordinates()[2]) maxZ = b.GetCoordinates()[2];
            }
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
            double totalWeight = 0;
            foreach (Block block in blocks)
            {
                totalWeight += block.GetWeight();                
            }
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
                    if (!mineralWeight.ContainsKey(grade)) mineralWeight.Add(grade, grades[grade]);
                    else mineralWeight[grade] += grades[grade] * block.GetWeight();
                }
            }
            return mineralWeight;
        }

        public void ReBlock(int xAmount, int yAmount, int zAmount)
        {
            int id = 0;
            int count = blocks.Count();
            for(int i = 0; i < maxX; i+= xAmount)
            {
                int iCount = 0;
                for (int j = 0; j < maxY; j += yAmount)
                {
                    int jCount = 0;
                    for (int k = 0; k < maxZ; k += zAmount)
                    {
                        int kCount = 0;
                        List<Block> cluster = new List<Block>();
                        for (int ii = 1; ii < xAmount - 1; ii++)
                        {
                            for (int jj = 1; jj < yAmount - 1; jj++)
                            {
                                for (int kk = 1; kk < xAmount - 1; kk++)
                                {
                                    cluster.Add(SimpleCoordsQuery(i + ii, j + jj, k + kk));
                                }
                            }
                        }
                        double weight = 0;
                        Dictionary<string, double> grades = new Dictionary<string, double>();
                        foreach (Block b in cluster)
                        {
                            weight += b.GetWeight();
                            if(b.GetGrades() != null)
                            {
                                foreach(string key in b.GetGrades().Keys)
                                {
                                    grades[key] += b.GetGrades()[key];
                                }
                            }
                        }
                        foreach (string key in grades.Keys)
                        {
                            grades[key] = grades[key] / weight;
                        }
                        blocks.Add(new Block(id, iCount, jCount, kCount, weight, grades));
                        id++;
                        kCount++;
                    }
                    jCount++;
                }
                iCount++;
            }
            blocks.RemoveRange(0, count);
        }

    }
}
