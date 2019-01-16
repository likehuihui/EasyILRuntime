/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
namespace RunTimeFrame
{
    public class TestIOCP :MonoBehaviour
    {
        public void Start()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint p = new IPEndPoint(ip,8885);
            IOCPClient iocp = new IOCPClient(p, p);
            iocp.Connect();
        }
       
    }
}