using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class FindReferences
{

    [MenuItem("工具/清除选中文(文件夹)下没有使用的文件", false, 10)]
    static private void Find()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(path))
        {
            List<string> guidArr = new List<string>();
            if (Directory.Exists(path))
            {
                guidArr.AddRange(AllGuid(path));
            }
            else
            {

                guidArr.Add(path);
            }
            List<string> withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };



            string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories).Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
            int startIndex = 0;
            List<string> all = new List<string>();
            all.AddRange(guidArr);
            EditorApplication.update = delegate ()
            {
                string file = files[startIndex];

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)files.Length);
                for (int i = 0; i < guidArr.Count; i++)
                {
                    bool isIn = false;
                    //int index = guidArr[i].IndexOf("Assets");
                    //string gstr = guidArr[i].Remove(0, index);
                    if (Regex.IsMatch(File.ReadAllText(file), AssetDatabase.AssetPathToGUID(guidArr[i])))
                    {
                        isIn = true;
                        Debug.Log("关联文件:" + file);
                        if (all.Contains(guidArr[i]))
                            all.Remove(guidArr[i]);
                    }
                    if (isIn == false)
                    {
                        AssetImporter aip = AssetImporter.GetAtPath(guidArr[i]);
                        if (aip != null)
                        {
                            string abName = aip.assetBundleName;
                            if (string.IsNullOrEmpty(abName) == false)
                            {
                                if (all.Contains(guidArr[i]))
                                    all.Remove(guidArr[i]);
                            }
                        }
                    }
                }
                startIndex++;
                if (isCancel || startIndex >= files.Length)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.Log("匹配结束");
                    DelRes(all);
                }

            };

        }
    }
    private static void DelRes(List<string> all)
    {
        for (int i = 0; i < all.Count; i++)
        {
            Debug.Log("删除没有依赖的文件:" + all[i]);
            File.Delete(all[i]);
            AssetDatabase.Refresh();
        }
    }
    private static List<string> AllGuid(string path)
    {
        string allpath = Application.dataPath + path.Replace("Assets", "");
        string[] filesArr = Directory.GetFiles(allpath, "*.*", SearchOption.AllDirectories);
        List<string> fileList = new List<string>();
        fileList.AddRange(filesArr.ToList<string>());
        for (int i = 0; i < filesArr.Length; i++)
        {
            if (Directory.Exists(filesArr[i]))
            {
                fileList.AddRange(AllGuid(filesArr[i]));
            }
            if (filesArr[i].Contains(".meta"))
            {
                fileList.Remove(filesArr[i]);
            }
        }
        List<string> newList = new List<string>();
        foreach (string item in fileList)
        {
            int index = item.IndexOf("Assets");
            string gstr = item.Remove(0, index);
            newList.Add(gstr);
        }
        return newList;
    }
    [MenuItem("Assets/Find References", true)]
    static private bool VFind()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return (!string.IsNullOrEmpty(path));
    }
}