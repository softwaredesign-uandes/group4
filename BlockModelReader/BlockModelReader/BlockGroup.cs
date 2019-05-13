using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    class BlockGroup : IReblockable
    {
        private List<IReblockable> blocks;
        private int id;
        private int xCoordinate;
        private int yCoordinate;
        private int zCoordinate;
        public BlockGroup(int id, int xCoordinate, int yCoordinate, int zCoordinate, List<IReblockable> blocks)
        {
            this.id = id;
            this.blocks = blocks;
        }

        public int[] GetCoordinates()
        {
            return new[] { xCoordinate, yCoordinate, zCoordinate };
        }

        public Dictionary<string, double> GetGrades()
        {
            throw new NotImplementedException();
        }

        public int GetId()
        {
            throw new NotImplementedException();
        }

        public double GetWeight()
        {
            throw new NotImplementedException();
        }
    }
}
