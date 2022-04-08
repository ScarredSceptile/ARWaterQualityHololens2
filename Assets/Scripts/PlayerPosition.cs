using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public double Latitude;
    public double Longitude;
    private double _prevLatitude;
    private double _prevLongitude;

    [SerializeField]
    private MarkerManager _markerManager;

    private void Awake()
    {
        _markerManager.SetPlayerPosition(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Temp values
        Latitude = 60.7897;
        Longitude = -10.6822;
        _markerManager.SetPlayerPosition(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_prevLatitude != Latitude || _prevLongitude != Longitude)
        {
            _markerManager.ManageMarkers();
            _prevLongitude = Longitude;
            _prevLatitude = Latitude;
        }
    }
}
