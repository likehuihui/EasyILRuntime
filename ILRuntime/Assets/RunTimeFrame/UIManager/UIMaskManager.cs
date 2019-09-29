/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RunTimeFrame.UI
{
    /// <summary>
    /// 遮罩控制
    /// </summary>
   public class UIMaskManager  
   {
        private Transform canvasRoot;
        // private Transform TransUIScriptsNode = null;
        private GameObject maskPanel;
        internal UIMaskManager(Transform root, GameObject mask)
        {
            canvasRoot = root;
            maskPanel = mask;
            maskPanel.SetActive(false);
        }
        /// <summary>
        /// 设置遮罩状态
        /// </summary>
        /// <param name="goDisplayUIForms">需要显示的UI窗体</param>
        /// <param name="lucenyType">显示透明度属性</param>
        internal void SetMaskWindow(GameObject goDisplayUIForms, UIFormLucencyType lucenyType = UIFormLucencyType.Lucency)
        {
            maskPanel.transform.SetParent(goDisplayUIForms.transform.parent);
            maskPanel.transform.SetAsLastSibling();
            //启用遮罩窗体以及设置透明度
            switch (lucenyType)
            {
                case UIFormLucencyType.Lucency:
                    maskPanel.SetActive(true);
                    //Color newColor1 = new Color(0, 0, 0, 0F / 255F);
                    maskPanel.GetComponent<Image>().color = SystemDefine.LucencyColor;
                    break;
                case UIFormLucencyType.Translucence:
                    maskPanel.SetActive(true);
                   // Color newColor2 = new Color(0, 0, 0, 50 / 255F);
                    maskPanel.GetComponent<Image>().color = SystemDefine.TranslucenceColor;
                    break;
                case UIFormLucencyType.ImPenetrable:
                    maskPanel.SetActive(true);
                   // Color newColor3 = new Color(0, 0, 0, 200F / 255F);
                    maskPanel.GetComponent<Image>().color = SystemDefine.ImPenetrableColor;
                    break;
                case UIFormLucencyType.Pentrate:
                    if (maskPanel.activeInHierarchy)
                    {
                        maskPanel.SetActive(false);
                    }
                    break;

                default:
                    break;
            }
            maskPanel.transform.SetAsLastSibling();
            goDisplayUIForms.transform.SetAsLastSibling();
        }
        /// <summary>
        /// 取消遮罩状态
        /// </summary>
	    internal void CancelMaskWindow()
        {
            maskPanel.transform.SetAsFirstSibling();
            maskPanel.SetActive(false);
        }
    }
}