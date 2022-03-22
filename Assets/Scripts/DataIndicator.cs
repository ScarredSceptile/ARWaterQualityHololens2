using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataIndicator : MonoBehaviour
{
    public DataGraph graph;
    [SerializeField]
    private TextMeshPro _name;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void ToggleGraph()
    {
        if (graph != null)
            graph.gameObject.SetActive(!graph.gameObject.activeSelf);
    }
}
