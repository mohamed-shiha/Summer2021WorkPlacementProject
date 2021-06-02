using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : NetworkBehaviour
{
    Transform cameraTransform;
    Canvas HUD;
    CharacterController cc;
    PlayerData PlayerData;
    PlayerAttack PlayerAttack;
    Health PlayerHealth;

    public Joystick LeftJoystick;
    public Joystick RightJoystick;
    public MobileTouchButton JumpButton;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        HUD = GetComponentInChildren<Canvas>();
        PlayerData = GetComponent<PlayerData>();
        PlayerAttack = GetComponent<PlayerAttack>();
        cc = GetComponent<CharacterController>();
        PlayerHealth = GetComponent<Health>();

        HUD.gameObject.SetActive(false);
        PlayerHealth.PlayerOutOfHealth += PlayerOutOfHealth;
        PlayerData.OnStateChanged += OnStateChanged;
        PlayerData.State = PlayerState.Lobby;

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            RightJoystick.gameObject.SetActive(false);
            LeftJoystick.gameObject.SetActive(false);
        }
    }

    private void PlayerOutOfHealth()
    {
        //Debug.Log("FPC : player has no health ");
        PlayerData.State = PlayerState.Dead;
        //RespawnClientRpc(Vector3.zero);
    }

    public void OnStateChanged(PlayerState oldState, PlayerState newState)
    {
        //Debug.Log("State changed "+newState);
        switch (newState)
        {
            case PlayerState.Lobby:
                cameraTransform.GetComponent<AudioListener>().enabled = false;
                cameraTransform.GetComponent<Camera>().enabled = false;
                HUD.gameObject.SetActive(false);
                PlayerData.PlayMode = false;
                PlayerAttack.PlayMode = false;
                break;
            case PlayerState.InGame:
                if (IsLocalPlayer)
                {
                    cameraTransform.GetComponent<AudioListener>().enabled = true;
                    cameraTransform.GetComponent<Camera>().enabled = true;
                }
                HUD.gameObject.SetActive(true);
                PlayerData.PlayMode = true;
                PlayerAttack.PlayMode = true;
                // Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                break;
            case PlayerState.Dead:
                SpawnManager.Instance.GiveNewLocationAfterDeath(gameObject);
                break;
            case PlayerState.PreGame:
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer)
            return;

        if (PlayerData.PlayMode)
        {
            Look();
            Move();

        }
    }

    void Look()
    {
        //get the mouse input axis values
        float xInput = 0, yInput = 0;
        //xInput = Input.GetAxis("Mouse X") * PlayerData.mouseSensitivity;
        //yInput = Input.GetAxis("Mouse Y") * PlayerData.mouseSensitivity;
      if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            //Debug.Log("In Windows");
            xInput = Input.GetAxis("Mouse X") * PlayerData.mouseSensitivity;
            yInput = Input.GetAxis("Mouse Y") * PlayerData.mouseSensitivity;

        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.Log("In Android");
            xInput = RightJoystick.Horizontal * PlayerData.mouseSensitivity;
            yInput = RightJoystick.Vertical * PlayerData.mouseSensitivity;
        }
        //turn the whole object based on the x input
        transform.Rotate(0, xInput, 0);
        //now add on y input to pitch, and clamp it
        PlayerData.pitch -= yInput;
        PlayerData.pitch = Mathf.Clamp(PlayerData.pitch, PlayerData.minPitch, PlayerData.maxPitch);
        //create the local rotation value for the camera and set it
        Quaternion rot = Quaternion.Euler(PlayerData.pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }

    void Move()
    {
        //update speed based on the input
        Vector3 input = Vector3.zero;
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            input = new Vector3(LeftJoystick.Horizontal, 0, LeftJoystick.Vertical);
        }
        //input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        //transform it based off the player transform and scale it by movement speed
        Vector3 move = transform.TransformVector(input) * PlayerData.movementSpeed;

        // play animation 
        //animationController.StartWalkingAnimation(input.z != 0);

        //is it on the ground
        if (cc.isGrounded)
        {
            PlayerData.yVelocity = -PlayerData.gravity * Time.deltaTime;
            //check for jump here
            if (Input.GetButtonDown("Jump") || JumpButton.pressed)
            {
                PlayerData.yVelocity = PlayerData.jumpSpeed;
            }
        }
        //add the gravity to the Y velocity
        PlayerData.yVelocity -= PlayerData.gravity * Time.deltaTime;
        move.y = PlayerData.yVelocity;

        //Apply movement
        cc.Move(move * Time.deltaTime);
    }


    public void Respawn(Vector3 newPos)
    {
        if (IsLocalPlayer)
            RespawnServerRpc(newPos);
        //RespawnClientRpc(newPos);
    }

    [ServerRpc]
    public void RespawnServerRpc(Vector3 newPos)
    {
        RespawnClientRpc(newPos);
    }

    [ClientRpc]
    public void RespawnClientRpc(Vector3 newPos)
    {
        //Debug.Log("clientRPC: new location to the client");
        cc.enabled = false;
        transform.position = newPos;
        cc.enabled = true;
    }

}
