using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour, IMixedRealityFocusHandler, IMixedRealityPointerHandler
{
    public double Longitude;
    public double Latitude;
    public float DistanceFromPlayer;
    public string Name;
    public float[] Values;
    public List<WaterQuality> WaterQualityList;
    private GameObject _player;
    [SerializeField]
    private GameObject _stationInfo;
    private GameObject _spawnedInfo;
    private MarkerInfo _markerInfo;

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    public void StartLookAt()
    {
        if (_spawnedInfo != null)
            return;
        _spawnedInfo = Instantiate(_stationInfo);
        _spawnedInfo.transform.position = transform.position;
        _spawnedInfo.transform.LookAt(2 * transform.position - _player.transform.position);
        _markerInfo = _spawnedInfo.GetComponent<MarkerInfo>();
        _markerInfo.UpdateNameAndStatus(Name, "Test string");
    }

    public void EndLookAt()
    {
        if (_spawnedInfo == null)
            return;
        Destroy(_spawnedInfo);
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        StartLookAt();
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        EndLookAt();
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
