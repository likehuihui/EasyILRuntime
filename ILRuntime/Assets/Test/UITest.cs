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
namespace RunTimeFrame.UI
{
    public class UITest : MonoBehaviour
    {
        public Text text;
        AndroidJavaObject abj;
        private void Start()
        {
            abj = new AndroidJavaObject("com.jing.unity.Unity2Android");
            //  Animator anim = GetComponent<Animator>();
            //   float le = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //Transform root = GameObject.Find("MainPanel").transform;
            //UIManager.Instance.Init(root, root.Find("Normal"), root.Find("Mask"), root.Find("Fixed"), root.Find("Pop"));
            //UIManager.Instance.ShowUIForm("UITest");
            //  StartCoroutine(Test());
        }
        /// <summary>
        /// 场景上按点击时触发该方法
        /// </summary>
        public void OnBtnClick()
        {
            Debug.Log("");
            text.text = SystemInfo.deviceType.ToString() + "---" + SystemInfo.processorType;
            //通过API来调用原生代码的方法
            bool success = abj.Call<bool>("showToast", "this is unity");
            if (true == success)
            {
                //请求成功
            }
        }

        /// <summary>
        /// 原生层通过该方法传回信息
        /// </summary>
        /// <param name="content"></param>
        public void FromAndroid(string content)
        {
            
         //   text.text = SystemInfo.deviceType.ToString() +"---"+ SystemInfo.processorType;
          //  SystemInfo.operatingSystemFamily
        }
        //private IEnumerator Test()
        //{
        //    int a = 1;
        //    int b = 2;
        //    int c = a + b;
        //    Debug.Log(c);
        //    yield return 0;
        //}
    }
}