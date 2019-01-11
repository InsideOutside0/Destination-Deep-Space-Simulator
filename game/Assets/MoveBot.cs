using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBot : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float slowdown = 1.5f; // I refuse to call it "deceleration"
    [SerializeField] private float velocitCap = 3f;

    private Rigidbody2D rb; // this is what handles velocity and collisions and such

    private float hMove;
    private float vMove;
    private Vector2 velocityVector;
    private float velocity;
    private float angle = 0f;
    private bool goingForward = false;


    // Start is called before the first frame update
    void Start()
    {
        velocityVector = new Vector2();
        rb = GetComponent<Rigidbody2D>(); // you cannot use a RigidBody without initializing it first
    }

    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");
        angle = transform.rotation.eulerAngles.z; // the z angle is the effective angle in a 2D space
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
            velocityVector = new Vector2(velocity * Mathf.Cos(toRadians(angle)), velocity * Mathf.Sin(toRadians(angle)));

        }

        rb.velocity = velocityVector;
    }

    // this function ensures that a number is within a given range
    private float Clamp(float num, float min, float max)
    {
        if (num > max) return max;
        else if (num < min) return min;
        return num;
    }

    // Unity uses degrees. That is a disgrace, though I understand the descision.
    private float toRadians(float num) {
        return num*Mathf.PI/180;
    }

    private void Rotate() // uses horizontal input
    {
        if (hMove>0) // right input
        {
            // Vector3.forward = (0, 0, 1)
            // transform.Rotate rotates BY the angle of the parameter, not TO the angle
            transform.Rotate(Vector3.forward * -rotationSpeed);
        }
        else if (hMove<0) // left input
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
    }

    // Called less frequently than Update()
    private void FixedUpdate()
    {
        Rotate();
        Move();
    }
}
