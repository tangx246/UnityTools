using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopdownCameraController))]
public class TopdownCameraControllerEditor : Editor
{
    public GameObject boundsTarget;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var cameraControls = (TopdownCameraController)target;

        boundsTarget = (GameObject)EditorGUILayout.ObjectField("Bounds Target", boundsTarget, typeof(GameObject), true);
        if (GUILayout.Button("Set Bounds"))
        {
            cameraControls.SetBounds(boundsTarget);
        }
    }
}