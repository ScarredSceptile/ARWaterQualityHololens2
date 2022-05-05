using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaterQuality
{
    public string Localtime;
    public string pH;
    public string Conductivity;
    public string Turbidity;
    public int WaterLevelMm;
    public string WaterLevelPolynomial;
}
