/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using K.LocalWork;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
namespace K.Editors
{
    /// <summary>
    /// 平台数据
    /// </summary>
    [Serializable]
    public class ClientData
    {
        public  ClientData()
        {
            platform = "新平台";
            gameName = PlayerSettings.productName;
            gameID = PlayerSettings.applicationIdentifier;
            version = PlayerSettings.bundleVersion;
            iconPath = "iconUrl";
            preloadPath = "preloadUrl";
            windowsName = PlayerSettings.productName;
            DirectoryInfo temp = Directory.GetParent(Application.dataPath);
            outpath = temp.FullName.Replace("\\","/");
            outpath = outpath+"/AssetBundle/";
        }
        public string outpath;
        public string platform;
        public string gameName;
        public string gameID;
        public string version;
        public string iconPath;
        public string preloadPath;
        public string windowsName;
        public string androidName;
    }
    [Serializable]
    public class GameData
    {
        public string name;
        public bool select;
    }

    /// <summary>
    /// 编辑面板数据
    /// </summary>
    [Serializable]
    public class ClientDataList
    {
        public List<ClientData> clientData;
        public List<string> platformNameList;
        public List<GameData> game;
        public int platformIndex;
        public ClientDataList()
        {
            if (EditorApplication.isCompiling) return;
            clientData = new List<ClientData>();
            game = new List<GameData>();
            ClientData cld = new ClientData();
            clientData.Add(cld);
            platformNameList = new List<string>();
            platformNameList.Add(cld.platform);
            platformIndex = 0;
        }
    }
    public class EditorMainWindow : EditorBase
    {
        public static ClientDataList cdl;
        public ResSetting resSetting;
        public ClientData nowData;
        public static string dataListUrl = "";
        private Vector2 scrollPosition;
        private string configName = "Bulid.json";
        private Dictionary<string, DependentsCrosses> _crossDic = new Dictionary<string, DependentsCrosses>();
        private string platform;
        private static BuildTarget targetPlatform
        {
            get
            {
                BuildTarget platform;
#if UNITY_STANDALONE
                platform = BuildTarget.StandaloneWindows;
#elif UNITY_IPHONE
                platform = BuildTarget.iOS;
#elif UNITY_ANDROID
                platform = BuildTarget.Android;
#endif
                return platform;
            }
        }
        private static BuildTargetGroup GetBuildGroupByTarget
        {
            get
            {
                switch (targetPlatform)
                {
                    case BuildTarget.Android:
                        return BuildTargetGroup.Android;
                    case BuildTarget.StandaloneWindows:
                    case BuildTarget.StandaloneWindows64:
                    case BuildTarget.StandaloneLinux:
                    case BuildTarget.StandaloneLinux64:
                    case BuildTarget.StandaloneLinuxUniversal:
                        return BuildTargetGroup.Standalone;
                    case BuildTarget.iOS:
                        return BuildTargetGroup.iOS;
                }
                return BuildTargetGroup.Unknown;
            }
        }
        public static void Open()
        {
            EditorMainWindow ab = EditorWindow.GetWindow<EditorMainWindow>("编辑窗口", true);
            ab.minSize = new Vector2(640, 360);
            ab.maxSize = new Vector2(1280, 720);
            ab.Show();


        }
        private void OnEnable()
        {
            cdl = LoadConfig<ClientDataList>(configName);
            if (cdl == null)
                cdl = new ClientDataList();
        }
        private void OnGUI()
        {
            if (cdl == null) return;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            //GUILayout.Space(50);
            //ModeToggle();

            //cdl = new ClientDataList();
            cdl.platformIndex = EditorGUILayout.Popup("平台选择:", cdl.platformIndex, cdl.platformNameList.ToArray());
            nowData = cdl.clientData[cdl.platformIndex];
            platform = nowData.platform;
            if (GUILayout.Button("编辑平台", GUILayout.Width(100)))
            {
                EditorClientWindow ab = EditorWindow.GetWindow<EditorClientWindow>("平台数据", true);
                ab.minSize = new Vector2(640, 360);
                ab.maxSize = new Vector2(1280, 720);
                ab.Show();
                ab.Open(nowData, false);
            }
            if (GUILayout.Button("增加平台", GUILayout.Width(100)))
            {
                ClientData cd = new ClientData();
                EditorClientWindow ab = EditorWindow.GetWindow<EditorClientWindow>("平台数据", true);
                ab.minSize = new Vector2(640, 360);
                ab.maxSize = new Vector2(1280, 720);
                ab.Show();
                ab.Open(cd, true);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("输路径:" + nowData.outpath);
            EditorGUILayout.LabelField("游戏名称:" + nowData.gameName);
            EditorGUILayout.LabelField("游戏ID:" + nowData.gameID);
            EditorGUILayout.LabelField("版本号:" + nowData.version);
            EditorGUILayout.LabelField("icon路劲:" + nowData.iconPath);
            EditorGUILayout.LabelField("主场景路径:" + nowData.preloadPath);
            EditorGUILayout.LabelField("pc名称:" + nowData.windowsName);
            EditorGUILayout.LabelField("Android名称:" + nowData.androidName);


            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < cdl.game.Count; i++)
            {
                cdl.game[i].select = EditorGUILayout.ToggleLeft(i + "子游戏:", cdl.game[i].select);
            }

            GUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存配置"))
            {
                Save();
            }
            if (GUILayout.Button("应用配置"))
            {
                Apply();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("打包AB资源"))
            {
                BuildAB();
            }
            if (GUILayout.Button("生成配置文件"))
            {
                CreatResSetting();
            }
            // GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        private void Apply()
        {
            for (int i = 0; i < cdl.clientData.Count; i++)
            {
                if (cdl.clientData[i].platform == nowData.platform)
                {
                    cdl.clientData[i] = nowData;
                    break;
                }
            }
            PlayerSettings.productName = nowData.gameName;
            PlayerSettings.applicationIdentifier = nowData.gameID;
            PlayerSettings.bundleVersion = nowData.version;
            BuildTargetGroup group = GetBuildGroupByTarget;
#if UNITY_STANDALONE
            group = GetBuildGroupByTarget;
#elif UNITY_ANDROID
            PlayerSettings.Android.keystorePass = "123456";
            PlayerSettings.Android.keyaliasPass = "123456";
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
            group = GetBuildGroupByTarget(BuildTarget.Android);
#elif UNITY_IPHONE
            group = GetBuildGroupByTarget(BuildTarget.iOS);
            PlayerSettings.SetArchitecture(group,0);
            PlayerSettings.stripEngineCode = false;
            PlayerSettings.SetArchitecture(group, 1);
#endif
            int[] iconSizes = PlayerSettings.GetIconSizesForTargetGroup(group);
            Texture2D[] texture = new Texture2D[iconSizes.Length];
            for (int i = 0; i < texture.Length; i++)
            {
                texture[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(nowData.iconPath);
            }
            PlayerSettings.SetIconsForTargetGroup(group, texture);
            AssetDatabase.Refresh();
            Debug.Log("设置成功:" + nowData.gameName + "+" + nowData.gameID + "+" + nowData.version);
        }
        public static bool AddClient(ClientData data)
        {
            for (int i = 0; i < cdl.clientData.Count; i++)
            {
                if (cdl.clientData[i].platform == data.platform)
                {
                    return false;
                }
            }
            cdl.platformNameList.Add(data.platform);
            cdl.clientData.Add(data);
            return true;
        }
        private void Save()
        {
            SaveConfig(cdl, configName);
        }
        /// <summary>
        /// 得到AB文件的MD5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetMD5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                int len = (int)fs.Length;
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(data);
                string fileMD5 = "";
                foreach (byte b in result)
                {
                    fileMD5 += Convert.ToString(b, 16);
                }
                Debug.LogFormat("{0}{1}", filePath, fileMD5);
                return fileMD5;
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e.Message);
                return "";
            }
        }
        public void CreatResSetting()
        {
            resSetting = new ResSetting();
            resSetting.manifestName = "ab";
            string[] allName = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < allName.Length; i++)
            {
                string url = nowData.outpath +"/"+ allName[i];
                ABItem item = new ABItem();
                item.name = allName[i];
                item.version = GetMD5(url);

                resSetting.items.Add(item);
            }
            DirectoryInfo dir = Directory.GetParent(Application.dataPath);
            string u = Path.Combine(dir.ToString(), "ProjectData/ResVersion.json");
            string data = JsonMapper.ToJson(resSetting);
            ShowNotification(new GUIContent("生成配置完成"));
            System.Diagnostics.Process.Start(Path.Combine(dir.ToString(), "ProjectData"));
            File.WriteAllText(u, data);
        }
        private void BuildAB()
        {
            if (!Directory.Exists(nowData.outpath))
            {
                Directory.CreateDirectory(nowData.outpath);
            }
            AssetBundleManifest buildManifest = BuildPipeline.BuildAssetBundles(nowData.outpath, BuildAssetBundleOptions.ChunkBasedCompression, targetPlatform);
            //string[] allName = AssetDatabase.GetAllAssetBundleNames();
            //Dictionary<string, List<string>> abbDic = new Dictionary<string, List<string>>();
            //foreach (var abName in allName)
            //{
            //    var dir = Path.GetDirectoryName(abName);
            //    if (false == _crossDic.ContainsKey(dir))
            //    {
            //        _crossDic[dir] = new DependentsCrosses();
            //    }
            //    DependentsCrosses cross = _crossDic[dir];

            //    //AB包对应的资源
            //    string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(abName);
            //    foreach (var assetPath in assetPaths)
            //    {
            //        //获取到资源
            //        AssetImporter ai = AssetImporter.GetAtPath(assetPath);
            //        if (ai.assetBundleVariant == "" || ai.assetBundleVariant == platform)
            //        {
            //            //这里必须用ai.assetBundleName才不会有assetBundleVariant标记
            //            if (abbDic.ContainsKey(ai.assetBundleName) == false)
            //            {
            //                abbDic[ai.assetBundleName] = new List<string>();
            //            }
            //            abbDic[ai.assetBundleName].Add(assetPath);
            //            cross.AddAssetPath(ai);
            //        }
            //    }
            //}
            ////提取出每个包的交叉资源
            //foreach (var entry in _crossDic)
            //{
            //    var crossResult = entry.Value.GetCrossResult();
            //    if (crossResult.Count > 0)
            //    {
            //        string dependsBaseName = string.Format("{0}/depends_base.ab", entry.Key);
            //        abbDic[dependsBaseName] = crossResult;
            //    }
            //}
            //AssetBundleBuild[] abbList = new AssetBundleBuild[abbDic.Count];
            //int i = 0;
            //foreach (var abb in abbDic)
            //{
            //    abbList[i] = new AssetBundleBuild();
            //    abbList[i].assetBundleName = abb.Key;
            //    abbList[i].assetNames = abb.Value.ToArray();
            //    i++;
            //}

            //if (false == Directory.Exists(outPath))
            //{
            //    Directory.CreateDirectory(outPath);
            //}

            //   AssetBundleManifest abm = BuildPipeline.BuildAssetBundles(outPath, abbList, BuildAssetBundleOptions.ChunkBasedCompression, targetPlatform);
            //  AssetDatabase.Refresh();
        }
        private void OnDisable()
        {

        }
    }
}