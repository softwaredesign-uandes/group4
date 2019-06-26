using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;

namespace BlockModelReader
{
    public static class MineralDepositsEnvironment
    {
        private static List<MineralDeposit> mineralDeposits = new List<MineralDeposit>();
        private static List<BlockModel> blockModels = new List<BlockModel>();

        private static void SerializeMineralDeposits()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("MineralDeposits.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, mineralDeposits);
            stream.Close();
        }

        private static void SerializeBlockModels()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("BlockModels.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, blockModels);
            stream.Close();
        }

        public static void SerializePersistentData()
        {
            SerializeMineralDeposits();
            SerializeBlockModels();
            Block.SerializeIdCount();
            BlockModel.SerializeIdCount();
            MineralDeposit.SerializeIdCount();
        }

        public static void InitializeEnvironment()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream;
            if (File.Exists("MineralDeposits.dat"))
            {
                stream = new FileStream("MineralDeposits.dat", FileMode.Open, FileAccess.Read);
                mineralDeposits = (List<MineralDeposit>)formatter.Deserialize(stream);
                stream.Close();
            }
            if (File.Exists("BlockModels.dat"))
            {
                stream = new FileStream("BlockModels.dat", FileMode.Open, FileAccess.Read);
                blockModels = (List<BlockModel>)formatter.Deserialize(stream);
                stream.Close();
            }
            if (File.Exists("MineralDepositIdCount.dat"))
            {
                stream = new FileStream("MineralDepositIdCount.dat", FileMode.Open, FileAccess.Read);
                MineralDeposit.SetIdCount((int)formatter.Deserialize(stream));
                stream.Close();
            }
            if (File.Exists("BlockModelCurrentIdCount.dat"))
            {
                stream = new FileStream("BlockModelCurrentIdCount.dat", FileMode.Open, FileAccess.Read);
                BlockModel.SetIdCount((int)formatter.Deserialize(stream));
                stream.Close();
            }
            if (File.Exists("BlockCurrentIdCount.dat"))
            {
                stream = new FileStream("BlockCurrentIdCount.dat", FileMode.Open, FileAccess.Read);
                Block.BlockIdCount = (int)formatter.Deserialize(stream);
                stream.Close();
            }
        }

        public static void AddMineralDeposit(string name)
        {
            MineralDeposit mineralDeposit = GetMineralDeposit(name);
            if (mineralDeposit == null)
            {
                mineralDeposits.Add(new MineralDeposit(name));
            }
        }

        public static void AddBlockModel(BlockModel blockModel)
        {
            blockModels.Add(blockModel);
        }

        public static BlockModel GetBlockModel(int id)
        {
            return blockModels.First(i => i.GetId() == id);
        }
        public static List<BlockModel> GetBlockModels()
        {
            return blockModels;
        }

        public static MineralDeposit GetMineralDeposit(int id)
        {
            return mineralDeposits.First(i => i.GetId() == id);
        }

        public static MineralDeposit GetMineralDeposit(string name)
        {
            try
            {
                return mineralDeposits.First(i => i.GetName() == name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<MineralDeposit> GetMineralDeposits()
        {
            return mineralDeposits;
        }
    }
}
