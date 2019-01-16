using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
namespace K.LocalWork
{

    public class UpdateDll : MonoBehaviour
    {
        public class DllVersion
        {
            public string name;
            public int version;
        }
        public class DllVersionList
        {
            public DllVersionList()
            {
                item = new List<DllVersion>();
            }
            public List<DllVersion> item;
        }
        private string urlServer = "39.104.184.3/client_assets/0_RunTime_Test/resources";
        private long nowUtc;
        private string jsonName = "dllVersion.json";
        private string dllName = "dllVersion.zip";
        private Dictionary<string, DllVersion> localDL;
        private DllVersionList netDL;
        private DllVersionList local;
        private ProgressBar progress;
        private string localPath;
        // Use this for initialization
        public void Open()
        {
            localPath = Application.dataPath + "/StreamingAssets/";
            netDL = new DllVersionList();
            localDL = new Dictionary<string, DllVersion>();
            progress = GameObject.Find("ProgressBar").GetComponent<ProgressBar>();
            InitGame();
        }
        void CreatDllSetting(string txt)
        {
            FileTools.CreateFile(Application.dataPath, "dllVersion.json", txt);
        }
        void InitGame()
        {
            nowUtc = DateTime.UtcNow.ToFileTimeUtc();
            string url = string.Format("http://{0}/{1}?v={2}", urlServer, jsonName, nowUtc);
            local = FileTools.ReadText<DllVersionList>(localPath + "/" + jsonName);
            if (local == null)
                local = new DllVersionList();
            for (int i = 0; i < local.item.Count; i++)
            {
                localDL.Add(local.item[i].name, local.item[i]);
            }
            StartCoroutine(DownSetting(url));
        }
        IEnumerator DownSetting(string url)
        {
            WWW www = new WWW(url);
            while (!www.isDone)
            {
                progress.SetValue(www.progress, "获取配置");
                yield return new WaitForEndOfFrame();
            }
            if (string.IsNullOrEmpty(www.error))
            {
                List<DllVersion> needUpdateDll = new List<DllVersion>();
                netDL = JsonMapper.ToObject<DllVersionList>(www.text);
                for (int i = 0; i < netDL.item.Count; i++)
                {
                    if (localDL.ContainsKey(netDL.item[i].name))
                    {
                        if (netDL.item[i].version > localDL[netDL.item[i].name].version)
                        {
                            needUpdateDll.Add(netDL.item[i]);
                        }
                    }
                    else
                    {
                        needUpdateDll.Add(netDL.item[i]);
                    }
                }
                if (needUpdateDll.Count > 0)
                    StartCoroutine(DownDll(needUpdateDll));
            }
            else
            {
                Debug.LogError("配置获取错误");
            }
        }
        IEnumerator DownDll(List<DllVersion> needUpdate)
        {
            nowUtc = DateTime.UtcNow.ToFileTimeUtc();
            string url = string.Format("http://{0}/{1}?v={2}", urlServer, needUpdate[0].name, nowUtc);
            WWW www = new WWW(url);
            while (!www.isDone)
            {
                progress.SetValue(www.progress, "下载资源:" + needUpdate[0].name);
                yield return new WaitForEndOfFrame();
            }
            if (string.IsNullOrEmpty(www.error))
            {
                ZipHelper zh = new ZipHelper();
                zh.UnZip(www.bytes, localPath + "RunTimeFrame/");
                while (!zh.isDone)
                {
                    progress.SetValue(zh.progress, "解压资源:" + needUpdate[0].name);
                    yield return new WaitForEndOfFrame();
                }
                if (zh.error != null)
                {
                    //seterror("脚本解压失败，请检查网络环境，并重启游戏");
                    Debug.Log(zh.error);
                    yield break;
                }
                www.Dispose();
                bool isCover = false;
                for (int i = 0; i < local.item.Count; i++)
                {
                    if (local.item[i].name == needUpdate[0].name)
                    {
                        isCover = true;
                        local.item[i].version = needUpdate[0].version;
                        break;
                    }
                }
                if (!isCover)
                {
                    local.item.Add(needUpdate[0]);
                }
                if (needUpdate.Count >= 1)
                {
                    needUpdate.RemoveAt(0);
                }
                if (needUpdate.Count >= 1)
                {
                    StartCoroutine(DownDll(needUpdate));
                }
                string localTxt = JsonMapper.ToJson(local);
                Debug.Log("保存文件:" + localTxt);
                FileTools.CreateFile(localPath, jsonName, localTxt);
            }
            else
            {
                Debug.LogError("下载失败:" + needUpdate[0].name);
            }

        }
    }
}
