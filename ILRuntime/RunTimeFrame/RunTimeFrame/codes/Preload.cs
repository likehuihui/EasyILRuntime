/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using K.LocalWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunTimeFrame
{
    public class Preload
    {
        ProgressBar pro;
        public Preload()
        {

        }
        public void Start(string sceneName)
        {
            pro = Object.FindObjectOfType(typeof(ProgressBar)) as ProgressBar;
            bool useLocalRes = Main.Instance.runtimeConfig.isUseLocalResourse;
            ResSetting res = FileTools.ReadText<ResSetting>(FileTools.GetLocalPath("", "ResVersion.json"));
            if (res == null)
                res = new ResSetting();
            UpdateAB uab = new UpdateAB(res, FileTools.GetNetPath(""));
            uab.progress += Progress;// pro.Progress;
            uab.updateEnd += UpdateEnd;
            uab.Start();
          
          
       

            Debug.Log("进入");
        }
        private void Progress(float p, string msg)
        {
            Debug.Log(msg);
            pro.Progress(p,msg);
        }
        private void UpdateEnd()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}