using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHTB
{
    /// <summary>
    /// 客户端向服务器发消息的助手
    /// </summary>
    public class C2SHelper
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        public static C2SHelper ins = new C2SHelper();

        /// <summary>
        /// 正在请求换子弹
        /// </summary>
        public bool isRequestingChangeBullet = false;

        private C2SHelper()
        {

        }

        /// <summary>
        /// 心跳协议
        /// </summary>
        /// <param name="sign"></param>
        public void HeartBeat(ushort sign = 0)
        {
            var cmd = new CMD_CS_HeartBeat();
            cmd.Sign = sign;
            SHTB_.SHTBInstance.HeartBeat(cmd);
        }

        /// <summary>
        /// 切换子弹
        /// </summary>
        public void ChangeBullet(ushort bulletType)
        {
            if (isRequestingChangeBullet)
            {
                return;
            }
            var cmd = new CMD_C_ChangeBullet();
            cmd.BulletType = bulletType;
            Debug.Log(string.Format("发送切换子弹协议 BulletType:{0} ", bulletType));
            SHTB_.SHTBInstance.ChangeBullet(cmd);
            isRequestingChangeBullet = true;
        }

        /// <summary>
        /// 开火
        /// </summary>
        public void Fire(ushort bulletId, short angle, ushort canonType, ushort lockFishId)
        {
            //Debug.LogFormat("发射子弹：{0}", bulletId);
            var cmd = new CMD_C_RequestFire();
            cmd.BulletID = bulletId;
            cmd.Angle = angle;
            cmd.CanonType = canonType;
            cmd.LockFishID = lockFishId;
            //Debug.Log(string.Format("发送开火协议 bulletId:{0} angle:{1} canonType:{2} lockFishId:{3}", bulletId, angle, canonType, lockFishId));
            SHTB_.SHTBInstance.RequestFire(cmd);
        }

        /// <summary>
        /// 最后锁定的鱼的ID，如果为0，则表示没有锁定
        /// </summary>
        private ushort _lastLockFishId = 0;

        /// <summary>
        /// 锁定目标
        /// </summary>
        /// <param name="action">0为解锁   1为锁定</param>
        /// <param name="lockFishId">锁定鱼ID</param>
        public void Lock(ushort action, ushort lockFishId)
        {
            if (0 == action && 0 == _lastLockFishId)
            {
                //本来已是解锁状态
                return;
            }
            else if (1 == action && lockFishId == _lastLockFishId)
            {
                //锁定同一个鱼不再重复发
                return;
            }

            _lastLockFishId = lockFishId;
            var cmd = new CMD_C_RequestLock();
            cmd.Action = action;
            cmd.LockFishID = lockFishId;
            //Debug.Log(string.Format("发送锁定鱼协议 Action:{0} LockFishID:{1}", action, lockFishId));
            SHTB_.SHTBInstance.RequestLock(cmd);
        }
        /// <summary>
        /// 请求自动
        /// </summary>
        /// <param name="state"></param>
        public void Auto(ushort state,ushort chairID)
        {
            CMD_ChangeAutoState s = new CMD_ChangeAutoState();
            s.State = state;
            s.ChairID = chairID;
            SHTB_.SHTBInstance.RequestAuto(s);
        }
        /// <summary>
        /// 开启特殊武器  3凝霜  4穿云
        /// </summary>
        public void OpenSpecialCanon(ushort specialCanonType)
        {
            var cmd = new CMD_C_RequestTurnOnSpecialCanon();
            cmd.SpecialCanonType = specialCanonType;
            Debug.Log("请求开启特殊炮台：" + specialCanonType);
            SHTB_.SHTBInstance.OpenSpecialCanon(cmd);
        }

        /// <summary>
        /// 发射激光炮
        /// </summary>
        /// <param name="angle"></param>
        public void RequestLaser(short angle)
        {
            var cmd = new CMD_C_RequestLaser();
            cmd.Degree = angle;
            //Debug.Log("--------------------发射激光炮:" + angle);
            SHTB_.SHTBInstance.RequestLaser(cmd);
        }

        /// <summary>
        /// 请求鱼图鉴
        /// </summary>
        public void RequestShowFishData()
        {
            //SHTB_.SHTBInstance.RequestShowFishData();
        }

        /// <summary>
        /// 通知服务器客户端碰撞到鱼
        /// </summary>
        public void CollideFish(ushort bulletId, List<ushort> fishIds)
        {
            //Debug.LogFormat("子弹碰撞：{0}", bulletId);
            var cmd = new CMD_C_CollideByClientBullet();
            cmd.BulletID = bulletId;
            cmd.FishIDs = fishIds.ToArray();
            cmd.CollideNum = (ushort)cmd.FishIDs.Length;
            SHTB_.SHTBInstance.CollideFish(cmd);
        }
        /// <summary>
        /// 请求GM控制信息
        /// </summary>
        /// <param name="sign"></param>
        public void GetGmInfo(ushort sign)
        {
            CMD_GM_C_SendInfo gm = new CMD_GM_C_SendInfo();
            gm.sign = sign;
            SHTB_.SHTBInstance.RequestGmInfo(gm);

        }
        /// <summary>
        /// 请求保存GM信息
        /// </summary>
        /// <param name="info"></param>
        public void SaveGMInfo(CMD_GM_C_SaveConfig info)
        {
            SHTB_.SHTBInstance.SaveGMInfo(info);
        }
        /// <summary>
        /// 请求,刷新库存
        /// </summary>
        /// <param name="sign"></param>
        public void RepertoryInfo(ushort sign)
        {
            CMD_GM_C_GetStock gm = new CMD_GM_C_GetStock();
            gm.Sign = sign;
            SHTB_.SHTBInstance.RepertoryInfo(gm);
        }
        /// <summary>
        /// 请求保存库存
        /// </summary>
        /// <param name="gm"></param>
        public void SaveRepertory(CMD_GM_C_SaveStock gm)
        {
            SHTB_.SHTBInstance.SaveRepertory(gm);
        }
        /// <summary>
        /// 请求清理库存
        /// </summary>
        /// <param name="sign"></param>
        public void CleanRepertory(ushort sign)
        {
            CMD_GM_C_ClearStock gm = new CMD_GM_C_ClearStock();
            gm.Sign = sign;
            SHTB_.SHTBInstance.CleanRepertory(gm);
        }
        /// <summary>
        /// 请求更新黑白名单
        /// </summary>
        /// <param name="ms"></param>
        public void UpdateBWList(byte Type, byte OperateIndex, uint GameID, float ControlPer)
        {
            CMD_GM_C_BlackWhite gm = new CMD_GM_C_BlackWhite();
            gm.Type = Type;
            gm.OperateIndex = OperateIndex;
            gm.GameID = GameID;
            gm.ControlPer = ControlPer;
            SHTB_.SHTBInstance.UpdateBWList(gm);
        }
    }
}
