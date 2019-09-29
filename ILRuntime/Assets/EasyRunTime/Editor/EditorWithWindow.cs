/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace K.Editors
{
    public class EditorWithWindow : EditorWindow
    {

        [MenuItem("工具/打开编辑窗口", false, 0)]
        public static void AssetBundle()
        {
            EditorMainWindow.Open();

        }
        [MenuItem("工具/打开DLL窗口", false, 0)]
        public static void EditorDll()
        {
            EditorDLL.Open();
        }
        public static string PlatformDirName
        {
            get
            {
                string name;

#if UNITY_STANDALONE
                name = "pc/";
#elif UNITY_IPHONE
        name = "ios/";
#elif UNITY_ANDROID
                name = "android/";
#endif
                return name;
            }
        }
    }
}