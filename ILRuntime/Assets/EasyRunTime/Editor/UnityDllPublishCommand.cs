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
    public class UnityDllPublishCommand
    {
        /// <summary>
        /// 打包完成，返回一个bool表示成功还是失败
        /// </summary>
        public event Action<UnityDllPublishCommand, bool> onFinished;

        string _sourcesDir;

        string _resDir;

        public string OutputAssemblyPath;

        public UnityDllPublishCommand()
        {
            var cfg = new DllPublishConfigModel();

            _sourcesDir = cfg.VO.ilDevelopDir;
            _resDir = FileSystem.CombineDirs(true, cfg.VO.resDir, PathUtil.PlatformDirName, "dll");
            if (false == Directory.Exists(_resDir))
            {
                Directory.CreateDirectory(_resDir);
            }
            OutputAssemblyPath = FileSystem.CombinePaths(_resDir, "ILProject.dll");
        }

        public void Execute()
        {
            var scriptPaths = Directory.GetFiles(_sourcesDir, "*.cs", SearchOption.AllDirectories);
            var ab = new AssemblyBuilder(OutputAssemblyPath, scriptPaths);
            ab.compilerOptions = new ScriptCompilerOptions();
            ab.additionalReferences = GetDepends();
            ab.buildFinished += OnFinished;
            if (false == ab.Build())
            {
                if (onFinished != null)
                    onFinished(this, false);
               // onFinished?.Invoke(this, false);
                onFinished = null;
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
                if (onFinished != null)
                    onFinished(this, false);
               // onFinished?.Invoke(this, false);
            }
            else
            {
                if (onFinished != null)
                    onFinished(this, true);
               // onFinished?.Invoke(this, true);
            }

            onFinished = null;
        }

    }
}
