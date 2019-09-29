/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace K.Editors
{
    public class EditorAssetBundleWin : Editor
    {



        private void OnGUI()
        {
           // EditorGUILayout.BeginVertical();
           // selectIndex = EditorGUILayout.Popup("平台选择:",selectIndex, platName);
            //if (GUILayout.Button("增加平台"))
            //{

            //}
           // Debug.Log(selectIndex);
           // EditorGUILayout.EnumPopup("选择平台",)
            //GUILayout.Space(100);
            //  prefab=(GameObject)EditorGUI.ObjectField(new Rect(0, 40, 500, 20), "编辑的模型", prefab, typeof(GameObject));
            // icon = (Texture2D)EditorGUILayout.ObjectField("Icon", icon, typeof(Texture2D), false);
            // GUILayout.Space(100);
            EditorGUILayout.EndHorizontal();
            // SerializedProperty sp=null;
            //  GUI.DrawTexture(new Rect(100,100,100,50), icon);
            // EditorGUILayout.PropertyField(sp);
            if (GUILayout.Button("保存信息"))
            {

            }
        }
    }
}