using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBot : MonoBehaviour {

    public float rotationSpeed = 0.1f;
    public float acceleration = 1f;
    public float slowdown = 1.5f; // I refuse to call it "deceleration"
    public float velocityCap = 3f;
    public int id;

    private Rigidbody2D rb; // this is what handles velocity and collisions and such

    public string controllerName = "";
    private float hMove;
    private float vMove;
    public int started;
    public int inAction;
    private Vector2 velocityVector;
    private float velocity;
    private float angle = 0f;
    private bool goingForward = false;

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
    public int ballID;
    public int panelID;
    public GameController.Action currentAction;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // you cannot use a RigidBody without accessing it on the object first
        started = 0;
        inAction = 1;
        currentAction = GameController.Action.None;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        pressedAction = false;
        pressedMid = false;
        pressedHigh = false;
    }

    // I hate using six controllers
    void GetInputs()
    {
        float actionInput = 0;
        float midInput = 0;
        float highInput = 0;
        if (controllerName == "WASD") {
            hMove = Input.GetAxisRaw("WASD-H");
            vMove = Input.GetAxisRaw("WASD-V");
            actionInput = Input.GetAxisRaw("WASD-Action");
            midInput = Input.GetAxisRaw("WASD-Mid");
            highInput = Input.GetAxisRaw("WASD-High");
        } else if (controllerName == "ArrowKeys") {
            hMove = Input.GetAxisRaw("ArrowKeys-H");
            vMove = Input.GetAxisRaw("ArrowKeys-V");
            actionInput = Input.GetAxisRaw("ArrowKeys-Action");
            midInput = Input.GetAxisRaw("ArrowKeys-Mid");
            highInput = Input.GetAxisRaw("ArrowKeys-High");
        } else if (controllerName == "Joy1") {
            hMove = Input.GetAxisRaw("Joy1-H");
            vMove = Input.GetAxisRaw("Joy1-V");
            actionInput = Input.GetAxisRaw("Joy1-Action");
            midInput = Input.GetAxisRaw("Joy1-Mid");
            highInput = Input.GetAxisRaw("Joy1-High");
        } else if (controllerName == "Joy2") {
            hMove = Input.GetAxisRaw("Joy2-H");
            vMove = Input.GetAxisRaw("Joy2-V");
            actionInput = Input.GetAxisRaw("Joy2-Action");
            midInput = Input.GetAxisRaw("Joy2-Mid");
            highInput = Input.GetAxisRaw("Joy2-High");
        } else if (controllerName == "Joy3") {
            hMove = Input.GetAxisRaw("Joy3-H");
            vMove = Input.GetAxisRaw("Joy3-V");
            actionInput = Input.GetAxisRaw("Joy3-Action");
            midInput = Input.GetAxisRaw("Joy3-Mid");
            highInput = Input.GetAxisRaw("Joy3-High");
        } else if (controllerName == "Joy4") {
            hMove = Input.GetAxisRaw("Joy4-H");
            vMove = Input.GetAxisRaw("Joy4-V");
            actionInput = Input.GetAxisRaw("Joy4-Action");
            midInput = Input.GetAxisRaw("Joy4-Mid");
            highInput = Input.GetAxisRaw("Joy4-High");
        }
        if (actionInput >= 1) pressedAction = true;
        if (midInput >= 1) pressedMid = true;
        if (highInput >= 1) pressedHigh = true;
        angle = transform.rotation.eulerAngles.z; // the z angle is the effective angle in a 2D space
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    // Called less frequently than Update()
    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    // Unity uses degrees. That is a disgrace, though I understand the decision.
    private float ToRadians(float num)
    {
        return num * Mathf.PI / 180;
    }

    // this function ensures that a number is within a given range
    private float Clamp(float num, float min, float max)
    {
        if (num > max) return max;
        else if (num < min) return min;
        return num;
    }

    // currently only doing WestCoast Drive

    private void Move() // uses vertical input
    {
        if (vMove>0) { // up input, forward
            // use the bot's angle to determine movemennt direction
            goingForward = true;
            if (velocity<velocityCap) {
                velocity += acceleration;
                velocity = Clamp(velocity, 0, velocityCap);
            }
            velocityVector = new Vector2(velocity*Mathf.Cos(ToRadians(angle)), velocity*Mathf.Sin(ToRadians(angle)));

        } else if (vMove<0) { // down input, reverse

            if (goingForward) { // slow down
                velocity -= slowdown;
                velocity = Clamp(velocity, 0f, velocityCap);
                if (velocity == 0) goingForward = false;
                velocityVector = new Vector2(velocity * Mathf.Cos(ToRadians(angle)), velocity * Mathf.Sin(ToRadians(angle)));
            } else { // then reverse
                velocity += acceleration;
                velocity = Clamp(velocity, 0f, velocityCap);
                velocityVector = new Vector2(-velocity * Mathf.Cos(ToRadians(angle)), -velocity * Mathf.Sin(ToRadians(angle)));
            }

        } else { // no input

            velocity -= slowdown;
            velocity = Clamp(velocity, 0, velocityCap);
            if (velocity == 0) goingForward = false;
            if (goingForward)
                velocityVector = new Vector2(velocity * Mathf.Cos(ToRadians(angle)), velocity * Mathf.Sin(ToRadians(angle)));
            else
                velocityVector = new Vector2(-velocity * Mathf.Cos(ToRadians(angle)), -velocity * Mathf.Sin(ToRadians(angle)));

        }

        rb.velocity = velocityVector*started*inAction;
    }

    private void Rotate() // uses horizontal input
    {
        // Vector3.forward = (0, 0, 1)
        // transform.Rotate rotates BY the angle of the parameter, not TO the angle
        rb.angularVelocity = 0; // could have used it to rotate, but whatever
        transform.Rotate(inAction * started * hMove * Vector3.forward * -rotationSpeed);
            
    }

    public void StartAction(GameController.Action action)
    {

    }


}
