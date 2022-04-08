using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public double Longitude;
    public double Latitude;
    public float DistanceFromPlayer;
    public string Name;
    public float[] Values;
    private SerializedObject halo;

    //Temp for color changing
    public enum ColorEnum { Green, Blue, Red, Yellow };
    public ColorEnum color;
    private ColorEnum curColor;

    private void Awake()
    {
        halo = new SerializedObject(GetComponent("Halo"));
        color = ColorEnum.Red;
        curColor = color;
    }

    private void Update()
    {
        if (curColor != color)
        {
            curColor = color;
            switch (color)
            {
                case ColorEnum.Green: SetHaloColor(Color.green); break;
                case ColorEnum.Blue: SetHaloColor(Color.blue); break;
                case ColorEnum.Red: SetHaloColor(Color.red); break;
                case ColorEnum.Yellow: SetHaloColor(Color.yellow); break;
                default: SetHaloColor(Color.black); break;
            }
        }
    }

    public void SetHaloColor(Color color)
    {
        halo.FindProperty("m_Color").colorValue = color;
        halo.ApplyModifiedProperties();
    }
}
