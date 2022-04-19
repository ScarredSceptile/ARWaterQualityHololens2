using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    private const double KmCon = 110.574;

    //Blue, Green, Red, Yellow
    [SerializeField]
    private List<GameObject> _markers = new List<GameObject>();
    [SerializeField]
    private PinchSlider _graphSlider;
    [SerializeField]
    private PinchSlider _settingsSlider;
    private List<Marker> _spawnedMarkers = new List<Marker>();

    public float MarkerDistance;
    public float SpawnDistance;


    //Temp variables for spawning markers
    public double TempLatitude;
    public double TempLongitude;
    public string TempName;
    System.Random random = new System.Random();
    public TextAsset jsonFile;

    private PlayerPosition _playerPos;

    private void Start()
    {
        var qual = JsonUtility.FromJson<WaterQualities>(jsonFile.text);

        var values1 = RandomPoints(50, -50, 50);
        var values2 = RandomPoints(50, -50, 50);
        var values3 = RandomPoints(50, -50, 50);
        var values4 = RandomPoints(50, -50, 50);
        SpawnMarker(60.76, -10.683, "Marker North", values1, qual);
        SpawnMarker(60.81, -10.683, "Marker South", values2, null);
        SpawnMarker(60.789, -10.7, "Marker East", values3, null);
        SpawnMarker(60.789, -10.66, "Marker West", values4, null);
    }

    public void UpdateDistance(string Slider)
    {
        if (Slider == "Graph")
        {
            MarkerDistance = _graphSlider.SliderValue * 29 + 1;
            if (_settingsSlider.SliderValue != _graphSlider.SliderValue)
                _settingsSlider.SliderValue = _graphSlider.SliderValue;
        }
        else if (Slider == "Settings")
        {
            MarkerDistance = _settingsSlider.SliderValue * 29 + 1;
            if (_settingsSlider.SliderValue != _graphSlider.SliderValue)
                _graphSlider.SliderValue = _settingsSlider.SliderValue;
        }
        ManageMarkers();
    }

    public void SetPlayerPosition(PlayerPosition playerPos)
    {
        _playerPos = playerPos;
    }

    public void SpawnMarker(double latitude, double longitutde, string name, float[] values, WaterQualities qual)
    {
        var objec = _markers[random.Next(_markers.Count())];

        var obj = Instantiate(objec, transform);
        var marker = obj.GetComponent<Marker>();
        if (qual != null)
            marker.WaterQualityList = qual.WaterQuality;
        marker.SetPlayer(_playerPos.gameObject);
        marker.Latitude = latitude;
        marker.Longitude = longitutde;
        marker.Name = name;
        marker.Values = values;

        var (dist, markerPos, origin) = GetDistanceFromPlayer(marker.Latitude, marker.Longitude);
        marker.DistanceFromPlayer = (float)dist;
        CheckMarkerDist(marker, markerPos, origin);

        _spawnedMarkers.Add(marker);
    }

    public void ManageMarkers()
    {
        foreach (var marker in _spawnedMarkers)
        {
            var (dist, markerPos, origin) = GetDistanceFromPlayer(marker.Latitude, marker.Longitude);
            marker.DistanceFromPlayer = (float)dist;
            CheckMarkerDist(marker, markerPos, origin);
        }
    }

    private void CheckMarkerDist(Marker marker, Vector2 markerpos, Vector2 origin)
    {
        if (marker.DistanceFromPlayer < MarkerDistance)
        {
            marker.gameObject.SetActive(true);
            markerpos -= origin;
            markerpos.Normalize();
            var pos = (Vector2)_playerPos.transform.position + markerpos * SpawnDistance;
            marker.transform.position = new Vector3(pos.x, 0, pos.y);
        }
        else
            marker.gameObject.SetActive(false);
    }

    public (double dist, Vector2 markerpos, Vector2 origin) GetDistanceFromPlayer(double Latitude, double Longitude)
    {
        var markerpos = new Vector2((float)Latitude, (float)Longitude);
        var origin = new Vector2((float)_playerPos.Latitude, (float)_playerPos.Longitude);

        return (Vector2.Distance(origin, markerpos) * KmCon, markerpos, origin);
    }

    public List<Marker> GetMarkerData()
    {
        var availableMarkers = _spawnedMarkers.Where(m => m.gameObject.activeSelf).ToList();
        availableMarkers = availableMarkers.OrderBy(m => m.DistanceFromPlayer).ToList();
        return availableMarkers;
    }

    /// <summary>
    /// Creates x random points to simulate a graph
    /// </summary>
    public float[] RandomPoints(int amount, float min, float max)
    {
        float[] points = new float[amount];
        for (int i = 0; i < amount; i++)
        {
            points[i] = Random.Range(min, max);
        }
        return points;
    }

    private List<WaterQuality> DataFromJson(string json)
    {
        return JsonUtility.FromJson<List<WaterQuality>>(json);
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MarkerManager), true)]
    public class UiElementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var manager = target as MarkerManager;
            var lat = manager.TempLatitude;
            var lon = manager.TempLongitude;
            var name = manager.TempName;

            DrawDefaultInspector();
            if (GUILayout.Button("Spawn Marker"))
            {
                var values = manager.RandomPoints(50, -50, 50);
                manager.SpawnMarker(lat, lon, name, values, null);
            }
        }
    }

#endif

}
