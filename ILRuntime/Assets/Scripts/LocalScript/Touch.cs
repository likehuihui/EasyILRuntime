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
    public class Touch : MonoBehaviour
    {
        public RectTransform rectTViewport;
        public RectTransform rectTJoy;//将获取坐标作为摇杆键值
        public int r;
        void Start()
        {
            r = (int)rectTViewport.sizeDelta.x / 2;
        }
        public void OnMove(RectTransform rect)
        {
            if (rect.anchoredPosition.magnitude > r)
            {//将摇杆限制在 半径 r_ 以内
                rect.anchoredPosition = rect.anchoredPosition.normalized * r;
            }
        }
    }
}
