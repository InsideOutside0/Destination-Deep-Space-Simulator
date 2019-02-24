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

    public GameController gameController;
    public bool pressedAction;
    public bool pressedMid;
    public bool pressedHigh;
    public GameController.Action currentAction;

    // Start is called before the first frame update
    void Start()
    {
        currentAction = GameController.Action.None;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        pressedAction = false;
        pressedMid = false;
        pressedHigh = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        float actionInput = 0;
        float midInput = 0;
        float highInput = 0;
        string controllerName = transform.GetComponent<MoveBot>().controllerName;
        switch (controllerName)
        {
            case "WASD":
                actionInput = Input.GetAxisRaw("WASD-Action");
                midInput = Input.GetAxisRaw("WASD-Mid");
                highInput = Input.GetAxisRaw("WASD-High");
                break;
            case "ArrowKeys":
                actionInput = Input.GetAxisRaw("ArrowKeys-Action");
                midInput = Input.GetAxisRaw("ArrowKeys-Mid");
                highInput = Input.GetAxisRaw("ArrowKeys-High");
                break;
            case "Joy1":
                actionInput = Input.GetAxisRaw("Joy1-Action");
                midInput = Input.GetAxisRaw("Joy1-Mid");
                highInput = Input.GetAxisRaw("Joy1-High");
                break;
            case "Joy2":
                actionInput = Input.GetAxisRaw("Joy2-Action");
                midInput = Input.GetAxisRaw("Joy2-Mid");
                highInput = Input.GetAxisRaw("Joy2-High");
                break;
            case "Joy3":
                actionInput = Input.GetAxisRaw("Joy3-Action");
                midInput = Input.GetAxisRaw("Joy3-Mid");
                highInput = Input.GetAxisRaw("Joy3-High");
                break;
            case "Joy4":
                actionInput = Input.GetAxisRaw("Joy4-Action");
                midInput = Input.GetAxisRaw("Joy4-Mid");
                highInput = Input.GetAxisRaw("Joy4-High");
                break;
            default: break;
        }
        if (actionInput >= 1) pressedAction = true;
        if (midInput >= 1) pressedMid = true;
        if (highInput >= 1) pressedHigh = true;
    }

    public void StartAction(GameController.Action action)
    {

    }

}
