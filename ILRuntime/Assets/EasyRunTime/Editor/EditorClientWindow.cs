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
    public class EditorClientWindow : EditorBase
    {
        public ClientData clientData;
        /// <summary>
        /// 是否是新增加平台
        /// </summary>
        private static bool addOre;
        private ClientData editorData;
        public void Open(ClientData data, bool addOrEditor)
        {
            addOre = addOrEditor;

            clientData = data;
            editorData = new ClientData();
            editorData.androidName = clientData.androidName;
            editorData.platform = clientData.platform;
            editorData.gameName = clientData.gameName;
            editorData.gameID = clientData.gameID;
            editorData.version = clientData.version;
            editorData.iconPath = clientData.iconPath;
            editorData.preloadPath = clientData.preloadPath;
            editorData.windowsName = clientData.windowsName;
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            editorData.platform = EditorGUILayout.TextField("平台名称:", editorData.platform);
            editorData.gameName = EditorGUILayout.TextField("游戏名称:", editorData.gameName);
            editorData.gameID = EditorGUILayout.TextField("游戏ID:", editorData.gameID);
            editorData.version = EditorGUILayout.TextField("版本号:", editorData.version);
            editorData.iconPath = EditorGUILayout.TextField("icon路劲:", editorData.iconPath);
            editorData.preloadPath = EditorGUILayout.TextField("主场景路径:", editorData.preloadPath);
            editorData.windowsName = EditorGUILayout.TextField("pc名称:", editorData.windowsName);
            editorData.androidName = EditorGUILayout.TextField("Android名称:", editorData.androidName);

            if (!addOre)
            {
                if (GUILayout.Button("保存配置"))
                {
                    clientData = editorData;
                    ShowNotification(new GUIContent("保存成功:" + clientData.gameName + "+" + clientData.gameID + "+" + clientData.version));
                }
            }
            else
            {
                if (GUILayout.Button("增加平台"))
                {
                    bool succeed = EditorMainWindow.AddClient(clientData);
                    if (succeed)
                    {
                        ShowNotification(new GUIContent("添加成功:" + clientData.gameName + "+" + clientData.gameID + "+" + clientData.version));
                    }
                    else
                    {
                        ShowNotification(new GUIContent("添加失败!已有平台:" + clientData.platform));
                    }
                }
            }
            EditorGUILayout.EndVertical();

        }
    }
}