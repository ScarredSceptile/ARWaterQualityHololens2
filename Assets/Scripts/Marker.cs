using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        {
            _spawnedInfo.gameObject.SetActive(true);
            return;
        }
        _spawnedInfo = Instantiate(_stationInfo);
        _spawnedInfo.transform.position = transform.position;
        _spawnedInfo.transform.LookAt(2 * transform.position - _player.transform.position);
        _markerInfo = _spawnedInfo.GetComponent<MarkerInfo>();
        _markerInfo.UpdateNameAndStatus(Name, "Test string");
        if (WaterQualityList != null)
        {
            var values = WaterQualityList.Skip(Mathf.Max(0, WaterQualityList.Count() - 12));
            var ph = values.Select(n => double.Parse(n.pH)).Average();
            var conduc = values.Select(n => double.Parse(n.Conductivity)).Average();
            var turbidity = values.Select(n => double.Parse(n.Turbidity)).Average();
            var watermm = values.Select(n => n.WaterLevelMm).Average();
            var waterpoly = values.Select(n => double.Parse(n.WaterLevelPolynomial)).Average();
            _markerInfo.UpdateValues(ph.ToString("N2"), conduc.ToString("N2"), turbidity.ToString("N2"), watermm.ToString("N2"), waterpoly.ToString("N2"));
        }
    }

    public void EndLookAt()
    {
        if (_spawnedInfo == null)
            return;
        Destroy(_spawnedInfo);
    }

    public void RemoveSpawned()
    {
        _spawnedInfo = null;
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        StartLookAt();
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        //EndLookAt();
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
