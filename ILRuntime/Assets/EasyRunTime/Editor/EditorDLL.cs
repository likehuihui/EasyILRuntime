/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
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
            public string devenvPath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community\\Common7\\IDE\\devenv";

            /// <summary>
            /// 开发目录
            /// </summary>
            public string ilDevelopDir = Application.dataPath + "/EasyRunTime/Test";

            public string outPath = Application.dataPath + "RunTimeFrame.dll";//"./Res/android/dll/RunTimeFrame/RunTimeFrame.dll";

            /// <summary>
            /// 项目目录
            /// </summary>
            public string ilProjDir = Directory.GetParent(Application.dataPath).FullName + "/RunTimeFrame/RunTimeFrame";

            /// <summary>
            /// 项目csproj路径
            /// </summary>
            public string ilProjPath = "dic";

            /// <summary>
            /// 是否在发布DLL的时候自动拷贝代码
            /// </summary>
            public bool isAudoCopy = false;
        }
        [SerializeField]
        DllPublishConfigModel _cfg;
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
            _cfg = new DllPublishConfigModel();

        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("保存配置"))
            {
                _cfg.Save();
            }
            //if (GUILayout.Button("拷贝代码"))
            //{
            //    CopyDll();
            //}
            if (GUILayout.Button("生成DLL"))
            {
                CreatDll();
            }
            _cfg.VO.resDir = EditorGUILayout.TextField("Res目录:", _cfg.VO.resDir);
            GUILayout.Space(10);

            _cfg.VO.ilDevelopDir = EditorGUILayout.TextField("DLL开发目录:", _cfg.VO.ilDevelopDir);
            EditorGUILayout.EndVertical();
        }
        private void CreatDll()
        {
            //if (_cfg.VO.isAudoCopy)
            //{
            //    Copy2DllProj();
            //}

            var cmd = new UnityDllPublishCommand();
            cmd.onFinished += OnFinished;
            //cmd.onComplete += OnPublishDllComplete;
            cmd.Execute();
        }
        private void OnFinished(UnityDllPublishCommand cmd, bool isSuccess)
        {
            if (isSuccess)
            {
                UnityEngine.Debug.Log("dll release success");
            }
            else
            {
                UnityEngine.Debug.Log("dll release fail");
            }
        }
        string[] GetDepends()
        {
            //依赖Assets下的DLL
            var assetDir = Application.dataPath;
            var dllList0 = Directory.GetFiles(assetDir, "*.dll", SearchOption.AllDirectories);
            //依赖Library/ScriptAssemblies下的DLL
            var projectDir = Directory.GetParent(assetDir).FullName;
            var dllList1 = Directory.GetFiles(FileSystem.CombineDirs(true, projectDir, "Library", "ScriptAssemblies"), "*.dll", SearchOption.AllDirectories);
            //排除对Editor代码的依赖
            for (int i = 0; i < dllList1.Length; i++)
            {
                if (dllList1[i].Contains("Assembly-CSharp-Editor"))
                {
                    dllList1[i] = "";
                    break;
                }
            }


            //依赖Unity安装目录下的DLL
            var dir = FileSystem.CombineDirs(true, EditorApplication.applicationContentsPath, "Managed");
            var dllList2 = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories);

            string[] depends = new string[dllList0.Length + dllList1.Length + dllList2.Length];
            Array.Copy(dllList0, 0, depends, 0, dllList0.Length);
            Array.Copy(dllList1, 0, depends, dllList0.Length, dllList1.Length);
            Array.Copy(dllList2, 0, depends, dllList0.Length + dllList1.Length, dllList2.Length);
            return depends;
        }
    }
}