using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

/// <summary> これがアタッチされているオブジェクトをヒエラルキーに投げる。 </summary>
public class MachomanUser : MonoBehaviour
{
    public enum FragmentationMode
    {
        WithoutPhysics,
        WithPhysics,
    }

    public enum CuttingMode
    {
        Manual,
        Automatic,
    }

    [SerializeField] private FragmentationMode _fragmentationMode;
    [SerializeField] private CuttingMode _cuttingMode;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Material _capMaterial;
    [SerializeField] private GameObject _victimObject;
    [SerializeField] private GameObject _planeObject;
    [SerializeField] private string _objectName = "Cutted Mesh";

    public FragmentationMode FragMode => _fragmentationMode;
    public CuttingMode CutMode => _cuttingMode;

    private Vector3 _planeNormal;
    private float _deltaAngle;
    private Random _random = new Random();
    private List<GameObject> _cuttedMeshes = new List<GameObject>();

    public void CheckDirectory() // 保存パスが存在するか確認する
    {
        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesFolderAbsolutePath + $"{_objectName}/");
        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesPrefabFolderAbsolutePath);
    }

    public void CutMesh() // メッシュのカットを実施する
    {
        if (_victimObject is null) return;

        MachomanHelper.CutTheMesh(_victimObject, _cuttedMeshes, Vector3.zero, _planeNormal, _capMaterial);
    }

    public void SaveCuttedMeshes() // 保存先のパスにメッシュのアセットとプレハブを保存する
    {
        if (_cuttedMeshes.Count < 1) return;

        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesFolderAbsolutePath + $"{_objectName}/");
        MachoMan.FindSaveTargetDirectory(MachoMan.CuttedMeshesPrefabFolderAbsolutePath);

        _cuttedMeshes[0].name = _objectName;

        // カットしたメッシュは一つのオブジェクトにする
        for (int i = 1; i < _cuttedMeshes.Count; ++i)
        {
            _cuttedMeshes[i].transform.parent = _cuttedMeshes[0].transform;
        }

        // コンポーネントのアタッチ
        if (_fragmentationMode == FragmentationMode.WithPhysics)
        {
            foreach (var cuttedMesh in _cuttedMeshes)
            {
                cuttedMesh.AddComponent<MeshCollider>();
                cuttedMesh.AddComponent<Rigidbody>();
            }
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

    public void RunFragmentation()
    {
        Debug.Log("まだ実装してない");
    }

    private void Start()
    {
        _planeNormal = _planeObject.transform.up;
    }

    private void Update()
    {
        _planeNormal = _planeObject.transform.up;

        var inputHor = Input.GetAxis("Horizontal");
        var inputVer = Input.GetAxis("Vertical");

        if (inputHor < 0) // 左
        {
            _planeObject.transform.Rotate(Vector3.forward, Time.deltaTime * _rotateSpeed);
        }
        else if (inputHor > 0) // 右
        {
            _planeObject.transform.Rotate(Vector3.forward, -Time.deltaTime * _rotateSpeed);
        }

        if (inputVer > 0) // 下
        {
            _planeObject.transform.Rotate(Vector3.right, Time.deltaTime * _rotateSpeed);
        }
        else if (inputVer < 0) // 上
        {
            _planeObject.transform.Rotate(Vector3.right, -Time.deltaTime * _rotateSpeed);
        }
    }
}