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
                        instance = obj.AddComponent<CoroutineCentre>();
                    }
                }
                return instance;
            }
        }
        public void Go(IEnumerator routine)
        {
            StartCoroutine(routine);
        }

        public void Stop(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        public void StopAll()
        {
            StopAllCoroutines();
        }
    }
}