/*
 *    日期:
 *    作者:like
 *    标题:
 *    功能:
*/
using K.LocalWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace RunTimeFrame
{
    /// <summary>
    /// 资源管理
    /// </summary>
    public class ResourcesCentre
    {
        private ResourcesCentre() { }
        /// <summary>
        /// 是否使用本地资源
        /// </summary>
        public bool isUse;
        /// <summary>
        /// 资源描述
        /// </summary>
        AssetBundleManifest manifest;

        /// <summary>
        /// 已加载的AB资源字典
        /// </summary>
        Dictionary<string, AssetBundle> loadedABDic;
        private string rootDirUrl;
        static ResourcesCentre instance;
        public static ResourcesCentre Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResourcesCentre();
                return instance;
            }
        }
        public void Init(bool isUseLocalResourse, string abManifestName = "")
        {
            isUse = isUseLocalResourse;
            if (!isUse)
            {
                ABLoadManifest(abManifestName);
            }

        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName"></param>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public T Load<T>(string abName, string prefabName) where T : UnityEngine.Object
        {
            if (isUse)
            {
                return LoadLocalRes<T>(prefabName);
            }
            else
            {
                AssetBundle ab = LoadAB(abName);
                return ab.LoadAsset<T>(prefabName);
            }
        }
        /// <summary>
        /// 使用本地资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        private T LoadLocalRes<T>(string prefabName) where T : UnityEngine.Object
        {
            return Resources.Load<T>(prefabName);

        }
        /// <summary>
        /// 异步加载AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="prefabName"></param>
        /// <param name="onLoaded"></param>
        /// <param name="onProgress"></param>
        public void LoadAsync(string abName, string prefabName, Action<UnityEngine.Object> onLoaded, Action<float> onProgress = null)
        {
            if (isUse)
            {
                CoroutineCentre.Instance.StartCoroutine(LoadAsyncResource(prefabName, onLoaded, onProgress));
            }
            else
            {
                AssetBundle ab = LoadAB(abName);
                CoroutineCentre.Instance.StartCoroutine(LoadAsyncAB(ab, prefabName, onLoaded, onProgress));
            }
        }
        /// <summary>
        /// 卸载所有的Resources资源
        /// </summary>
        public void UnloadAllRes()
        {
            Resources.UnloadUnusedAssets();
        }
        /// <summary>
        /// 卸载指定的Resources资源
        /// </summary>
        /// <param name="assetToUnload"></param>
        public void UnloadRes(UnityEngine.Object assetToUnload)
        {
            Resources.UnloadAsset(assetToUnload);
        }
        /// <summary>
        /// 卸载指定的AB资源包
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="isUnloadAllLoaded"></param>
        /// <param name="isUnloadDepends"></param>
        public void UnloadAB(string abName, bool isUnloadAllLoaded = false, bool isUnloadDepends = true)
        {
            if (loadedABDic.ContainsKey(abName))
            {
                AssetBundle ab = loadedABDic[abName];
                loadedABDic.Remove(abName);
                ab.Unload(isUnloadAllLoaded);
                //Debug.LogFormat("释放AB：{0}", abName);

                if (isUnloadDepends)
                {
                    string[] dependList = manifest.GetAllDependencies(abName);
                    foreach (string depend in dependList)
                    {
                        UnloadAB(depend, isUnloadAllLoaded, isUnloadDepends);
                    }
                }
            }
        }
        /// <summary>
        /// 卸载所有的AB资源
        /// </summary>
        /// <param name="isUnloadAllLoaded"></param>
        public void UnloadAllAB(bool isUnloadAllLoaded = false)
        {
            if (null != loadedABDic)
            {
                foreach (AssetBundle cached in loadedABDic.Values)
                {
                    cached.Unload(isUnloadAllLoaded);
                }
            }
        }

        private void ABLoadManifest(string manifestFilePath)
        {
            UnloadAllAB();
            loadedABDic = new Dictionary<string, AssetBundle>();
            rootDirUrl = Path.GetDirectoryName(manifestFilePath);
            AssetBundle ab = AssetBundle.LoadFromFile(manifestFilePath);
            //得到依赖文件
            manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependsFile = manifest.GetAllDependencies("666");
            if (manifest == null)
            {
                Debug.LogError("依赖文件有错:" + manifestFilePath);
            }
            ab.Unload(false);
        }
        /// <summary>
        /// 自动处理加载AB包依赖
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private AssetBundle LoadAB(string abName)
        {
            string abPath = GenerateAbsoluteFile(rootDirUrl, abName);
            if (false == File.Exists(abPath))
            {
                Debug.LogError("AB资源不存在:" + abPath);
            }
            string[] dependList = manifest.GetAllDependencies(abName);
            for (int i = 0; i < dependList.Length; i++)
            {
                if (!loadedABDic.ContainsKey(dependList[i]))
                    loadedABDic.Add(dependList[i], AssetBundle.LoadFromFile(GenerateAbsoluteFile(rootDirUrl, dependList[i])));
            }
            if (loadedABDic.ContainsKey(abName))
                return loadedABDic[abName];
            else
                return AssetBundle.LoadFromFile(GenerateAbsoluteFile(rootDirUrl, abName));
        }

        private IEnumerator LoadAsyncAB(AssetBundle ab, string assetName, Action<UnityEngine.Object> onGet, Action<float> onProgress)
        {
            AssetBundleRequest abr = ab.LoadAssetAsync<GameObject>(assetName);
            do
            {
                if (onProgress != null)
                {
                    onProgress.Invoke(abr.progress);
                }
                yield return new WaitForEndOfFrame();
            }
            while (false == abr.isDone);
            //加载完成
            onGet.Invoke(abr.asset);
        }
        private IEnumerator LoadAsyncResource(string assetPath, Action<UnityEngine.Object> onLoaded, Action<float> onProgress)
        {
            ResourceRequest rr = Resources.LoadAsync(assetPath);
            do
            {
                if (onProgress != null)
                {
                    onProgress.Invoke(rr.progress);
                }
                yield return new WaitForEndOfFrame();
            }
            while (false == rr.isDone);

            //加载完成
            onLoaded.Invoke(rr.asset);
        }
        private string GenerateAbsoluteFile(string filePath, string fileName)
        {
            return string.Concat(filePath, "/", fileName).Trim();
        }
    }
}