namespace CYNetwork.NetStruct
{
    public struct WriteDataVO
    {
        public ushort mainCmd;
        public ushort subCmd;
        public byte[] data;
        public int dataSize;

        public WriteDataVO(ushort mainCmd, ushort subCmd, byte[] data, int dataSize)
        {
            this.mainCmd = mainCmd;
            this.subCmd = subCmd;
            this.data = data;
            this.dataSize = dataSize;            
        }
    }
}