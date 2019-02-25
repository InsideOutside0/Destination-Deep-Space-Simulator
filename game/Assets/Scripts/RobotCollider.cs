using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCollider : MonoBehaviour
{

    public MoveBot m;

    // Start is called before the first frame update
    void Start()
    {
        m = transform.parent.GetComponent<MoveBot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) // this is where all the magic happens, baby
    {
        Transform o = other.transform;
        if (m.pressedAction)
        {
            if (m.pressedMid)
            {
                if (m.ballID > -1) // has a ball
                {

                }
                if (m.panelID > -1)
                {

                }
            }
        }
    }
}
