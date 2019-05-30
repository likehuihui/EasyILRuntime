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
    public class FPS : MonoBehaviour
    {
        private const float updateTime = 1f;
        private int frames = 0;
        private float time = 0f;
        private string fps = "";
        int _avgFps = 0;
        GUIStyle fontStyle=new GUIStyle();
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
        void Update()
        {
            time += Time.deltaTime;
            if (time >= updateTime)
            {
                fps = "FPS: " + (frames / time).ToString("f2");
                time = 0f;
                _avgFps = frames;
                frames = 0;
               
            }
            frames++;
            if (_avgFps < 30)
                fontStyle.normal.textColor = Color.red;
            else
                fontStyle.normal.textColor = Color.green;
        }

        // 
        void OnGUI()
        {

            GUI.Label(new Rect(0, 0, 100, 20), fps, fontStyle);
        }
    }
}
