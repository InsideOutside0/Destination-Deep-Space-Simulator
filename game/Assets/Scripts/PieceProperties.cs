using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceProperties : MonoBehaviour
{
    public int id;
    public bool active;
    public bool onRobot;
    public bool scored;

    private void Update()
    {
        if (onRobot) transform.position = transform.parent.position;
    }
}
