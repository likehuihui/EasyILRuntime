/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace K.LocalWork
{

    public class ResSetting
    {
        public ResSetting()
        {
            items = new List<ABItem>();
        }
        public string manifestName;
        public List<ABItem> items;
    }
    public class ABItem
    {
        public string name;
        public string version;
        public long size;
    }
    public class Setting
    {

    }
}