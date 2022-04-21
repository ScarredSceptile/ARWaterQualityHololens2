using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    [SerializeField]
    private List<DataIndicator> _dataIndicators;
    [SerializeField]
    private GameObject _lineObject;
    [SerializeField]
    private GameObject _indicatorObject;
    [SerializeField]
    private TextMeshPro _maxText;
    [SerializeField]
    private TextMeshPro _minText;
    [SerializeField]
    private TextMeshPro _curShowing;
    [SerializeField]
    private MarkerManager _markerManager;

    public float PositionDistance;
    private float _positionDistance;

    private Color[] colors = { Color.green, Color.red, Color.magenta, Color.white, Color.black, Color.cyan };

    private enum States { Ph, Conductivity, Turbidity, WaterLevelMm, WaterLevelPoly };
    private States _states = States.Ph;

    // Start is called before the first frame update
    void Start()
    {
        _dataIndicators = new List<DataIndicator>();
        _positionDistance = PositionDistance;
    }

    private void Update()
    {
        if (_positionDistance != PositionDistance)
        {
            _positionDistance = PositionDistance;
            for (int i = 0; i < _dataIndicators.Count; i++)
            {
                _dataIndicators[i].transform.localPosition = new Vector3(0.645f, 0.464f - i * _positionDistance, 0f);
            }
        }
        ManageMarkers();
    }

    public void ChangeShowing(string value)
    {
        switch (value)
        {
            case "Ph": _states = States.Ph;
                _curShowing.text = "Currently Showing: Ph";
                break;
            case "Conductivity": _states = States.Conductivity;
                _curShowing.text = "Currently Showing: Conductivity";
                break;
            case "Turbidity": _states = States.Turbidity;
                _curShowing.text = "Currently Showing: Turbidity";
                break;
            case "WaterLevelMm": _states = States.WaterLevelMm;
                _curShowing.text = "Currently Showing: Water Level in mm";
                break;
            case "WaterLevelPoly": _states = States.WaterLevelPoly;
                _curShowing.text = "Currently Showing: Water Level Polynomial";
                break;
            default: _states = States.Ph;
                _curShowing.text = "Currently Showing: Ph";
                break;
        }
        var markers = _markerManager.GetMarkerData();
        markers = markers.GetRange(0, Mathf.Min(colors.Length, markers.Count));
        GenerateGraphs(markers);
    }

    private void ManageMarkers()
    {
        var markers = _markerManager.GetMarkerData();
        markers = markers.GetRange(0, Mathf.Min(colors.Length, markers.Count));
        foreach (var marker in markers)
        {
            if (_dataIndicators.Where(d => d.GetName() == marker.Name).Count() == 0 || _dataIndicators.Count != markers.Count())
            {
                Debug.Log("Change in markers Noticed!");
                GenerateGraphs(markers);
                return;
            }
        }
    }

    private void GenerateGraphs(List<Marker> markers)
    {
        for (int i = _dataIndicators.Count - 1; i >= 0; i--)
        {
            Destroy(_dataIndicators[i].gameObject);
            _dataIndicators.RemoveAt(i);
        }

        int j = 0;
        foreach (var marker in markers)
        {
            var obj = Instantiate(_lineObject, transform);
            var ind = Instantiate(_indicatorObject, transform);
            ind.transform.localPosition = new Vector3(0.645f, 0.464f - j * _positionDistance, 0f);
            var graph = obj.GetComponent<DataGraph>();
            var indicator = ind.GetComponent<DataIndicator>();
            graph.SetLineColor(colors[j]);
            indicator.SetColor(colors[j]);
            var values = GetCurrentValues(marker);
            graph.SetPoints(values);
            var name = marker.Name;
            indicator.graph = graph;
            indicator.SetName(name);
            indicator.SetManager(this);
            _dataIndicators.Add(indicator);
            j++;
        }

        var minValue = _dataIndicators.Min(n => n.graph.GetMinNumber());
        var maxValue = _dataIndicators.Max(n => n.graph.GetMaxNumber());

        var max = RoundNumber(maxValue);
        var min = RoundNumber(minValue);

        int graphRange;
        bool pos, neg;
        if (max < 0)
        {
            graphRange = min;
            pos = false;
            neg = true;
            _maxText.text = "0";
            _minText.text = (graphRange).ToString();

        }
        else if (min > 0)
        {
            graphRange = max;
            pos = true;
            neg = false;
            _maxText.text = (graphRange).ToString();
            _minText.text = "0";
        }
        else
        {
            graphRange = Mathf.Max(Mathf.Abs(max), Mathf.Abs(min)) * 2;
            pos = true;
            neg = true;
            _maxText.text = (graphRange / 2).ToString();
            _minText.text = (-graphRange / 2).ToString();
        }

        _dataIndicators.ForEach(n => n.graph.GenerateGraph(graphRange, pos, neg));
    }

    private float[] GetCurrentValues(Marker marker)
    {
        //Returns based on what the current State is
        if (_states == States.Ph)
            return marker.WaterQualityList.Where(n => n.pH != "").Select(n => float.Parse(n.pH)).ToArray();
        if (_states == States.Conductivity)
            return marker.WaterQualityList.Where(n => n.Conductivity != "").Select(n => float.Parse(n.Conductivity)).ToArray();
        if (_states == States.Turbidity)
            return marker.WaterQualityList.Where(n => n.Turbidity != "").Select(n => float.Parse(n.Turbidity)).ToArray();
        if (_states == States.WaterLevelMm)
            return marker.WaterQualityList.Select(n => (float)n.WaterLevelMm).ToArray();
        return marker.WaterQualityList.Where(n => n.WaterLevelPolynomial != "").Select(n => float.Parse(n.WaterLevelPolynomial)).ToArray();

    }

    private int RoundNumber(float number)
    {
        var negativ = (int)Mathf.Sign(number);
        var posNum = (int)Mathf.Abs(number);
        var scale = (int)Mathf.Pow(10, Mathf.Floor(Mathf.Log10(posNum)));
        return negativ * scale * (int)Mathf.Round(posNum / scale + 1);
    }

    public void HideOtherLines(DataIndicator origin)
    {
        _dataIndicators.Where(d => d != origin).ToList().ForEach(d => d.HideLine());
    }

    public void ShowLines()
    {
        _dataIndicators.ForEach(d => d.ShowLine());
    }
}
