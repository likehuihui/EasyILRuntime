/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RunTimeFrame.UI
{
    public class UITest : MonoBehaviour
    {
        private void Start()
        {
            Animator anim = GetComponent<Animator>();
            float le = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //Transform root = GameObject.Find("MainPanel").transform;
            //UIManager.Instance.Init(root, root.Find("Normal"), root.Find("Mask"), root.Find("Fixed"), root.Find("Pop"));
            //UIManager.Instance.ShowUIForm("UITest");
        }
    }
}