using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public static MarkerManager Instance { get; private set; }

    private const double KmCon = 110.574;

    [SerializeField]
    private GameObject _marker;
    private List<Marker> _markers = new List<Marker>();

    public float MarkerDistance;
    public float SpawnDistance;

    //Temp variables for spawning markers
    public double TempLatitude;
    public double TempLongitude;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SpawnMarker(double latitude, double longitutde)
    {
        var obj = Instantiate(_marker, transform);
        var marker = obj.GetComponent<Marker>();
        marker.Latitude = latitude;
        marker.Longitude = longitutde;
        _markers.Add(marker);
    }

    public void ManageMarkers(double latitude, double longitude, Vector2 originPos)
    {
        foreach (var marker in _markers)
        {
            var markerpos = new Vector2((float)marker.Latitude, (float)marker.Longitude);
            var origin = new Vector2((float)latitude, (float)longitude);
            
            var dist = Vector2.Distance(origin, markerpos) * KmCon;
            if (dist < MarkerDistance)
            {
                marker.gameObject.SetActive(true);
                markerpos -= origin;
                markerpos.Normalize();
                var pos = originPos + markerpos * SpawnDistance;
                marker.transform.position = new Vector3(pos.x, 0, pos.y);
            }
            else
                marker.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [CustomEditor(typeof(MarkerManager), true)]
    public class UiElementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var manager = target as MarkerManager;
            var lat = manager.TempLatitude;
            var lon = manager.TempLongitude;

            DrawDefaultInspector();
            if (GUILayout.Button("Spawn Marker"))
            {
                manager.SpawnMarker(lat, lon);
            }
        }
    }
}
