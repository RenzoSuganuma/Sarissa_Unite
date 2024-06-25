using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MachomanUser : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Material _capMaterial;
    [SerializeField] private GameObject _victimObject;
    [SerializeField] private GameObject _planeObject;
    [SerializeField] private string _objectName = "Cutted Mesh";

    private Vector3 _planeNormal;
    private List<GameObject> _cuttedMeshes = new List<GameObject>();

    public void CheckDirectory()
    {
        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesFolderAbsolutePath + $"{_objectName}/");
        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesPrefabFolderAbsolutePath);
    }

    public void SaveCuttedMeshes()
    {
        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesFolderAbsolutePath + $"{_objectName}/");
        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesPrefabFolderAbsolutePath);

        _cuttedMeshes[0].name = _objectName;

        // カットしたメッシュは一つのオブジェクトにする
        for (int i = 1; i < _cuttedMeshes.Count; ++i)
        {
            _cuttedMeshes[i].transform.parent = _cuttedMeshes[0].transform;
        }

        // 保存処理
        for (int i = 0; i < _cuttedMeshes.Count; ++i)
        {
            var mesh = _cuttedMeshes[i].GetComponent<MeshFilter>().mesh;

            AssetDatabase.CreateAsset(mesh,
                MachoMan.CuttedMeshesFolderAbsolutePath + $"{_objectName}/{mesh.name}_{i}.asset");
        }

        PrefabUtility.SaveAsPrefabAsset(_cuttedMeshes[0],
            MachoMan.CuttedMeshesPrefabFolderAbsolutePath + $"{_objectName}.prefab");
    }

    private void Start()
    {
        _planeNormal = _planeObject.transform.up;
    }

    private void Update()
    {
        _planeNormal = _planeObject.transform.up;

        var input = Input.GetAxis("Horizontal");

        if (input < 0) // 左
        {
            _planeObject.transform.Rotate(Vector3.forward, Time.deltaTime * _rotateSpeed);
        }
        else if (input > 0) // 右
        {
            _planeObject.transform.Rotate(Vector3.forward, -Time.deltaTime * _rotateSpeed);
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(500, 700, 200, 100), "CUT"))
        {
            MachomanHelper.CutTheMesh(_victimObject, _cuttedMeshes, Vector3.zero, _planeNormal, _capMaterial);
        }
    }
}