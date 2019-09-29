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
    /// <summary>
    /// 公共变量
    /// </summary>
    public class Global
    {
        /// <summary>
        /// 资源目录
        /// </summary>
        public static string ResourcesPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.WindowsEditor:
                        return Application.streamingAssetsPath + "/";
                    case RuntimePlatform.WindowsPlayer:
                        return string.Format("file://{0}/StreamingAssets/", Application.dataPath);
                    case RuntimePlatform.IPhonePlayer:
                        return string.Format("file://{0}/Raw/", Application.dataPath);
                    case RuntimePlatform.Android:
                        return Application.streamingAssetsPath + "/";
                    default:
                        return Application.dataPath + "/StreamingAssets/";
                       
                }
            }
        }
    }
}