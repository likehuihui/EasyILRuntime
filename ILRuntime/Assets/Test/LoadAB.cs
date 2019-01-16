/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RunTimeFrame
{
    public class LoadAB : MonoBehaviour
    {
        public string maniUrl;
        /// <summary>
        /// 资源描述
        /// </summary>
        AssetBundleManifest _manifest;
        public void Start()
        {
            //maniUrl = "file: F:/MyWork/EasyILRuntime/ILRuntime/Assets/StreamingAssets/ab/test/cube.ab.manifest";
            maniUrl = Application.dataPath + "/StreamingAssets/ab/ab";
           // maniUrl = Application.streamingAssetsPath + "/ab/test/cube.ab";
        }
        public void Loadmanifest()
        {
           // StartCoroutine(LoadConfig());
        }
        private IEnumerator LoadConfig()
        {
            AssetBundle ab = AssetBundle.LoadFromFile(maniUrl);
            yield return ab;
            _manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependList0 = _manifest.GetAllDependencies("test/cube.ab");
            string[] dependList = _manifest.GetAllDependencies("cube");
            string[] dependList1 = _manifest.GetAllDependencies("cube.ab");
            for (int i = 0; i < dependList.Length; i++)
            {
                Debug.Log(dependList[i]);
            }
        }
    }
}