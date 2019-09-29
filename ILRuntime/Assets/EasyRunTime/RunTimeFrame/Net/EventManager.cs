/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections.Generic;
using UnityEngine;
namespace RunTimeFrame
{
    public delegate void ProtocolHandler(string data);   //定义一个处理消息的委托
    public class EventManager : MonoBehaviour
    {

        public static EventManager instance;  //定义一个EventManager类型变量，命名为instance
        public Dictionary<string, ProtocolHandler> handlers = new Dictionary<string, ProtocolHandler>();  //定义并实例化一个字典，键为字符串类型，值为ProtocolHandler类型，命名为handlers
        public Dictionary<string, string> eventDatas = new Dictionary<string, string>();  //定义并实例化一个字典，键与值都为string类型
                                                                                          // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);   //调用DontDestroyOnLoad方法，将自己的游戏对象传入，使游戏对象在切换场景时不会被删除
            instance = this;  //将自己赋值给instance
           // ReceivesMessage rec = new ReceivesMessage();   //实例化ReceivesMessage类
        }


        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public void AddEventData(string eventName, string data)
        {

            eventDatas.Add(eventName, data);  //将消息添加到EventManager的eventDatas变量中，传入
        }


        /// <summary>
        /// 追加事件
        /// </summary>
        public void additionalEvent(string p, ProtocolHandler ph)
        {
            foreach (string key in handlers.Keys)  //循环遍历handlers的key
            {
                if (key == p)   //如果key与传入的p相同
                {
                    handlers[key] += new ProtocolHandler(ph);  //将传入的事件累加到该key的handlers中
                    return;
                }
            }
        }




        void Update()
        {
            foreach (string key in eventDatas.Keys)  //循环事件
            {
                ProtocolHandler protocolHandler = null;   //定义一个ProtocolHandler类型的变量protocolHandler
                if (handlers.TryGetValue(key, out protocolHandler))  //尝试获取handlers中key所在的值，如果有就赋值给protocolHandler
                {
                    protocolHandler(eventDatas[key]);   //将事件数据中的key数据读取出来
                    eventDatas.Remove(key);             //删除该列数据
                    return;
                }
            }
        }



        //添加事件
        public void AddEvent(string name, ProtocolHandler ph)
        {
            if (!handlers.ContainsKey(name))  //如果handlers中不含有与name相同的Key
                handlers.Add(name, ph);         //将该事件添加到其中
        }
        //执行事件
        public void ExecutionEvent(string name, string data)
        {
            ProtocolHandler ph;  //定义一个ProtocolHandler类型的变量protocolHandler
            if (handlers.TryGetValue(name, out ph)) //尝试获取handlers中key为name的值
                ph(data);   //将data传入，执行该委托
            else
                Debug.Log("事件不存在");  //否则提示不存在该事件
        }


    }

}
