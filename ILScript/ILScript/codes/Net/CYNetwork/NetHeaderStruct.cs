//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
namespace CYNetwork
{

    struct CMD_Info
    {
        public byte cbVersion;							//版本标识
        public byte cbCheckCode;						//效验字段
        public ushort wPacketSize;						//数据大小
#if LINUX_SVR
        public ushort wLine1;							//
        public ushort wLine2;							//
        public uint dwUserID;							//用户 I D
#endif
    }
    //网络命令
    struct CMD_Command
    {
        public ushort wMainCmdID;							//主命令码
        public ushort wSubCmdID;							//子命令码
    }

    //网络包头
    struct CMD_Head
    {
        public CMD_Info CmdInfo;							//基础结构
        public CMD_Command CommandInfo;						//命令信息
    }

}
