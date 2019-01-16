/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace K.Editors
{
    /// <summary>
    /// 查找指定的AssetBundle中交叉依赖的资源
    /// </summary>
    public class DependentsCrosses
    {

        Dictionary<string, HashSet<string>> _dependsCollectionDic = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// 添加一个AssetBundle包的地址
        /// </summary>
        /// <param name="assetbundlePath"></param>
        public void AddAssetBundle(string assetBundleName)
        {
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            foreach (var assetPath in assetPaths)
            {
                AssetImporter ai = AssetImporter.GetAtPath(assetPath);
                AddAssetPath(ai);
            }
        }

        /// <summary>
        /// 加入一个资源的路径地址
        /// </summary>
        /// <param name="assetPath"></param>
        public void AddAssetPath(AssetImporter ai)
        {
            if (false == _dependsCollectionDic.ContainsKey(ai.assetBundleName))
            {
                _dependsCollectionDic[ai.assetBundleName] = new HashSet<string>();
            }

            HashSet<string> dependsSet = _dependsCollectionDic[ai.assetBundleName];

            //获取依赖的资源
            string[] dps = AssetDatabase.GetDependencies(ai.assetPath);
            foreach (string dependPath in dps)
            {
                if (dependPath.Contains(ai.assetPath) || dependPath.Contains(".cs"))
                {
                    //要过滤掉依赖的自己本身和脚本文件，自己本身的名称已设置，而脚本不能打包
                    continue;
                }

                AssetImporter assetAi = AssetImporter.GetAtPath(dependPath);
                if (assetAi.assetBundleName != "")
                {
                    //已经指定到AB的不做处理
                    continue;
                }

                if (false == dependsSet.Contains(dependPath))
                {
                    dependsSet.Add(dependPath);
                }
            }

            _dependsCollectionDic[ai.assetBundleName] = dependsSet;
        }

        /// <summary>
        /// 得到资源交叉的结果
        /// </summary>
        /// <returns></returns>
        public List<string> GetCrossResult()
        {
            List<string> crossResult = new List<string>();
            HashSet<string> dependsCountSet = new HashSet<string>();
            foreach (var entry in _dependsCollectionDic)
            {
                foreach (var dependPath in entry.Value)
                {
                    if (false == dependsCountSet.Contains(dependPath))
                    {
                        dependsCountSet.Add(dependPath);
                    }
                    else
                    {
                        crossResult.Add(dependPath);
                    }
                }
            }

            return crossResult;
        }
    }
}