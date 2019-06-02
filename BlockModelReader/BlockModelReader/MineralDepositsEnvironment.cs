using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;

namespace BlockModelReader
{
    public static class MineralDepositsEnvironment
    {
        private static List<MineralDeposit> mineralDeposits = new List<MineralDeposit>();
        private static int mineralDepositIdCount;

        public static void AddMineralDeposit()
        {
            mineralDeposits.Add(new MineralDeposit(mineralDepositIdCount));
            mineralDepositIdCount++;
        }

        public static MineralDeposit GetMineralDeposit(int id)
        {
            return mineralDeposits.First(i => i.GetId() == id);
        }
    }
}
