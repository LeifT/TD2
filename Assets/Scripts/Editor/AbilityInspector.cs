using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ISpell), true)]
public class AbilityInspector : Editor {

    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();
        //Draw some buttons or something.
    }

    //public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int textWidth, int textHeight) {
    //    Texture2D staticPrev = new Texture2D(textWidth, textHeight);

    //    ISpell spell = target as ISpell;

    //    if (spell == null || spell.Icon == null) {
    //        return null;
    //    }

    //    //EditorUtility.CopySerialized(spell.Icon, staticPrev);

    //    return staticPrev;
    //}
}
