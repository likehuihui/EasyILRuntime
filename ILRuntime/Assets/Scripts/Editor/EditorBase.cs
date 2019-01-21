/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
namespace K.Editors
{
    public class EditorBase : EditorWindow
    {
        public static string ConfigPath
        {
            get
            {
                DirectoryInfo temp = Directory.GetParent(Application.dataPath);
                string dir= string.Format("{0}/{1}", temp,"EditorSetting");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">配置文件名称</param>
        /// <returns></returns>
        public static T LoadConfig<T>(string fileName)
        {
            string path = Path.Combine(ConfigPath, fileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonMapper.ToObject<T>(json);
            }
            return default(T);
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="data">配置的数据</param>
        /// <param name="fileName">文件名</param>
        public static void SaveConfig(object data, string fileName)
        {
            string json = JsonMapper.ToJson(data);
            File.WriteAllText(Path.Combine(ConfigPath, fileName), json);
        }
    }
}