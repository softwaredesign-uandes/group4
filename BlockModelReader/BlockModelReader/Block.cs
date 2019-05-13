﻿using System;
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
        private Dictionary<string, double> additionalData;
        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.weight = weight;
            this.grades = new Dictionary<string, double>();
            //this.additionalData = new Dictionary<string, double>();
        }

        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight, Dictionary<string, double> grades)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.weight = weight;
            this.grades = grades;
            //this.additionalData = new Dictionary<string, double>();
        }

        public Block(int id, int xCoordinate, int yCoordinate, int zCoordinate, double weight, Dictionary<string, double> grades, Dictionary<string, double> additionalData)
        {
            this.id = id;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.zCoordinate = zCoordinate;
            this.weight = weight;
            this.grades = grades;
            //this.additionalData = additionalData;
        }

        public void SetGrade(string key, double value)
        {
            grades[key] = value;
        }

        /*public void SetAdditionalData(string key, double value)
        {
            additionalData[key] = value;
        }*/

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
            Block other = obj as Block;
            if (xCoordinate != other.xCoordinate)
            {
                return false;
            }
            if (yCoordinate != other.yCoordinate)
            {
                return false;
            }
            if (weight != other.weight)
            {
                return false;
            }
            foreach(string key in grades.Keys)
            {
                if (grades[key] != other.grades[key])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
