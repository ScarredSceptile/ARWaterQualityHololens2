using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _cube;
    [SerializeField]
    private int _distanceMultiplier;
    [SerializeField]
    private float _distance;
    [SerializeField]
    private PinchSlider _slider;
    private GameObject _primitive;
    private bool _isSpawning = false;
    private Transform _cameraTransform;
    private Vector3 _position = Vector3.zero;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        _primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _primitive.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _primitive.SetActive(false);
    }

    public void StartSpawn()
    {
        _primitive.SetActive(true);
        _isSpawning = true;
    }

    public void EndSpawn()
    {
        _primitive.SetActive(false);
        _isSpawning = false;
    }

    public void SpawnCube()
    {
        GameObject.Instantiate(_cube, _position, Quaternion.identity);
        EndSpawn();
    }

    private void Update()
    {
        _distance = _slider.SliderValue;
        if (_isSpawning is false)
            return;

        _position = _cameraTransform.position + _cameraTransform.forward * _distanceMultiplier * _distance;
        _primitive.transform.position = _position;
    }
}
