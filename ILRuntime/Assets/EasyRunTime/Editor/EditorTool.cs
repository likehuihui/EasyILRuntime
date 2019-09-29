/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using K.Editors;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace K.Editors
{
    public class EditorTool
    {
        private static string dirName = "Res";
        /// <summary>
        /// 编辑器生成的配置文件保存目录
        /// </summary>
        /// <returns></returns>
        static string ConfigDir
        {
            get
            {
                DirectoryInfo temp = Directory.GetParent(Application.dataPath);
                string dir = FileSystem.CombineDirs(true, temp.FullName, "EditorConfig", EditorWithWindow.PlatformDirName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }
    }
}
