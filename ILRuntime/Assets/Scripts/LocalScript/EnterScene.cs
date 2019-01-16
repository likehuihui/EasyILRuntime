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
namespace K.LocalWork
{
    public class EnterScene : MonoBehaviour
    {
        private void Awake()
        {
            if (MessageCentre.enterGameScene != null)
                MessageCentre.enterGameScene("GameScene");
        }
    }
}


