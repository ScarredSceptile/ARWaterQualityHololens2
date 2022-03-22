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

    public float PositionDistance;
    private float _positionDistance;

    private Color[] colors = { Color.green, Color.red, Color.magenta, Color.white, Color.black, Color.cyan };

    // Start is called before the first frame update
    void Start()
    {
        _dataIndicators = new List<DataIndicator>();
        int range = 100;
        _positionDistance = PositionDistance;

        for (int i = 0; i < colors.Length; i++)
        {
            var obj = Instantiate(_lineObject, transform);
            var ind = Instantiate(_indicatorObject, transform);
            ind.transform.localPosition = new Vector3(0.645f, 0.464f - i * _positionDistance, 0f);
            var graph = obj.GetComponent<DataGraph>();
            var indicator = ind.GetComponent<DataIndicator>();
            graph.SetLineColor(colors[i]);
            indicator.SetColor(colors[i]);
            var points = RandomPoints(30, -range * (i + 1), range * (i + 1));
            graph.SetPoints(points);
            var name = RandomString();
            indicator.graph = graph;
            indicator.SetName(name);
            indicator.SetManager(this);
            _dataIndicators.Add(indicator);
        }

        var minValue = _dataIndicators.Min(n => n.graph.GetMinNumber());
        var maxValue = _dataIndicators.Max(n => n.graph.GetMaxNumber());

        var max = RoundNumber(maxValue);
        var min = RoundNumber(minValue);

        int graphRange = Mathf.Max(Mathf.Abs(max), Mathf.Abs(min)) * 2;
        _maxText.text = (graphRange / 2).ToString();
        _minText.text = (-graphRange / 2).ToString();
        _dataIndicators.ForEach(n => n.graph.GenerateGraph(graphRange));
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
    }

    /// <summary>
    /// Creates x random points to simulate a graph
    /// </summary>
    private float[] RandomPoints(int amount, float min, float max)
    {
        float[] points = new float[amount];
        for (int i = 0; i < amount; i++)
        {
            points[i] = Random.Range(min, max);
        }
        return points;
    }

    private int RoundNumber(float number)
    {
        var negativ = (int)Mathf.Sign(number);
        var posNum = (int)Mathf.Abs(number);
        var scale = (int)Mathf.Pow(10, Mathf.Floor(Mathf.Log10(posNum)));
        return negativ * scale * (int)Mathf.Round(posNum / scale + 1);
    }


    private static string RandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[Random.Range(0,s.Length)]).ToArray());
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
