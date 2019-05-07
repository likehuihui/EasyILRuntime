/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using K.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileTools
{
    public static string netUrl = "file://D:/ServerResource";
    /// <summary>
    /// 结构体写入本地
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    public static void CreateFile<T>(string path, string name, T info)
    {
        string msg = LitJson.JsonMapper.ToJson(info);
        CreateFile(path, name, msg);
        //LitJson.JsonMapper.
    }
    /// <summary>
    /// 文本写入本地
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    public static void CreateFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        else
        {
            if (FileExist(path))
                File.Delete(path + "//" + name);
        }
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
            // t.Delete();
        }

        //以行的形式写入信息
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }
    /// <summary>
    /// 删除文件
    /// </summary>
    public static void Delete(string path, string name)
    {
        FileInfo t = new FileInfo(path + "//" + name);
        if (t.Exists)
            File.Delete(path + "//" + name);
    }
    /// <summary>
    /// 删除文件夹
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteFolder(string path)
    {
        // FileInfo t = new FileInfo(path );
        if (Directory.Exists(path))
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            dir.Delete(true);
        }

    }
    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool FileExist(string path)
    {
        FileInfo t = new FileInfo(path);
        if (!t.Exists)
        {
            return false;
        }
        else
        {
            return true;
            // t.Delete();
        }
    }
    /// <summary>
    /// 读取文本
    /// </summary>
    /// <param name="url">地址</param>
    /// <returns></returns>
    public static T ReadText<T>(string url)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(url);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            return default(T);
        }
        string line = sr.ReadLine();

        // T config = JsonFx.Json.JsonReader.Deserialize<T>(line);
        T config = LitJson.JsonMapper.ToObject<T>(line);
        sr.Close();
        sr.Dispose();
        return config;
    }
    /// <summary>
    /// 读取图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Texture2D ReadTexture(string path, string name, int width = 132, int height = 132)
    {

        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream(path + "//" + name, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            texture.LoadImage(bytes);
            return texture;

        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            return null;
        }

    }

    /// <summary>
    /// 资源写入本地
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    /// <param name="length"></param>
    public static bool CreateModelFile(string path, byte[] info)
    {
        //文件流信息
        //StreamWriter sw;
        try
        {
            Stream sw;
            FileInfo t = new FileInfo(path);
            string[] p = path.Split('/');
            string dir = path.Replace(p[p.Length - 1], "");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.Create();
            }
            else
            {
                //如果此文件存在则打开
                // sw = t.AppendText();
                return false;
            }
            //以行的形式写入信息
            sw.Write(info, 0, info.Length);
            sw.Close();
            sw.Dispose();
            return true;
        }
        catch (Exception)
        {

            return false;
        }


    }
    /// <summary>
    /// 返回最后一个斜杠后的字符
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string ReturnNmae(string url)
    {
        string[] str = url.Split('/');
        return str[str.Length - 1];
    }
    /// <summary>
    /// 得到本地dll目录
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static string GetLocalDLLPath(string dic = "")
    {
#if UNITY_EDITOR
        DirectoryInfo temp = Directory.GetParent(Application.dataPath);
        //string dir = FileSystem.CombineDirs(true, temp.FullName, dic);
        string dir = string.Format("{0}/Res/{1}/dll/{2}", temp.FullName, GetPlatform, dic);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        return dir;
#else
        return string.Format("{0}/res/{1}/{2}", Application.streamingAssetsPath, GetPlatform, dic);
#endif 
    }
    public static string GetLocalPath(string name)
    {
        //#if UNITY_EDITOR
        DirectoryInfo temp = Directory.GetParent(Application.dataPath);
        string dir = string.Format("{0}/Res/{1}", temp.FullName, GetPlatform);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        dir += "/" + name;
        return dir;
        //#else
        //#endif
    }
    /// <summary>
    /// dll网络地址
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static string GetDllNetPath(string endurl)
    {
        return string.Format("{0}/{1}/{2}/{3}", netUrl, GetPlatform, "dll", endurl);
    }
    public static string GetNetPath(string endurl)
    {
        return string.Format("{0}/{1}/{2}", netUrl, GetPlatform, endurl);
    }
    public static string GetPlatform
    {
        get
        {
#if UNITY_IOS
        return "ios";
#elif UNITY_ANDROID
        return "android";
#else
            return "pc";
#endif
        }

    }
}
