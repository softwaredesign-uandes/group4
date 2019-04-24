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
            int iCount = 0;
            for (int i = 0; i < maxX+1; i+= xAmount)
            {
                
                int jCount = 0;
                for (int j = 0; j < maxY+1; j += yAmount)
                {
                    int kCount = 0;
                    
                    for (int k = 0; k < maxZ+1; k += zAmount)
                    {
                        
                        List<Block> cluster = new List<Block>();
                        for (int ii = 0; ii < xAmount; ii++)
                        {
                            for (int jj = 0; jj < yAmount; jj++)
                            {
                                for (int kk = 0; kk < xAmount; kk++)
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
                        List<string> keys = new List<string>(grades.Keys);
                        foreach (string key in keys)
                        {
                            grades[key] /= weight;
                            grades[key] = Math.Round(grades[key], 6);
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
