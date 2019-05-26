using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    public class Block: IReblockable
    {
        private int id, xCoordinate, yCoordinate, zCoordinate;
        private double weight;
        private Dictionary<string, double> grades;
        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.weight = weight;
            grades = new Dictionary<string, double>();
        }

        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight, Dictionary<string, double> grades)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.weight = weight;
            this.grades = grades;
        }

        public void SetGrade(string key, double value)
        {
            grades[key] = value;
        }

        public int GetId()
        {
            return id;
        }

        public int[] GetCoordinates()
        {
            return new [] { xCoordinate, yCoordinate, zCoordinate };
        }

        public double GetWeight()
        {
            return weight;
        }

        public Dictionary<string, double> GetGrades()
        {
            return grades;
        }
        public override bool Equals(object obj)
        {
            IReblockable other = obj as IReblockable;
            if (xCoordinate != other.GetCoordinates()[0])
            {
                return false;
            }
            if (yCoordinate != other.GetCoordinates()[1])
            {
                return false;
            }
            if (zCoordinate != other.GetCoordinates()[2])
            {
                return false;
            }
            if (weight != other.GetWeight())
            {
                return false;
            }
            Dictionary<string, double> otherGrades = other.GetGrades();
            foreach(string key in grades.Keys)
            {
                if (grades[key] != otherGrades[key])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
