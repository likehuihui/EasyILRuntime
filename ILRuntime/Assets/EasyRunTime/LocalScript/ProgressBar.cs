/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using K.LocalWork;
namespace K.LocalWork
{
    public class ProgressBar : MonoBehaviour
    {
        public Slider slider;
        public Text text;
        public void Start()
        {
            UpdateDll main = GameObject.Find("Main").GetComponent<UpdateDll>();
            slider.value = 0;
            main.progress += Progress;
        }
        public void Progress(float progress, string msg)
        {
            slider.value =progress;
            text.text = string.Format("{0}({1})", msg, progress * 100); //msg;
        }
    }
}
