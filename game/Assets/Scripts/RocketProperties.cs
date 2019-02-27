using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProperties : MonoBehaviour
{
    public int id;
    public int[] cargoOccupied = 
    {
        -1, -1, // bottom
        -1, -1, // mid
        -1, -1, // top
    };
    public int[] panelOccupied = 
    {
        -1, -1, // bottom
        -1, -1, // mid
        -1, -1, // top
    };
}
