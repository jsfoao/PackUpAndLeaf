using System;
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
    [SerializeField] string jumpKey;
    [SerializeField] string rollKey;
    [SerializeField] string verticalAxisKey;
    [SerializeField] string horizontalAxisKey;

    private bool inputActive;
    private bool runTimer;
    #region PROPERTIES
    public Vector2 InputDirection { get => inputDirection; set { inputDirection = value; } }
    public bool Jump { get => jump; set { jump = value; } }
    public bool JumpHold { get => jumpHold; set { jumpHold = value; } }
    public bool JumpUp { get => jumpUp; set { jumpUp = value; } }
    public bool StartRoll { get => startRoll; set { startRoll = value; } }
    public bool StopRoll { get => stopRoll; set { stopRoll = value; } }
    #endregion

    private float inputTimer;
    [NonSerialized] public bool usingMouse;
    private float mouseTimer;

    public void SetInput(bool active)
    {
        inputActive = active;
        
        // buffer if activating input
        if (inputActive == true)
        {
            inputTimer = 0.2f;
        }
        camera.GetComponent<MouseCameraController>().SetInput(active);
    }

    private void Awake()
    {
        SetInput(true);
        
        camTransform = camera.transform; //is there a way to assign this reference better?
        if (camera == null)
        {
            camera = Camera.main;
        }

        mouseTimer = 2f;
    }
    private void Update()
    {
        //Jump
        if (!inputActive)
        {
            return;
        }
        
        inputTimer -= Time.deltaTime;
        inputTimer = Mathf.Clamp(inputTimer, -1, 2);
        if (inputTimer <= 0f)
        {
            ExecuteInput();
        }

        // Mouse buffer
        Vector2 mouseVec = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        bool mouseInput = mouseVec.magnitude > 0.5f;

        mouseTimer = Mathf.Clamp(mouseTimer, -2f, 4f);
        if (!mouseInput)
        {
            mouseTimer -= Time.deltaTime;
        }
        else
        {
            mouseTimer = 2f;
        }

        Debug.Log("using mouse: " + usingMouse);
        usingMouse = mouseTimer > 0; 
    }

    private void ExecuteInput()
    {
        Jump = Input.GetButtonDown(jumpKey);

        //WASD direction relative to cameras forward
        Vector3 inputVector;
        inputVector = camTransform.forward * Input.GetAxisRaw(verticalAxisKey); //Does looking up or down alternate the input vector or does the normalization fix that?
        inputVector = inputVector + camTransform.right * Input.GetAxisRaw(horizontalAxisKey);
        inputVector.y = 0;
        inputVector.Normalize();
        
        
        InputDirection = new Vector2(inputVector.x, inputVector.z);

        //Get when roll inputted and released
        StartRoll = Input.GetButtonDown(rollKey);
        StopRoll = Input.GetButtonUp(rollKey);

        JumpUp = Input.GetButtonUp(jumpKey);
        JumpHold = Input.GetButtonDown(jumpKey);
    }
}
