using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IInput
{
    Transform camTransform;
    [SerializeField] private Camera camera;
    

    private Vector2 inputDirection;
    private bool jump;
    private bool jumpHold;
    private bool jumpUp;
    private bool startRoll;
    private bool stopRoll;

    [Header("KEY CONFIGURATION:")]
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode rollKey;
    [SerializeField] string verticalAxisKey;
    [SerializeField] string horizontalAxisKey;

    #region PROPERTIES
    public Vector2 InputDirection { get => inputDirection; set { inputDirection = value; } }
    public bool Jump { get => jump; set { jump = value; } }
    public bool JumpHold { get => jumpHold; set { jumpHold = value; } }
    public bool JumpUp { get => jumpUp; set { jumpUp = value; } }
    public bool StartRoll { get => startRoll; set { startRoll = value; } }
    public bool StopRoll { get => stopRoll; set { stopRoll = value; } }
    #endregion

    

    private void Awake()
    {
        camTransform = camera.transform; //is there a way to assign this reference better?
        if (camera == null)
        {
            camera = Camera.main;
        }
    }
    private void Update()
    {
        //Jump
        Jump = Input.GetKeyDown(jumpKey);

        //WASD direction relative to cameras forward
        Vector3 inputVector;
        inputVector = camTransform.forward * Input.GetAxisRaw(verticalAxisKey); //Does looking up or down alternate the input vector or does the normalization fix that?
        inputVector = inputVector + camTransform.right * Input.GetAxisRaw(horizontalAxisKey);
        inputVector.y = 0;
        inputVector.Normalize();
        
        
        InputDirection = new Vector2(inputVector.x, inputVector.z);

        //Get when roll inputted and released
        StartRoll = Input.GetKeyDown(rollKey);
        StopRoll = Input.GetKeyUp(rollKey);

        JumpUp = Input.GetKeyUp(jumpKey);
        JumpHold = Input.GetKey(jumpKey);

    }
}