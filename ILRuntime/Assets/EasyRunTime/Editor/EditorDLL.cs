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
            if (GUILayout.Button("生成DLL"))
            {
                CreatDll();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        private void CreatDll()
        {
            string[] scriptPaths = Directory.GetFiles(config.ilDevelopDir, "*.cs", SearchOption.AllDirectories);
            AssemblyBuilder ab = new AssemblyBuilder(config.outPath, scriptPaths);
            ab.additionalReferences = GetDepends();
            ab.buildFinished += OnFinished;
            if (false == ab.Build())
            {
              //  onFinished?.Invoke(this, false);
              //  onFinished = null;
            }
            //string srcPath = Path.Combine(Application.dataPath, "../RunTimeFrame/RunTimeFrame/codes");
            //string[] sources = new[] { Path.Combine(srcPath, "Test.cs") };
            //string[] references = new string[0];
            //string[] defines = new string[0];
            //string outputFile = Path.Combine(Application.dataPath, "RunTimeFrame.dll");

            //string[] msgs = EditorUtility.CompileCSharp(sources, references, defines, outputFile);
            //foreach (var msg in msgs)
            //{
            //    Debug.Log(msg);
            //}

            //string dllFile = "Assets/RunTimeFrame.dll";
            //if (File.Exists(dllFile))
            //{
            //    AssetDatabase.ImportAsset(dllFile, ImportAssetOptions.ForceUpdate);
            //}
        }
        private void OnFinished(string path, CompilerMessage[] msgs)
        {
            bool isFail = false;
            foreach (var msg in msgs)
            {
                if (msg.type == CompilerMessageType.Error)
                {
                    Debug.LogError(msg.message);
                    isFail = true;
                }
            }

            if (isFail)
            {
              //  onFinished?.Invoke(this, false);
            }
            else
            {
              //  onFinished?.Invoke(this, true);
            }

          //  onFinished = null;
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

        private void CopyDll()
        {
            string projCodeDir = Path.Combine(config.ilProjDir, "codes");
            // EditorUtility.CompileCSharp()
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