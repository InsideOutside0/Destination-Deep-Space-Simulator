using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotActions : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collision col)
    {
        if (col.gameObject.name.Equals("TestBall"))
        {
            col.gameObject.transform.parent = transform;
        }
    }
}
