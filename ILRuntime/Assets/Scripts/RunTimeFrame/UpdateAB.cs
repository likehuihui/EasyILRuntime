/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using K.LocalWork;
using System.IO;
using System;
using LitJson;
using UnityEngine.Networking;

namespace RunTimeFrame
{
    public class UpdateAB
    {
        private string netUrl;
        private string localUrl;
        private ResSetting res;
        public ResSetting netRes;
        private string resName = "ResVersion.json";
        private List<ABItem> needUpdate = new List<ABItem>();
        private List<ABItem> finishUpdate = new List<ABItem>();
        private Action<float, string> progress;
        public Action updateEnd;
        public UpdateAB(ResSetting res, string url)
        {
            this.netUrl = url;
            this.res = res;
            localUrl = FileTools.GetLocalPath("ab");
        }

        public void Start()
        {
            CoroutineCentre.Instance.StartCoroutine(LoadRes());
        }
        private IEnumerator LoadRes()
        {
            WWW www = new WWW(netUrl + resName);
            while (www.isDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
            if (string.IsNullOrEmpty(www.error))
            {
                netRes = JsonMapper.ToObject<ResSetting>(www.text);
                res.manifestName = netRes.manifestName;
                List<ABItem> net = netRes.items;
                List<ABItem> local = res.items;
                needUpdate = new List<ABItem>(net.ToArray());
                foreach (ABItem netab in net)
                {
                    foreach (ABItem localab in local)
                    {
                        if (netab.name == localab.name && netab.version == localab.version)
                        {
                            if (FileTools.FileExist(localUrl + localab.name) == true)
                                needUpdate.Remove(netab);
                        }
                    }
                }
            }
            CoroutineCentre.Instance.StartCoroutine(Update(needUpdate));
        }
        private IEnumerator Update(List<ABItem> net)
        {
            if (net.Count > 0)
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(netUrl + "ab" + net[0].name);
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    while (!webRequest.isDone)
                    {
                        if (progress != null)
                            progress.Invoke(webRequest.downloadProgress, "下载资源:" + net[0].name);
                        yield return new WaitForEndOfFrame();
                    }
                    if (webRequest.isDone)
                    {
                        bool finish = FileTools.CreateModelFile(localUrl + net[0].name, webRequest.downloadHandler.data);
                        if (progress != null)
                            progress.Invoke(1, "下载资源:" + net[0].name);
                        Debug.Log("下载完成:" + net[0].name);
                        bool isIn = false;
                        for (int i = 0; i < res.items.Count; i++)
                        {
                            if (res.items[i].name == net[0].name)
                            {
                                res.items.Remove(res.items[i]);
                                res.items.Add(net[0]);
                                isIn = true;
                                break;
                            }
                        }
                        if (isIn == false)
                        {
                            res.items.Add(net[0]);
                        }
                        net.RemoveAt(0);
                        if (net.Count > 0)
                        {
                            CoroutineCentre.Instance.StartCoroutine(Update(net));
                        }
                        else
                        {
                            string u = FileTools.GetLocalPath("ResVersion.json");
                            string data = JsonMapper.ToJson(res);
                            File.WriteAllText(u, data);
                            if (updateEnd != null)
                                updateEnd.Invoke();
                        }
                    }
                }

            }
            else
            {
                if (updateEnd != null)
                    updateEnd.Invoke();
            }
        }
    }
}
