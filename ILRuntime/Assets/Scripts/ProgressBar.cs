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

public class ProgressBar:MonoBehaviour
{
    private Slider slider;
    private Text info;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        info = transform.Find("Info").GetComponent<Text>();
        info.text = "";
    }
    public void SetValue(float value, string txt = "")
    {
        // int v = (int)(value * 100);
        slider.value = value;
        info.text = txt;
    }
}
