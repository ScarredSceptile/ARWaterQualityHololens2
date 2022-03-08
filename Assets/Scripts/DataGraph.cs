using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DataGraph : MonoBehaviour
{
    private const float ZValue = -0.01f;
    private const float XModifier = 0.96f;
    private const float YModifier = 0.94f;

    [SerializeField]
    private LineRenderer _lineRenderer;
    private float[] _points;

    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomPoints();
        GenerateGraph();
    }

    /// <summary>
    /// Creates x random points to simulate a graph
    /// </summary>
    private void RandomPoints()
    {
        int amount = 50;
        _points = new float[amount];
        for (int i = 0; i < amount; i++)
        {
            _points[i] = Random.Range(-500f, 500f);
        }
    }

    private void GenerateGraph()
    {
        int amount = _points.Length;
        float maxValue = _points.Max();
        float minValue = _points.Min();
        float range = Mathf.Max(Mathf.Abs(maxValue), Mathf.Abs(minValue)) * 2;
        Vector3[] points = new Vector3[amount];

        for (int i = 0; i < amount; i++)
        {
            float clampedValue = _points[i] / range * YModifier;
            float xPoint = ((float)i / (float)amount * XModifier) - (XModifier / 2);
            points[i] = new Vector3(xPoint, clampedValue, ZValue);
        }
        _lineRenderer.positionCount = amount;
        _lineRenderer.SetPositions(points);
    }
}
