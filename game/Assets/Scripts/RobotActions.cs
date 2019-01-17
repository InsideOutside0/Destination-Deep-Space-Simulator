using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotActions : MonoBehaviour
{

    private string cargoLevel;
    private string panelLevel;
    private bool hasVision;
    private bool hasRamp;
    private bool hasSideAuto;
    private bool hasCenterAuto;

    private float lowCargoAcc;
    private float medCargoAcc;
    private float highCargoAcc;
    private float lowPanelAcc;
    private float medPanelAcc;
    private float highPanelAcc;
    private float sideAutoAccMod;
    private float centerAutoAccMod;

    private float lowCargoSpeed;
    private float medCargoSpeed;
    private float highCargoSpeed;
    private float lowPanelSpeed;
    private float medPanelSpeed;
    private float highPanelSpeed;
    private float sideAutoSpeedMod;
    private float centerAutoSpeedMod;



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
