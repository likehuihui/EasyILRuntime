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
    public class UIManager
    {
        private UIManager()
        {
        }
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }
        /// <summary>
        /// 缓存所有UI窗体
        /// </summary>
        Dictionary<string, UIBase> allUIForms = new Dictionary<string, UIBase>();
        /// <summary>
        /// 当前显示的UI窗体
        /// </summary>
        Dictionary<string, UIBase> currentShowUIForms = new Dictionary<string, UIBase>();
        /// <summary>
        /// 定义“栈”集合，存储显示当前所有[反向切换]窗体
        /// </summary>
        Stack<UIBase> stackCurrentUIForms = new Stack<UIBase>();
        /// <summary>
        /// UI根节点
        /// </summary>
        private Transform transRoot;
        /// <summary>
        /// 普通UI显示节点
        /// </summary>
        internal Transform transNormalUI;
        /// <summary>
        /// 固定UI显示节点
        /// </summary>
        internal Transform transFixedUI;
        /// <summary>
        /// 弹窗UI显示节点
        /// </summary>
        internal Transform transPopUI;
        /// <summary>
        /// 遮罩
        /// </summary>
        internal UIMaskManager maskManager;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="mask">遮罩</param>
        /// <param name="normal">普通UI显示节点</param>
        /// <param name="fixedTr">固定UI显示节点</param>
        /// <param name="pop">弹窗UI显示节点</param>
        public void Init(Transform root, Transform mask, Transform normal, Transform fixedTr, Transform pop)
        {
            allUIForms = new Dictionary<string, UIBase>();
            currentShowUIForms = new Dictionary<string, UIBase>();
            stackCurrentUIForms = new Stack<UIBase>();
            transRoot = root;
            transNormalUI = transPopUI;
            transFixedUI = fixedTr;
            transPopUI = pop;
            maskManager = new UIMaskManager(transRoot, mask.gameObject);
        }
        /// <summary>
        /// 关闭（返回上一个）窗体
        /// </summary>
        /// <param name="uiFormName">UI窗体名称</param>
        public void CloseUIForm(string winName)
        {
            UIBase baseUiForm;                          //窗体基类

            //参数检查
            if (string.IsNullOrEmpty(winName)) return;
            //“所有UI窗体”集合中，如果没有记录，则直接返回
            allUIForms.TryGetValue(winName, out baseUiForm);
            if (baseUiForm == null) return;
            //根据窗体不同的显示类型，分别作不同的关闭处理
            switch (baseUiForm.CurrentUIType.uiFormShowMode)
            {
                case UIFormShowMode.Normal:
                    //普通窗体的关闭
                    ExitUIForms(winName);
                    break;
                case UIFormShowMode.ReverseChange:
                    //反向切换窗体的关闭
                    PopUIFroms();
                    break;
                case UIFormShowMode.HideOther:
                    //隐藏其他窗体关闭
                    ExitUIFormsAndDisplayOther(winName);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 显示一个窗口
        /// </summary>
        /// <param name="winName"></param>
        public void ShowUIForm(string winName)
        {
            if (allUIForms.ContainsKey(winName))
            {
                UIBase ui = allUIForms[winName];
                if (ui.CurrentUIType.isClearStack)
                    ClearStackArray();
                switch (ui.CurrentUIType.uiFormShowMode)
                {
                    case UIFormShowMode.Normal:
                        LoadUIToCurrentCache(winName);
                        break;
                    case UIFormShowMode.ReverseChange:
                        PushUIFormToStack(winName);
                        break;
                    case UIFormShowMode.HideOther:
                        EnterUIFormsAndHideOther(winName);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("窗口已经打开...");
            }

        }
        /// <summary>
        /// 退出指定UI窗体
        /// </summary>
        /// <param name="strUIFormName">UI窗体名称</param>
        private void ExitUIForms(string strUIFormName)
        {
            UIBase baseUIForm;                          //窗体基类

            //"正在显示集合"中如果没有记录，则直接返回。
            currentShowUIForms.TryGetValue(strUIFormName, out baseUIForm);
            if (baseUIForm == null) return;
            //指定窗体，标记为“隐藏状态”，且从"正在显示集合"中移除。
            baseUIForm.Hiding();
            currentShowUIForms.Remove(strUIFormName);
        }
        /// <summary>
        /// (“隐藏其他”属性)关闭窗体，且显示其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void ExitUIFormsAndDisplayOther(string strUIName)
        {
            UIBase baseUIForm;                          //UI窗体基类


            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;
            currentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm == null) return;
            baseUIForm.Hiding();
            currentShowUIForms.Remove(strUIName);
            foreach (UIBase baseUI in currentShowUIForms.Values)
            {
                baseUI.ReDisplay();
            }
            foreach (UIBase staUI in stackCurrentUIForms)
            {
                staUI.ReDisplay();
            }
        }
        /// <summary>
        /// （“反向切换”属性）窗体的出栈逻辑
        /// </summary>
        private void PopUIFroms()
        {
            if (stackCurrentUIForms.Count >= 2)
            {
                //出栈处理
                UIBase topUIForms = stackCurrentUIForms.Pop();
                //做隐藏处理
                topUIForms.Hiding();
                //出栈后，下一个窗体做“重新显示”处理。
                UIBase nextUIForms = stackCurrentUIForms.Peek();
                nextUIForms.ReDisplay();
            }
            else if (stackCurrentUIForms.Count == 1)
            {
                //出栈处理
                UIBase topUIForms = stackCurrentUIForms.Pop();
                //做隐藏处理
                topUIForms.Hiding();
            }
        }
        /// <summary>
        /// UI窗体入栈
        /// </summary>
        /// <param name="uiFormName">窗体的名称</param>
        private void PushUIFormToStack(string uiFormName)
        {
            UIBase baseUI;
            if (stackCurrentUIForms.Count > 0)
            {
                UIBase topUIForm = stackCurrentUIForms.Peek();
                topUIForm.Freeze();
            }
            //判断“UI所有窗体”集合是否有指定的UI窗体，有则处理。
            allUIForms.TryGetValue(uiFormName, out baseUI);
            if (baseUI != null)
            {
                //当前窗口显示状态
                baseUI.Display();
                //把指定的UI窗体，入栈操作。
                if (!stackCurrentUIForms.Contains(baseUI))
                    stackCurrentUIForms.Push(baseUI);
            }
            else
            {
                Debug.Log("baseUIForm==null,Please Check, 参数 uiFormName=" + uiFormName);
            }
        }
        /// <summary>
        /// 把当前窗体加载到“当前窗体”集合中
        /// </summary>
        /// <param name="uiFormName"></param>
        private void LoadUIToCurrentCache(string uiFormName)
        {
            UIBase baseUi;
            UIBase baseUIFormFromAllCache;
            //如果“正在显示”的集合中，存在整个UI窗体，则直接返回
            currentShowUIForms.TryGetValue(uiFormName, out baseUi);
            if (baseUi != null) return;
            //把当前窗体，加载到“正在显示”集合中
            allUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);
            if (baseUIFormFromAllCache != null)
            {
                currentShowUIForms.Add(uiFormName, baseUIFormFromAllCache);
                baseUIFormFromAllCache.Display();
            }
        }
        /// <summary>
        /// (“隐藏其他”属性)打开窗体，且隐藏其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void EnterUIFormsAndHideOther(string strUIName)
        {
            UIBase baseUIForm;
            UIBase baseUIFormFromALL;
            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;
            currentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm != null) return;
            //把“正在显示集合”与“栈集合”中所有窗体都隐藏。
            foreach (UIBase baseUI in currentShowUIForms.Values)
            {
                baseUI.Hiding();
            }
            foreach (UIBase staUI in stackCurrentUIForms)
            {
                staUI.Hiding();
            }
            //把当前窗体加入到“正在显示窗体”集合中，且做显示处理。
            allUIForms.TryGetValue(strUIName, out baseUIFormFromALL);
            if (baseUIFormFromALL != null)
            {
                currentShowUIForms.Add(strUIName, baseUIFormFromALL);
                baseUIFormFromALL.Display();
            }
        }
        /// <summary>
        /// 是否清空“栈集合”中de数据
        /// </summary>
        /// <returns></returns>
        private bool ClearStackArray()
        {
            if (stackCurrentUIForms != null && stackCurrentUIForms.Count >= 1)
            {
                //清空栈集合
                stackCurrentUIForms.Clear();
                return true;
            }
            return false;
        }
    }
}