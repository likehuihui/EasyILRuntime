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
    public class EditorDLL : EditorBase
    {

        public static void Open()
        {
            EditorDLL ab = EditorWindow.GetWindow<EditorDLL>("编辑窗口", true);
            ab.minSize = new Vector2(640, 360);
            ab.maxSize = new Vector2(1280, 720);
            ab.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("保存配置"))
            {
                //SaveConfig();
            }
            if (GUILayout.Button("发布DLL"))
            {
                //Apply();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        private void BuildReleaseDLL()
        {

        }
    }
}