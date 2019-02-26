using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProperties : MonoBehaviour
{
    public int id;
    public int[] cargoOccupied = 
    {
        -1, -1, -1, // left
        -1, -1, -1, // right
    };
    public int[] panelOccupied = 
    {
        -1, -1, -1, // left
        -1, -1, -1, // right
    };
}
