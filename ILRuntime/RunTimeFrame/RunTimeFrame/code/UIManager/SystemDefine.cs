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
   public class SystemDefine  
   {
        /// <summary>
        /// 2D UI根节点名称
        /// </summary>
        public const string UICanvas2D = "UICamera2D";
        /// <summary>
        /// 普通UI显示节点名称
        /// </summary>
        public const string NormalPath = "Norma";
        /// <summary>
        /// 固定UI显示节点名称
        /// </summary>
        public const string FixedPath = "Fixed";
        /// <summary>
        /// 弹窗UI显示节点名称
        /// </summary>
        public const string PopPath = "Pop";
        /// <summary>
        ///  完全透明，不能穿透
        /// </summary>
        public static Color LucencyColor= new Color(0, 0, 0, 0F / 255F);
        /// <summary>
        ///  半透明，不能穿透
        /// </summary>
        public static Color TranslucenceColor = new Color(0, 0, 0, 50 / 255F);
        /// <summary>
        ///  低透明度，不能穿透
        /// </summary>
        public static Color ImPenetrableColor = new Color(0, 0, 0, 200F / 255F);
    }
    /// <summary>
    /// UI窗体的位置类型
    /// </summary>
    public enum UIFormType
    {
        /// <summary>
        /// 普通窗体
        /// </summary>
        Normal,
        /// <summary>
        /// 固定窗体
        /// </summary>
        Fixed,
        /// <summary>
        /// 弹出窗体
        /// </summary>
        PopUp
    }
    /// <summary>
    /// 窗体的显示类型
    /// </summary>
    public enum UIFormShowMode
    {
        /// <summary>
        /// 普通显示
        /// </summary>
        Normal,
        /// <summary>
        /// 反向切换显示
        /// </summary>
        ReverseChange,
        /// <summary>
        /// 隐藏其他显示
        /// </summary>
        HideOther
    }
    /// <summary>
    /// UI窗体的透明度类型
    /// </summary>
    public enum UIFormLucencyType
    {
        /// <summary>
        /// 完全透明，不能穿透
        /// </summary>
        Lucency,
        /// <summary>
        /// 半透明，不能穿透
        /// </summary>
        Translucence,
        /// <summary>
        /// 低透明度，不能穿透
        /// </summary>
        ImPenetrable,
        /// <summary>
        /// 可以穿透
        /// </summary>
        Pentrate
    }
    /// <summary>
    /// 窗体类型
    /// </summary>
    public class UIType
    {
        /// <summary>
        /// 是否关闭隐藏
        /// </summary>
        public bool isHidden = true;
        /// <summary>
        /// 是否清空"栈集合"，适用反向切换窗体，多弹窗UI
        /// </summary>
        public bool isClearStack = false;

        /// <summary>
        /// UI窗体的位置类型
        /// </summary>
        public UIFormType uiFormType = UIFormType.Normal;

        /// <summary>
        /// UI窗体的显示类型
        /// </summary>
        public UIFormShowMode uiFormShowMode = UIFormShowMode.Normal;

        /// <summary>
        /// UI窗体的透明度类型
        /// </summary>
        public UIFormLucencyType uiFormLucencyType = UIFormLucencyType.Lucency;
    }
}