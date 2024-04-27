using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuchunLibrary
{
    public static class Library// : MonoBehaviour
    {
        /// <summary>
        /// int配列の中で一番初めのnumberのindexを返します
        /// </summary>
        /// <param name="ints"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int SearchNumberIndex(int[] ints, int number)
        {
            for (int i = 0; i < ints.Length; i++) if (ints[i] == number) return i;
            return -1;
        }

        /// <summary>
        /// bool配列の一番最初に現れたfalseの位置を返します、全部trueだったら-1を返します
        /// </summary>
        /// <param name="bools"></param>
        /// <returns></returns>
        public static int FirstFalseIndex(bool[] bools)
        {
            for (int i = 0; i < bools.Length; i++) if (!bools[i]) return i;
            return -1;
        }

        /// <summary>
        /// 特性関数、bool配列に対してTrueの数を返します
        /// </summary>
        /// <param name="bools"></param>
        /// <returns></returns>
        public static int CharacteristicFanction(bool[] bools)
        {
            int count = 0;
            for (int i = 0; i < bools.Length; i++) if (bools[i]) count++;
            return count;
        }

        /// <summary>
        /// 整数の下二桁を二文字のstring型で返す
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string LastTwoDigits(int a)
        {
            int reminder = a % 100;

            if (a > 9) return reminder.ToString();
            else if (a >= 0) return "0" + reminder.ToString();
            else return "";
        }
    }


//missionコンポーネントで自分がCustomEditorやPropertyDrawerを使って頑張った功績をここに残しておく、
//お疲れ自分

// #if UNITY_EDITOR
//     [CanEditMultipleObjects]
//     [CustomEditor(typeof(Mission), true)]
//     public class MissionEditor : Editor
//     {
//         public Mission mission;

//         private SerializedProperty _missionGroupDatas;


//         // private SerializedProperty[] _throughCurrentValueProperties;

//         // private SerializedProperty _id;
//         // private SerializedProperty _currentValue;
//         // private SerializedProperty _goalValue;
//         // private SerializedProperty _missionState;

//         private void OnEnable()
//         {
//             mission = target as Mission;

//             _missionGroupDatas = serializedObject.FindProperty("missionGroupDatas");

//             // _missionGroupDatas= serializedObject.FindProperty("missionGroupDatas");

//             // _throughCurrentValueProperties = new SerializedProperty[mission.missionGroupDatas.Count()];
//             // for (int i = 0; i < _throughCurrentValueProperties.Length; i++)
//             // {
//             //     // _throughCurrentValueProperties[i]= mission.missionGroupDatas[i].throughCurrentValue;
//             // }

//             // _id = serializedObject.FindProperty("id");
//             // _currentValue = serializedObject.FindProperty("currentValue");
//             // _goalValue = serializedObject.FindProperty("goalValue");
//             // _missionState = serializedObject.FindProperty("goalValue");
//         }

//         public override void OnInspectorGUI()
//         {
//             // EditorGUILayout.PropertyField(_missionGroupDatas);

//             base.OnInspectorGUI();


//             // serializedObject.Update();

//             // for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
//             // {
//             //     switch (mission.missionGroupDatas[i].missionType)
//             //     {
//             //         case MissionType.Through:
//             //             // EditorGUILayout.IntField("currentValue", mission.missionGroupDatas[i].throughCurrentValue);
//             //             EditorGUILayout.HelpBox("表示するテキスト", MessageType.Info);
//             //             break;
//             //     }
//             // }
//             // EditorGUILayout.HelpBox("表示するテキスト", MessageType.Info);

//             // serializedObject.ApplyModifiedProperties();
//         }
//     }
// #endif

}

