using System;
using System.Collections.Generic;

namespace BlockModelReader
{
    public class Block: IReblockable
    {
        private const double TOLERANCE = 0.001;

        private readonly int id, xCoordinate, yCoordinate, zCoordinate;
        private readonly double weight;
        private readonly Dictionary<string, double> grades;

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
            if (!(obj is IReblockable other))
            {
                return false;
            }  
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
            if (Math.Abs(weight - other.GetWeight()) > TOLERANCE)
            {
                return false;
            }
            Dictionary<string, double> otherGrades = other.GetGrades();
            foreach(string key in grades.Keys)
            {
                if (Math.Abs(grades[key] - otherGrades[key]) > TOLERANCE)
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
