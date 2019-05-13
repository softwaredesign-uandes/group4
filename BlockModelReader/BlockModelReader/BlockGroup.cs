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
            Dictionary<string, double> mineralWeights = new Dictionary<string, double>();
            foreach (IReblockable block in blocks)
            {
                Dictionary<string, double> grades = block.GetGrades();
                grades = grades.Keys.ToList().Select(grade =>
                {
                    double gradeWeight = grades[grade] * block.GetWeight();
                    if (!mineralWeights.ContainsKey(grade))
                    {
                        mineralWeights.Add(grade, gradeWeight);
                    }
                    else
                    {
                        mineralWeights[grade] += gradeWeight;
                    }
                    return new KeyGradePair(grade, gradeWeight);
                }).ToDictionary(KeyGradePair => KeyGradePair.GetKey(), KeyGradePair => KeyGradePair.GetValue());
            }

            Dictionary<string, double> newGrades = new Dictionary<string, double>();
            newGrades = mineralWeights.Keys.ToList().Select(key =>
            {
                double gradeValue = mineralWeights[key] / GetWeight();
                gradeValue = Math.Round(gradeValue, 6);
                return new KeyGradePair(key, gradeValue);
            }).ToDictionary(keyGradePair => keyGradePair.GetKey(), keyGradePair => keyGradePair.GetValue());
            return newGrades;
        }

        public int GetId()
        {
            return id;
        }

        public double GetWeight()
        {
            double totalWeight = blocks.Select(block => block.GetWeight()).Sum();
            return totalWeight;
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
            if (GetWeight() != other.GetWeight())
            {
                return false;
            }
            Dictionary<string, double> otherGrades = other.GetGrades();
            Dictionary<string, double> grades = GetGrades();
            foreach (string key in grades.Keys)
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
