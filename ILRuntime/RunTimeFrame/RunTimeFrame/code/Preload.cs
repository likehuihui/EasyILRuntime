/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Middle;
namespace RunTimeFrame
{
    public class Preload 
    {
        public Preload()
        {
            MessageCentre.enterGameScene += Start;
        }
        public void Start(string sceneName)
        {
            Debug.Log("进入");
        }
    }
}