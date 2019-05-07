/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace K.LocalWork
{
    public class CoroutineCentre : MonoBehaviour
    {
        static CoroutineCentre instance;
        public static CoroutineCentre Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(CoroutineCentre)) as CoroutineCentre;
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "CoroutineCentre";
                        instance = obj.AddComponent<CoroutineCentre>();
                    }
                }
                return instance;
            }
        }
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}