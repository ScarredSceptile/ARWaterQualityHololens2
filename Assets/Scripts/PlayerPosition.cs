using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private double Latitude;
    private double Longitude;

    // Start is called before the first frame update
    void Start()
    {
        //Temp values
        Latitude = 60.7897;
        Longitude = -10.6822;
    }

    // Update is called once per frame
    void Update()
    {
        MarkerManager.Instance.ManageMarkers(Latitude, Longitude, transform.position);
    }
}
