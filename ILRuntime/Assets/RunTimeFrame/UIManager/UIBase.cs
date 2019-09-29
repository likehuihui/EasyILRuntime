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
    public class UIBase
    {
        /// <summary>
        /// ui GameObject
        /// </summary>
        public GameObject obj;
        /// <summary>
        /// ui名称
        /// </summary>
        public string name;
        protected UIType currentUIType = new UIType();
        /// <summary>
        /// 当前UI窗体类型
        /// </summary>
        public virtual UIType CurrentUIType
        {
            get { return currentUIType; }
            set { currentUIType = value; }
        }
        /// <summary>
        /// 打开
        /// </summary>
        public virtual void Open()
        {

        }
        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
              UIManager.Instance.CloseUIForm(name);
            // SystemLog.LogClick(WindData, "Close");
        }
        /// <summary>
        /// 窗体显示状态
        /// </summary>
        public virtual void Display()
        {
            //设置模态窗体调用(必须是弹出窗体)
            if (currentUIType.uiFormType == UIFormType.PopUp)
            {
                UIManager.Instance.maskManager.SetMaskWindow(obj, currentUIType.uiFormLucencyType);
            }
        }
        /// <summary>
        /// 窗体冻结状态（在"栈"集合中）
        /// </summary>
        public virtual void Freeze()
        {
            obj.SetActive(true);
        }
        /// <summary>
        /// 窗体隐藏状态（不在"栈"集合中)
        /// </summary>
        public virtual void Hiding()
        {
            //浏览功能UI界面不能隐藏，需要用到，故添加此判断
            if (currentUIType.isHidden)
                //取消模态窗体调用
                if (currentUIType.uiFormType == UIFormType.PopUp)
                {
                    UIManager.Instance.maskManager.CancelMaskWindow();
                }
        }
        /// <summary>
        /// 窗体重新显示状态
        /// </summary>
        public virtual void ReDisplay()
        {
            //设置模态窗体调用(必须是弹出窗体)
            if (currentUIType.uiFormType == UIFormType.PopUp)
            {
                UIManager.Instance.maskManager.SetMaskWindow(obj, currentUIType.uiFormLucencyType);
            }
        }
    }
}