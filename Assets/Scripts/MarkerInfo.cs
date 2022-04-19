using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarkerInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameStatus;
    [SerializeField]
    private TextMeshProUGUI _values;

    public void UpdateNameAndStatus(string name, string status)
    {
        _nameStatus.text = $"Name: {name}\nStatus: {status}";
    }

    public void UpdateValues(string Ph, string conductivity, string turbidity, string watermm, string waterpoly)
    {
        _values.text = $"{Ph}\n{conductivity}µS/cm\n{turbidity}NTU\n{watermm}mm\n{waterpoly}N2000";
    }
}
