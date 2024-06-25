using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MachomanUser))]
public class MachoManEditor : Editor // MachoManクラスを拡張する
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MachomanUser user = target as MachomanUser;

        if (GUILayout.Button("Check Directry"))
        {
            user.CheckDirectory();
        }
        
        if (GUILayout.Button("Save Meshes"))
        {
            user.SaveCuttedMeshes();
        }
    }
}
