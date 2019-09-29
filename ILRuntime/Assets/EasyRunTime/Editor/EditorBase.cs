using LitJson;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace K.Editors
{
    public class EditorBase : EditorWindow
    {
        private static string dirName = "Res";
        /// <summary>
        /// 编辑器生成的配置文件保存目录
        /// </summary>
        /// <returns></returns>
        public string ConfigDir
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

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="data">配置的数据</param>
        /// <param name="fileName">文件名</param>
        internal void SaveConfig(object data, string fileName)
        {
            string path = Path.Combine(ConfigDir, fileName);
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Create(path);
            //bf.Serialize(file, data);
            //file.Close();
            string json = JsonMapper.ToJson(data);
            string url = Path.Combine(ConfigDir, fileName);
            File.WriteAllText(url, json);
            Debug.Log("保存成功!");
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">配置文件名称</param>
        /// <returns></returns>
        internal T LoadConfig<T>(string fileName)
        {
            string path = Path.Combine(ConfigDir, fileName);
            if (File.Exists(path))
            {
                //BinaryFormatter bf = new BinaryFormatter();
                //FileStream file = File.Open(path, FileMode.Open);
                //object data = bf.Deserialize(file) ;       
                //file.Close();
                //return (T)data;
                string json = File.ReadAllText(path);
                return JsonMapper.ToObject<T>(json);
            }
            return default(T);
        }
    }
}
