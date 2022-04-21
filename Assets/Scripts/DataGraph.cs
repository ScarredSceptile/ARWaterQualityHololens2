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

    public void SetPoints(float[] points)
    {
        _points = points;
    }

    public void SetLineColor(Color color)
    {
        _lineRenderer.startColor = _lineRenderer.endColor = color;
    }

    public float GetMaxNumber()
    {
        return _points.Max();
    }
    
    public float GetMinNumber()
    {
        return _points.Min();
    }

    public void GenerateGraph(int range, bool pos, bool neg)
    {
        int amount = _points.Length;
        Vector3[] points = new Vector3[amount];

        for (int i = 0; i < amount; i++)
        {
            float clampedValue;
            if (pos)
                clampedValue = _points[i] / range * YModifier - 0.5f;
            else if (neg)
                clampedValue = _points[i] / -range * YModifier + 0.5f;
            else
             clampedValue = _points[i] / range * YModifier;

            float xPoint = ((float)i / (float)amount * XModifier) - (XModifier / 2);
            points[i] = new Vector3(xPoint, clampedValue, ZValue);
        }
        _lineRenderer.positionCount = amount;
        _lineRenderer.SetPositions(points);
    }

    /*
     int amount = _points.Length;
        Vector3[] points = new Vector3[amount];

        for (int i = 0; i < amount; i++)
        {
            float clampedValue = _points[i] / range * YModifier;
            float xPoint = ((float)i / (float)amount * XModifier) - (XModifier / 2);
            points[i] = new Vector3(xPoint, clampedValue, ZValue);
        }
        _lineRenderer.positionCount = amount;
        _lineRenderer.SetPositions(points);
    */
}
