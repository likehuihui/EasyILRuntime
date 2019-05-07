using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
namespace K.LocalWork
{

    public class UpdateDll : MonoBehaviour
    {
        public class DllVersion
        {
            public string dllName;
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
        private long nowUtc;
        private string jsonName = "dllVersion.json";
        private Dictionary<string, DllVersion> localDL;
        private DllVersionList netDL;
        private DllVersionList local;
        private string localPath;
        private Action end;
        private Action<float, string> progress;
        private Action<string> updateFail;
        private void Awake()
        {
            localPath = FileTools.GetLocalDLLPath();
        }
        // Use this for initialization
        public void Run(Action end)
        {
            this.end = end;
            netDL = new DllVersionList();
            localDL = new Dictionary<string, DllVersion>();
            InitGame();
        }
        void InitGame()
        {
            nowUtc = DateTime.UtcNow.ToFileTimeUtc();
            string url = FileTools.GetDllNetPath( jsonName + "?v=" + nowUtc);
            local = FileTools.ReadText<DllVersionList>(localPath + jsonName);
            if (local == null)
            {
                local = new DllVersionList();
                DllVersion dv = new DllVersion();
                dv.dllName = Main.Instance.runtimeConfig.runTimeName + ".zip";
                dv.version = 0;
                local.item.Add(dv);
                FileTools.CreateFile(localPath, jsonName, local);
            }

            for (int i = 0; i < local.item.Count; i++)
            {
                localDL.Add(local.item[i].dllName, local.item[i]);
            }
            StartCoroutine(DownSetting(url));
        }
        IEnumerator DownSetting(string url)
        {
            WWW www = new WWW(url);
            while (!www.isDone)
            {
                if (progress != null)
                    progress.Invoke(www.progress, "获取配置");
                yield return new WaitForEndOfFrame();
            }
            if (string.IsNullOrEmpty(www.error))
            {
                List<DllVersion> needUpdateDll = new List<DllVersion>();
                netDL = JsonMapper.ToObject<DllVersionList>(www.text);
                for (int i = 0; i < netDL.item.Count; i++)
                {
                    if (localDL.ContainsKey(netDL.item[i].dllName))
                    {
                        if (netDL.item[i].version > localDL[netDL.item[i].dllName].version)
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
                {
                    StartCoroutine(DownDll(needUpdateDll));
                }
                else
                {
                    if (end != null)
                        end();
                }
            }
            else
            {
                if (updateFail != null)
                    updateFail.Invoke("配置获取错误");
                Debug.LogError("配置获取错误");
            }
        }
        IEnumerator DownDll(List<DllVersion> needUpdate)
        {
            nowUtc = DateTime.UtcNow.ToFileTimeUtc();
            string url = FileTools.GetDllNetPath(needUpdate[0].dllName + "?v=" + nowUtc);
            WWW www = new WWW(url);
            while (!www.isDone)
            {
                Debug.Log(www.progress);
                if (progress != null)
                    progress.Invoke(www.progress, "下载资源:" + needUpdate[0].dllName);
                yield return new WaitForEndOfFrame();
            }
            if (string.IsNullOrEmpty(www.error))
            {
                ZipHelper zh = new ZipHelper();
                zh.UnZip(www.bytes, localPath + "RunTimeFrame/");
                while (!zh.isDone)
                {
                    if (progress != null)
                        progress.Invoke(www.progress, "解压资源:" + needUpdate[0].dllName);
                    yield return new WaitForEndOfFrame();
                }
                if (zh.error != null)
                {
                    if (updateFail != null)
                        updateFail.Invoke("脚本解压失败，请检查网络环境，并重启游戏");
                    Debug.LogError(zh.error);
                    yield break;
                }
                www.Dispose();
                bool isCover = false;
                for (int i = 0; i < local.item.Count; i++)
                {
                    if (local.item[i].dllName == needUpdate[0].dllName)
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
                if (end != null)
                    end();
            }
            else
            {
                if (updateFail != null)
                    updateFail.Invoke("下载失败:" + needUpdate[0].dllName);
                Debug.LogError("下载失败:" + needUpdate[0].dllName);
            }

        }
    }
}
