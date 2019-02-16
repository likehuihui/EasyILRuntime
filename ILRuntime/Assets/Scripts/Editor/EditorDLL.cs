/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace K.Editors
{
    public class EditorDLL : EditorBase
    {
        public class DllConfig
        {
            /// <summary>
            /// 编译工具
            /// </summary>
            public string devenvPath;

            /// <summary>
            /// 开发目录
            /// </summary>
            public string ilDevelopDir;

            /// <summary>
            /// 项目目录
            /// </summary>
            public string ilProjDir;

            /// <summary>
            /// 项目csproj路径
            /// </summary>
            public string ilProjPath;

            /// <summary>
            /// 是否在发布DLL的时候自动拷贝代码
            /// </summary>
            public bool isAudoCopy;
        }
        public DllConfig config;
        public string configName;
        public static void Open()
        {
            EditorDLL ab = EditorWindow.GetWindow<EditorDLL>("编辑窗口", true);
            ab.minSize = new Vector2(640, 360);
            ab.maxSize = new Vector2(1280, 720);
            ab.Show();
        }
        private void OnEnable()
        {
           config = LoadConfig<DllConfig>(configName);
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存配置"))
            {
                //  SaveConfig();
            }
            if (GUILayout.Button("拷贝代码"))
            {
                CopyDll();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        private void CopyDll()
        {
            string projCodeDir = Path.Combine("../ILScript/ILScript", "codes");
            if (Directory.Exists("./Assets/Scripts/RunTimeFrame/"))
            {
                if (Directory.Exists(projCodeDir))
                {
                    Directory.Delete(projCodeDir, true);
                }
                FileUtil.CopyFileOrDirectory("./Assets/Scripts/RunTimeFrame/", projCodeDir);
                AssetDatabase.Refresh();
            }
            else
            {
                ShowNotification(new GUIContent("文件夹不存在"));
            }
        }
    }
}