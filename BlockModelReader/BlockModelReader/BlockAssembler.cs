using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    public static class BlockAssembler
    {
        private static BlockModel blockModel;

        public static void SetBlocks(List<IReblockable> blocks)
        {
            blockModel = new BlockModel();
            blockModel.SetBlocks(blocks);
        }
    }
}
