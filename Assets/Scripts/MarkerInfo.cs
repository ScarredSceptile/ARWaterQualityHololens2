using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarkerInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameStatus;

    public void UpdateNameAndStatus(string name, string status)
    {
        _nameStatus.text = $"Name: {name}\nStatus: {status}";
    }
}
