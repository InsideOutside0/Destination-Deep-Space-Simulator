using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotActions : MonoBehaviour
{

    public int cargoLevel;
    public int panelLevel;
    public bool hasVision;
    public bool hasRamp;
    public bool hasSideAuto;
    public bool hasCenterAuto;

    public float lowCargoAcc;
    public float medCargoAcc;
    public float highCargoAcc;
    public float lowPanelAcc;
    public float medPanelAcc;
    public float highPanelAcc;
    public float sideAutoAccMod;
    public float centerAutoAccMod;

    public float lowCargoSpeed;
    public float medCargoSpeed;
    public float highCargoSpeed;
    public float lowPanelSpeed;
    public float medPanelSpeed;
    public float highPanelSpeed;
    public float sideAutoSpeedMod;
    public float centerAutoSpeedMod;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerEnter(Collision col)
    {
        if (col.gameObject.name.Equals("TestBall"))
        {
            col.gameObject.transform.parent = transform;
        }
    }*/
}
