using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Objects Attached to Player")]
    public PictureManager PicManager;
    public Camera playerCamera;
    public GameObject picobj;

    [Header("Character Stats")]
    public float speedModifier;
    public float jumpSpeed = 30f;
    public float characterVelocityY;
    public float SpeedOnGround = 10f;
    public float mouseSensitivity = 1f;
    public float gravityDownForce = 60f;
    public float sprintSpeedModifier = 2f;
    public float slowSpeedModifier = 0.5f;
    public float SpeedInAir = 25f;
    public float movementSharpnessOnGround = 15;

    public bool isGrounded { get; private set; }
    public bool isSprinting { get; private set; }
    public Vector3 characterVelocity { get; set; }

    CharacterController characterController;
    float LastTimeJumped = 0f;
    float cameraVerticalAngle;
    float m_TargetCharacterHeight;
    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.07f;


   

    private void Awake()
    {
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
        PicManager = transform.GetComponent<PictureManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        if(GetSprintInputHeld() && !GetSlowInputHeld())        //Set the speed modifier for Player's Velocity
            speedModifier = sprintSpeedModifier; 
        else if(!GetSprintInputHeld() && GetSlowInputHeld())
            speedModifier = slowSpeedModifier;
        else{
            speedModifier = 1f;
        }

        CharacterLook();
        CharacterMovement();
        GetObjectInteractionInput();
        //Debug.Log(Screen.height);
    }

    private void CharacterLook()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");
        // Rotate the player transform left right      
        transform.Rotate(new Vector3(0f,lookX * mouseSensitivity, 0f), Space.Self);
        cameraVerticalAngle += lookY * mouseSensitivity;

        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f); // Limit the camera's vertical angle. Prevent the player from turning 180 degree

        // Change the vertical angle of the camera
        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0); // We only want the camera to rotate up/down when the player looks up/down, not the whole player object

    }

    private void CharacterMovement()
    {
        CharacterLook();
        Vector3 worldspaceMoveInput = transform.TransformVector(GetMoveInput());    // Get player's movement input
        if (GroundCheck())                                                             //Check if the player is on the ground
        {
            Vector3 TargetVelocity = worldspaceMoveInput * SpeedOnGround * speedModifier;
            // Get the Player's Velocity
            characterVelocity = Vector3.Lerp(characterVelocity, TargetVelocity, movementSharpnessOnGround * Time.deltaTime);    // By slowly changing player's velocity to the target velocity, we are creating the moving animation 
            if (GroundCheck() && Input.GetKeyDown(KeyCode.Space))                                                                  // If the player isGrounded and he/she hits the jump button, the player will jump
            {
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);     // Set the player's VelocityY to 0 first
                characterVelocity += Vector3.up * jumpSpeed;                                       // Then, add the jumpSpeed value                                                                                                                       // Force grounding to false
            }
        }
        else
        {   /* New Method
            Vector3 TargetVelocity = worldspaceMoveInput * SpeedOnGround * speedModifier;
            TargetVelocity.y = characterVelocity.y;
            characterVelocity = Vector3.Lerp(characterVelocity, TargetVelocity, movementSharpnessOnGround * Time.deltaTime);
            */
            characterVelocity += worldspaceMoveInput * SpeedInAir * Time.deltaTime;             //Old Method
            characterVelocityY = characterVelocity.y;                                           // As I'm not allowed to edit the Character's Velocity Y itself, I have create a whole new variable.
            characterVelocityY += (gravityDownForce * Time.deltaTime) * -1;
            characterVelocity = new Vector3(characterVelocity.x, characterVelocityY, characterVelocity.z);
        }

        //Debug.Log(characterVelocityY);
        characterController.Move(characterVelocity * Time.deltaTime);   //Move the player
    }



    public bool GroundCheck()
    {
       return(characterController.isGrounded); // reset values before the ground check
    }
    public bool GetSprintInputHeld()
    {
        return Input.GetButton("Sprint");
    }
    public bool GetSlowInputHeld()
    {
        return Input.GetButton("Slow");
    }
    public Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); 
        move = Vector3.ClampMagnitude(move, 1); // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
        return move;
    }
    public void GetObjectInteractionInput()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(2) && hit.collider.gameObject != null)
            {
                Debug.Log(hit.collider.gameObject.transform.parent.name);
                if (hit.collider.gameObject.transform.parent.transform.GetComponent<PictureController>() != null) //If the object that the player is interacting is a picture
                {
                    if(hit.collider.gameObject.transform.parent.transform.GetComponent<PictureController>().PictureType == "Dynamic") {
                    GameObject parentObject = hit.collider.gameObject.transform.parent.gameObject;
                    PicManager.PickupPicture(parentObject);
                    }
                }
                else if (hit.collider.gameObject.GetComponent<ObjectInteractions>() != null)   // If the object can be interacted and is not a picture (e.g. door)
                {
                    GameObject InteractionObject = hit.collider.gameObject;
                    InteractionObject.GetComponent<ObjectInteractions>().ApplyInteraction();
                }
            }
        }
    }

}
