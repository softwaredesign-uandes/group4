using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    class Block
    {
        private int id, xCoordinate, yCoordinate, zCoordinate;
        private double weight;
        private Dictionary<string, double> grades;
        private Dictionary<string, double> additionalData;
        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.grades = new Dictionary<string, double>();
            this.additionalData = new Dictionary<string, double>();
        }

        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight, Dictionary<string, double> grades)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.grades = grades;
            this.additionalData = new Dictionary<string, double>();
        }

        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight, Dictionary<string, double> grades, Dictionary<string, double> additionalData)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.grades = grades;
            this.additionalData = additionalData;
        }

        public void SetGrade(string key, double value)
        {
            grades[key] = value;
        }

        public void SetAdditionalData(string key, double value)
        {
            additionalData[key] = value;
        }

        public int GetId()
        {
            return id;
        }

        public int[] GetCoordinates()
        {
            return new [] { xCoordinate, yCoordinate, zCoordinate };
        }
    }
}
