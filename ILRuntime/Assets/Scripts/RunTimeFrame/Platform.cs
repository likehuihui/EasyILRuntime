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
            bool useLocalRes = K.LocalWork.Main.Instance.runtimeConfig.isUseLocalResourse;
            ResSetting res= FileTools.ReadText<ResSetting>(FileTools.GetLocalPath("ResVersion.json"));
            ResourcesCentre.Instance.Init(useLocalRes, FileTools.GetLocalPath("ab/")+ res.manifestName);
            UpdateAB uab = new UpdateAB(FileTools.GetLocalPath("ab"), res);
            uab.Start();
            //   Preload pre = new Preload();
            //  Button btn = GameObject.Find("Button").GetComponent<Button>();
            //     btn.onClick.AddListener(OnClick);
        }
        public static void OnClick()
        {


        //    Debug.Log("点击1");
            //SceneManager.LoadScene("GameScene");
            //Application.LoadLevel(1);
        }
    }
}