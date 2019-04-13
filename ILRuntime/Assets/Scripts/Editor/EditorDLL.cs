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
        [System.Serializable]
        internal class DllConfig
        {
            /// <summary>
            /// 编译工具
            /// </summary>
            public string devenvPath= "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community\\Common7\\IDE\\devenv";

            /// <summary>
            /// 开发目录
            /// </summary>
            public string ilDevelopDir=Application.dataPath+ "/Scripts/RunTimeFrame";

            /// <summary>
            /// 项目目录
            /// </summary>
            public string ilProjDir= Directory.GetParent(Application.dataPath).FullName+"/RunTimeFrame/RunTimeFrame";

            /// <summary>
            /// 项目csproj路径
            /// </summary>
            public string ilProjPath="dic";

            /// <summary>
            /// 是否在发布DLL的时候自动拷贝代码
            /// </summary>
            public bool isAudoCopy=false;
        }
        [SerializeField]
        internal DllConfig config;
        public string configName = "DllEditor.json";
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
            if (config == null)
            {
                config = new DllConfig();
            }
                
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存配置"))
            {
                SaveConfig(config, configName);
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
            string projCodeDir = Path.Combine(config.ilProjDir, "codes");
            if (Directory.Exists(config.ilDevelopDir))
            {
                if (Directory.Exists(projCodeDir))
                {
                    Directory.Delete(projCodeDir, true);
                }
                FileUtil.CopyFileOrDirectory(config.ilDevelopDir, projCodeDir);
                AssetDatabase.Refresh();
            }
            else
            {
                ShowNotification(new GUIContent("文件夹不存在"));
            }
            Debug.Log("拷贝完成");
        }
    }
}