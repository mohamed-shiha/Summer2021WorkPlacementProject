using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement_Prototype : MonoBehaviour
{

    float yVelocity = 0f;
    [Range(5f, 25f)]
    public float gravity = 15f;
    //the speed of the player movement
    [Range(5f, 15f)]
    public float movementSpeed = 10f;
    //jump speed
    [Range(5f, 15f)]
    public float jumpSpeed = 10f;

    //now the camera so we can move it up and down
    Transform cameraTransform;
    float pitch = 0f;
    [Range(1f, 90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 2f;

    //the character component to move the player
    CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        Look();
        Move();

    }

    void Look()
    {
        //get the mouse input axis values
        float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
        //turn the whole object based on the x input
        transform.Rotate(0, xInput, 0);
        //now add on y input to pitch, and clamp it
        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        //create the local rotation value for the camera and set it
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }

    void Move()
    {
        //update speed based on the input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        //transform it based off the player transform and scale it by movement speed
        Vector3 move = transform.TransformVector(input) * movementSpeed;

        //is it on the ground
        if (cc.isGrounded)
        {
            yVelocity = -gravity * Time.deltaTime;
            //check for jump here
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }
        }
        //add the gravity to the Y velocity
        yVelocity -= gravity * Time.deltaTime;
        move.y = yVelocity;

        //Apply movement
        cc.Move(move * Time.deltaTime);
    }
}
