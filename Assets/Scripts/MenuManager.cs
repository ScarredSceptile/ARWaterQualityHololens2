using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _showMoreMenu;
    [SerializeField]
    private GameObject _compareMenu;
    [SerializeField]
    private GameObject _settingsMenu;

    public void ToggleShowMore()
    {
        _showMoreMenu.SetActive(!_showMoreMenu.activeSelf);
        _compareMenu.SetActive(false);
        _settingsMenu.SetActive(false);
    }

    public void ToggleCompare()
    {
        _showMoreMenu.SetActive(false);
        _compareMenu.SetActive(!_compareMenu.activeSelf);
        _settingsMenu.SetActive(false);
    }

    public void ToggleSettings()
    {
        _showMoreMenu.SetActive(false);
        _compareMenu.SetActive(false);
        _settingsMenu.SetActive(!_settingsMenu.activeSelf);
    }
}
