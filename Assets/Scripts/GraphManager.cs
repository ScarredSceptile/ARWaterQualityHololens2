using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    [SerializeField]
    private List<DataGraph> _dataGraphs;
    [SerializeField]
    private GameObject _lineObject;
    [SerializeField]
    private TextMeshPro _maxText;
    [SerializeField]
    private TextMeshPro _minText;

    private Color[] colors = { Color.green, Color.red, Color.magenta, Color.white, Color.black, Color.cyan };

    // Start is called before the first frame update
    void Start()
    {
        _dataGraphs = new List<DataGraph>();
        int range = 100;

        for (int i = 0; i < colors.Length; i++)
        {
            var obj = Instantiate(_lineObject, transform);
            var graph = obj.GetComponent<DataGraph>();
            graph.SetLineColor(colors[i]);
            var points = RandomPoints(30, -range * (i + 1), range * (i + 1));
            graph.SetPoints(points);
            _dataGraphs.Add(graph);
        }

        var minValue = _dataGraphs.Min(n => n.GetMinNumber());
        var maxValue = _dataGraphs.Max(n => n.GetMaxNumber());

        var max = RoundNumber(maxValue);
        var min = RoundNumber(minValue);

        int graphRange = Mathf.Max(Mathf.Abs(max), Mathf.Abs(min)) * 2;
        _maxText.text = (graphRange / 2).ToString();
        _minText.text = (-graphRange / 2).ToString();
        _dataGraphs.ForEach(n => n.GenerateGraph(graphRange));
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
}
