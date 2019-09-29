/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using K.LocalWork;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace RunTimeFrame
{
    public class Platform
    {
        public static void Main()
        {
            Preload pre = new Preload();
            pre.Start("GameScene");
        }

    }
}