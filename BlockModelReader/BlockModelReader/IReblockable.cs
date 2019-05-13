using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    public interface IReblockable
    {
        int GetId();
        int[] GetCoordinates();
        double GetWeight();
        Dictionary<string, double> GetGrades();
    }
}
