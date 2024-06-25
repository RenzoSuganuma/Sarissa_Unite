using UnityEditor;
using UnityEngine;

/// <summary> MachomanUserのインスペクタを拡張する。ボタンの追加をする </summary>
[CustomEditor(typeof(ArmStrongUser))]
public class ArmStrongEditor : Editor // MachoManクラスを拡張する
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ArmStrongUser user = target as ArmStrongUser;

        switch (user.CutMode)
        {
            case ArmStrongUser.CuttingMode.Manual:
            {
                if (GUILayout.Button("Cut Meshes"))
                {
                    user.CutMesh();
                }

                break;
            }

            case ArmStrongUser.CuttingMode.Automatic:
            {
                if (GUILayout.Button("Execute Fragmentation"))
                {
                    user.RunFragmentation();
                }

                break;
            }
        }

        if (GUILayout.Button("Check Directory"))
        {
            user.CheckDirectory();
        }

        if (GUILayout.Button("Save Meshes"))
        {
            user.SaveCuttedMeshes();
        }
    }
}
