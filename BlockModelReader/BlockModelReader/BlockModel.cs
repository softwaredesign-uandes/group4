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

        public BlockModel()
        {

        }

        public void SetBlocks(List<Block> blocks)
        {
            this.blocks = blocks;
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
    }
}
