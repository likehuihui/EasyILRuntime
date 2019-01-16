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
    public class EditorClientWindow : EditorWindow
    {
        public static ClientData clientData;
        /// <summary>
        /// 是否是新增加平台
        /// </summary>
        private static bool addOre;
 
        public static void Open(ClientData data,bool addOrEditor)
        {
            addOre = addOrEditor;
            //clientData = new ClientData();
            //clientData.platform = data.platform;
            //clientData.gameName = data.gameName;
            //clientData.gameID = data.gameID;
            //clientData.version = data.version;
            //clientData.iconPath = data.iconPath;
            //clientData.preloadPath = data.preloadPath;
            //clientData.windowsName = data.windowsName;
            //clientData.androidName = data.androidName;

             clientData = data;
            EditorClientWindow ab = EditorWindow.GetWindow<EditorClientWindow>("平台数据", true);
            ab.minSize = new Vector2(640, 360);
            ab.maxSize = new Vector2(1280, 720);
            ab.Show();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            clientData.platform = EditorGUILayout.TextField("平台名称:", clientData.platform);
            clientData.gameName = EditorGUILayout.TextField("游戏名称:", clientData.gameName);
            clientData.gameID = EditorGUILayout.TextField("游戏ID:", clientData.gameID);
            clientData.version = EditorGUILayout.TextField("版本号:", clientData.version);
            clientData.iconPath = EditorGUILayout.TextField("icon路劲:", clientData.iconPath);
            clientData.preloadPath = EditorGUILayout.TextField("主场景路径:", clientData.preloadPath);
            clientData.windowsName = EditorGUILayout.TextField("pc名称:", clientData.windowsName);
            clientData.androidName = EditorGUILayout.TextField("Android名称:", clientData.androidName);
        
            if (!addOre)
            {
                if (GUILayout.Button("保存信息"))
                {
                    EditorMainWindow.SaveData(clientData);
                    ShowNotification(new GUIContent("保存成功:" + clientData.gameName + "+" + clientData.gameID + "+" + clientData.version));
                }
            }
            else
            {
                if (GUILayout.Button("增加平台"))
                {
                    bool succeed=  EditorMainWindow.AddClient(clientData);
                    if (succeed)
                    {
                        ShowNotification(new GUIContent("添加成功:" + clientData.gameName + "+" + clientData.gameID + "+" + clientData.version));
                    }
                    else
                    {
                        ShowNotification(new GUIContent("添加失败!已有平台:"+ clientData.platform));
                    }
                }
            }
            EditorGUILayout.EndVertical();

        }
    }
}