using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Marker : BaseEyeFocusHandler
{
    public double Longitude;
    public double Latitude;
    public float DistanceFromPlayer;
    public string Name;
    public float[] Values;
    [SerializeField]
    private GameObject _stationInfo;
    private GameObject _spawnedInfo;
    private MarkerInfo _markerInfo;

    protected override void OnEyeFocusStart()
    {
        StartLookAt();
    }

    protected override void OnEyeFocusStop()
    {
        EndLookAt();
    }

    public void StartLookAt()
    {
        if (_spawnedInfo != null)
            return;
        _spawnedInfo = Instantiate(_stationInfo);
        _markerInfo = _spawnedInfo.GetComponent<MarkerInfo>();
        _markerInfo.UpdateNameAndStatus(Name, "Test string");
    }

    public void EndLookAt()
    {
        if (_spawnedInfo == null)
            return;
        Destroy(_spawnedInfo);
    }
}
