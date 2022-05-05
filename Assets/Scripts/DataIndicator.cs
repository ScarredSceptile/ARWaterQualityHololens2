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
    private GraphManager _graphManager;

    private bool _holdRunning = false;
    private bool _isShowing = true;
    private bool _inCountdown = false;

    private const float HoldSeconds = 1;

    public void SetName(string name)
    {
        _name.text = name;
    }

    public string GetName()
    {
        return _name.text;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void SetManager(GraphManager manager) { _graphManager = manager; }

    public void ToggleGraph()
    {
        if (graph != null)
        {
            graph.gameObject.SetActive(!graph.gameObject.activeSelf);
            _isShowing = graph.gameObject.activeSelf;
        }
    }

    public void HideLine()
    {
        graph.gameObject.SetActive(false);
    }

    public void ShowLine()
    {
        graph.gameObject.SetActive(_isShowing);
    }

    public void HoldBegin()
    {
        Debug.Log("Hold Started");
        StartCoroutine(HoldingButton());
    }

    public void HoldEnd()
    {
        Debug.Log("Hold Ended");
        if (_inCountdown)
            StopAllCoroutines();
        else if (_holdRunning)
            _holdRunning = false;
    }

    private IEnumerator HoldingButton()
    {
        float time = 0;
        _holdRunning = true;
        _inCountdown = true;
        while (true)
        {
            time += Time.deltaTime;
            yield return null;
            if (!_holdRunning)
                yield break;
            if (time > HoldSeconds)
                break;
        }
        _inCountdown = false;
        graph.gameObject.SetActive(true);
        _isShowing = true;
        _graphManager.HideOtherLines(this);
        while (true)
        {
            if (!_holdRunning)
            {
                _graphManager.ShowLines();
                break;
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        Destroy(graph.gameObject);
    }
}
