/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace RunTimeFrame
{
   public class CallScript:MonoBehaviour  
   {
        public void Start()
        {
            Type type = Type.GetType("RunTimeFrame.TypeTest");
            object obj = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("Call1");
            method.Invoke(obj, null);
        }


    }
}