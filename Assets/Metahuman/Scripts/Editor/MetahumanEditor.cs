using UnityEditor;
using UnityEngine;

// Class required to have Scale / Skin Tone render due to a bug in Unity
[CustomEditor(typeof(Metahuman))]
public class MetahumanEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var mh = (Metahuman)target;

        if (GUILayout.Button("Update"))
        {
            mh.skinTone.SetDirty(true);
            mh.skinTone.OnValueChanged?.Invoke(mh.skinTone.Value, mh.skinTone.Value);
            mh.hairColor.SetDirty(true);
            mh.hairColor.OnValueChanged?.Invoke(mh.hairColor.Value, mh.hairColor.Value);
            mh.outfitIndex.SetDirty(true);
            mh.outfitIndex.OnValueChanged?.Invoke(mh.outfitIndex.Value, mh.outfitIndex.Value);
            mh.hairIndex.SetDirty(true);
            mh.hairIndex.OnValueChanged?.Invoke(mh.hairIndex.Value, mh.hairIndex.Value);
        }
        if (GUILayout.Button("Preview Colors"))
        {
            mh.PreviewColors();
        }
    }
}
