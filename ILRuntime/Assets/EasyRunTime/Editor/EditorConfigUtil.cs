﻿/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace K.Editors
{
    public class EditorConfigUtil
    {
        /// <summary>
        /// 编辑器生成的配置文件保存目录
        /// </summary>
        /// <returns></returns>
        static string ConfigDir
        {
            get
            {
                DirectoryInfo temp = Directory.GetParent(Application.dataPath);
                string dir = FileSystem.CombineDirs(true, temp.FullName, "EditorConfig", PlatformDirName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }
        public static   string PlatformDirName
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
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="data">配置的数据</param>
        /// <param name="fileName">文件名</param>
        public static void SaveConfig(object data, string fileName)
        {
            string json = JsonMapper.ToJson(data);
            File.WriteAllText(Path.Combine(ConfigDir, fileName), json);
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">配置文件名称</param>
        /// <returns></returns>
        public static T LoadConfig<T>(string fileName)
        {
            string path = Path.Combine(ConfigDir, fileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonMapper.ToObject<T>(json);
            }
            return default(T);
        }
    }
}
