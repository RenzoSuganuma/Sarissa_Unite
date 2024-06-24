using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RydenUser : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Material _capMaterial;
    [SerializeField] private GameObject _victimObject;
    [SerializeField] private GameObject _planeObject;

    private Vector3 _planeNormal;
    private List<GameObject> _cuttedMeshes = new List<GameObject>();

    private void Start()
    {
        _planeNormal = _planeObject.transform.up;

        var result = Ryden.CutMesh(_victimObject, Vector3.zero, _planeNormal, _capMaterial, true);
        _cuttedMeshes.InsertRange(_cuttedMeshes.Count > 0 ? _cuttedMeshes.Count - 1 : 0, result);
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
        if (GUI.Button(new Rect(10, 700, 100, 50), "CUT"))
        {
            CutTheMesh();
        }
    }

    private void CutTheMesh()
    {
        List<GameObject> results = new List<GameObject>();

        foreach (var mesh in _cuttedMeshes)
        {
            var result = Ryden.CutMesh(mesh, Vector3.zero, _planeNormal, _capMaterial, true);
            results.InsertRange(results.Count > 0 ? results.Count - 1 : 0, result);
        }
        
        _cuttedMeshes.InsertRange(_cuttedMeshes.Count > 0 ? _cuttedMeshes.Count - 1 : 0, results);
    }
}
