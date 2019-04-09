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
namespace K.LocalWork
{
    [CustomEditor(typeof(Main))]
    public class MainEditor : Editor
    {
        public override void OnInspectorGUI()
        {
           base.OnInspectorGUI();
            Main a = (Main)target;
            a.type = (PlatType)EditorGUILayout.EnumPopup(a.type);
            switch (a.type)
            {
                case PlatType.Plat_HuaErJie:
                    a.skin = (ESkin)EditorGUILayout.EnumPopup(a.skin);
                    break;
                case PlatType.Plat_123:
                    a.skin996 = (ESkin996)EditorGUILayout.EnumPopup(a.skin996);
                    break;
                case PlatType.Plat_MG:
                    break;
                case PlatType.HuaErJie_BoWei:
                    break;
                case PlatType.HuaErJie_XianWan:
                    break;
                case PlatType.DaCaiShen:
                    break;
                default:
                    break;
            }
            // EditorGUILayout.PropertyField(a.type);
        }
    }
}
