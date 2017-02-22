using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEditor;

public class AbilityAssetScript : Editor
{

    [MenuItem("Tools/Regenerate Ability Scripts")]
    static void RegenerateAbilityAsset()//(Type abilityType)
    {
        string path = Application.dataPath + "/Scripts/Abilities/";
        string[] abilitiesFull = Directory.GetFiles(path);

        int x = 0;
        foreach (string str in abilitiesFull) {
            string filename = str.Substring(str.LastIndexOf("/") + 1);

            if (filename.EndsWith(".cs")) {
                string meh = filename.Remove(filename.Length - 3);

                var abilityType = meh;
                var ability = ScriptableObject.CreateInstance(abilityType);
                AssetDatabase.CreateAsset(ability, "Assets/Ability/" + abilityType + ".asset");
                AssetDatabase.SaveAssets();

                Debug.Log(meh);
                x++;
            }
        }
    }
}