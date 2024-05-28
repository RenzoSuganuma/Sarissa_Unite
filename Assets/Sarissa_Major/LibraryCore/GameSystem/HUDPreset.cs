using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HUDPreset_gen", menuName = "CreateHUDPreset", order = 10)]
public class HUDPreset : ScriptableObject
{
    public List<GameObject> HUDList;
}
