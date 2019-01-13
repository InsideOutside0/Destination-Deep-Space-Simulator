using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBot : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float slowdown = 1.5f; // I refuse to call it "deceleration"
    [SerializeField] private float velocitCap = 3f;

    private Rigidbody2D rb; // this is what handles velocity and collisions and such

    private string controllerName = "WASD";
    private float hMove;
    private float vMove;
    private Vector2 velocityVector;
    private float velocity;
    private float angle = 0f;
    private bool goingForward = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // you cannot use a RigidBody without accessing it on the object first
    }

    // I hate using six controllers
    void getInputs()
    {
        if (controllerName == "WASD") {
            hMove = Input.GetAxisRaw("WASD-H");
            vMove = Input.GetAxisRaw("WASD-V");
        } else if (controllerName == "ArrowKeys") {
            hMove = Input.GetAxisRaw("ArrowKeys-H");
            vMove = Input.GetAxisRaw("ArrowKeys-V");
        } else if (controllerName == "Joy1") {
            hMove = Input.GetAxisRaw("Joy1-H");
            vMove = Input.GetAxisRaw("Joy1-V");
        } else if (controllerName == "Joy2") {
            hMove = Input.GetAxisRaw("Joy2-H");
            vMove = Input.GetAxisRaw("Joy2-V");
        } else if (controllerName == "Joy3") {
            hMove = Input.GetAxisRaw("Joy3-H");
            vMove = Input.GetAxisRaw("Joy3-V");
        } else if (controllerName == "Joy4") {
            hMove = Input.GetAxisRaw("Joy4-H");
            vMove = Input.GetAxisRaw("Joy4-V");
        }
        angle = transform.rotation.eulerAngles.z; // the z angle is the effective angle in a 2D space
    }

    // Update is called once per frame
    void Update()
    {
        getInputs();
    }

    // Unity uses degrees. That is a disgrace, though I understand the descision.
    private float toRadians(float num)
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
        if (vMove>0) { // up input
            // use the bot's angle to determine movemennt direction
            goingForward = true;
            if (velocity<velocitCap) {
                velocity += acceleration;
                velocity = Clamp(velocity, 0, velocitCap);
            }
            velocityVector = new Vector2(velocity*Mathf.Cos(toRadians(angle)), velocity*Mathf.Sin(toRadians(angle)));

        } else if (vMove<0) {

            if (goingForward) {
                velocity -= slowdown;
                velocity = Clamp(velocity, 0f, velocitCap);
                if (velocity == 0) goingForward = false;
            } else {
                velocity += acceleration;
                velocity = Clamp(velocity, 0f, velocitCap);
                velocityVector = new Vector2(-velocity * Mathf.Cos(toRadians(angle)), -velocity * Mathf.Sin(toRadians(angle)));
            }

        } else {

            velocity -= slowdown;
            velocity = Clamp(velocity, 0, velocitCap);
            if (velocity == 0) goingForward = false;
            if (goingForward)
                velocityVector = new Vector2(velocity * Mathf.Cos(toRadians(angle)), velocity * Mathf.Sin(toRadians(angle)));
            else
                velocityVector = new Vector2(-velocity * Mathf.Cos(toRadians(angle)), -velocity * Mathf.Sin(toRadians(angle)));

        }

        rb.velocity = velocityVector;
    }

    private void Rotate() // uses horizontal input
    {
            // Vector3.forward = (0, 0, 1)
            // transform.Rotate rotates BY the angle of the parameter, not TO the angle
            transform.Rotate(hMove * Vector3.forward * -rotationSpeed);
  
    }

    // Called less frequently than Update()
    private void FixedUpdate()
    {
        Rotate();
        Move();
    }
}
