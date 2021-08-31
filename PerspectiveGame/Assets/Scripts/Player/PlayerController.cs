using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Camera")]
    public float CameraFOV = 60f;

    [Header("Character Stats")]
    public float capsuleHeightStanding = 1.8f;
    public float characterVelocityY;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float jumpSpeed = 30f;
    public float gravityDownForce = 60f;
    private float cameraVerticalAngle;
    public GameObject picobj;
    public Camera playerCamera;
    public PictureManager PicManager;
    //private CameraFov cameraFov;

    public bool isGrounded { get; private set; }
    public bool isSprinting { get; private set; }
    public Vector3 characterVelocity { get; set; }

    CharacterController characterController;
    float LastTimeJumped = 0f;
    float m_TargetCharacterHeight;
    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.07f;


    public float SpeedOnGround = 10f;
    public float movementSharpnessOnGround = 15;
    [Range(0, 1)]
    Vector3 m_GroundNormal;
    public float maxSpeedInAir = 10f;
    public float accelerationSpeedInAir = 25f;
    public float sprintSpeedModifier = 2f;
    public float cameraHeightRatio = 0.9f;
    public float speedModifier;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        PicManager = transform.GetComponent<PictureManager>();
        //cameraFov = playerCamera.GetComponent<CameraFov>();
        Cursor.lockState = CursorLockMode.Locked;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //bool wasGrounded = isGrounded;
        GroundCheck();

        speedModifier = isSprinting ? sprintSpeedModifier : 1f;
        //UpdateCharacterHeight(false);
        CharacterLook();
        CharacterMovement();
        GetObjectInteractionInput();
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
        //Debug.Log(characterVelocity.ToString() + " 1");
        CharacterLook();

        isSprinting = GetSprintInputHeld();
        Vector3 worldspaceMoveInput = transform.TransformVector(GetMoveInput());


        // 3 Axis (PLayer move forward/backward on the Z-axis, left/right on the X-axis, up/down on the Y-axis) 

        if (isGrounded) //Check if the player is on the ground
        {


            //characterVelocity = new Vector3(characterVelocity.x, -1f, characterVelocity.z);
            Vector3 TargetVelocity = worldspaceMoveInput * SpeedOnGround * speedModifier;

            characterVelocity = Vector3.Lerp(characterVelocity, TargetVelocity, movementSharpnessOnGround * Time.deltaTime);

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))  // If the player is touching the ground and he/she hits the jump button, the player will jump
            {

                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

                // then, add the jumpSpeed value upwards
                characterVelocity += Vector3.up * jumpSpeed;
                LastTimeJumped = Time.time;

                // Force grounding to false
                isGrounded = false;
                m_GroundNormal = Vector3.up;

            }

        }
        else
        {
            //Debug.Log(isGrounded + "2");
            characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;
            characterVelocityY = characterVelocity.y;
            characterVelocityY += (gravityDownForce * Time.deltaTime) * -1;
            characterVelocity = new Vector3(characterVelocity.x, characterVelocityY, characterVelocity.z);
        }

        //Debug.Log(characterVelocityY);
        characterController.Move(characterVelocity * Time.deltaTime);


    }
    void GroundCheck()
    {
        // reset values before the ground check
        isGrounded = characterController.isGrounded;

    }
    private void ResetGravityEffect()
    {
        characterVelocityY = 0f;
    }
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * characterController.radius);
    }

    // Gets the center point of the top hemisphere of the character controller capsule    
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - characterController.radius));
    }
    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) < characterController.slopeLimit;
    }

    public bool GetSprintInputHeld()
    {
        return Input.GetButton("Sprint");
    }

    public Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
        move = Vector3.ClampMagnitude(move, 1);
        return move;
    }

    public void GetObjectInteractionInput()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(0) && hit.collider.gameObject != null)
            {
                
                if (hit.collider.gameObject.transform.parent.transform.GetComponent<PictureController>() != null) //If the object that the player is interacting is a picture
                {
                    Debug.Log("pic");
                    GameObject parentObject = hit.collider.gameObject.transform.parent.gameObject;
                    PicManager.PickupPicture(parentObject);
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
