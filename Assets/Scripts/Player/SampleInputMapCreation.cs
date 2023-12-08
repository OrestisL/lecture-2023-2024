using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// Sample code for creating an action map.
/// You can add as many actions as you want.
/// Dont forget to add the InputSystem packge from the package manager (should be done before importing this script,
/// and  before adding more code).
/// </summary>
public class SampleInputMapCreation : GenericSingleton<SampleInputMapCreation>
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;
    [SerializeField] private CameraCollision camCollision;
    [SerializeField] private CameraMovement camMovement;
    [SerializeField] private FirstPersonMovement firstPersonMovement;
    public UnityEngine.UI.Button button;

    public GameObject player;
    private Animator anim;
    public enum MovementType
    {
        keyboard,
        gamepad
    }

    public enum PerspectiveType
    {
        firstPerson,
        thirdPerson
    }
    /// <summary>
    /// Determines the movement type.
    /// 0 = keyboard, 1= gamepad
    /// </summary>
    public MovementType movementType;

    public PerspectiveType perspectiveType;

    /// <summary>
    /// This component is used to handle inputs from the user.
    /// </summary>
    public PlayerInput playerInput;
    /// <summary>
    /// InputAction is used to create actions based on inputs.
    /// InputAction has to be manually enabled and disabled.
    /// </summary>
    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction buttonsAction, buttonsAction_g;
    public InputAction zoomAction;
    public InputAction fireAction;
    public InputAction altFireAction;
    public InputAction runAction;
    public InputAction jumpAction;
    /// <summary>
    /// Input action maps are used to store input actions.
    /// </summary>
    public InputActionMap keyboardMap, gamepadMap;
    [Header("This is for loading the input configuration")]
    public string jsonPath;
    private string mapJson;
    private string saveDir;

    public float gravity = -10.0f;
    public float jumpHeight = 5;
    private bool jumpIsPressed;

    public float speed = 3.0f, runSpeed = 5.0f;
    public bool isRunning = false;
    private Vector3 velocity;
    float lowEnergySpeed;
    private float turnSmoothVelocity, turnSmoothTime = 0.1f;
    public float scrollSensitivity = 0.5f;
    [Range(0.01f, 0.1f)]
    public float mouseSensitivity = 0.05f;
    //gamepad zoom is weird
    private bool isZooming;
    //charge attack
    [SerializeField]
    private NavMeshAgent currentAgent;
    private int previousSceneIndex;
    public override void Awake()
   {
        base.Awake();

        saveDir = Path.Combine(Application.persistentDataPath, "InputJson");
#if UNITY_EDITOR
        Debug.Log(Directory.Exists(saveDir));
#endif
        //check if folder exists
        if (!Directory.Exists(saveDir))
        {
            Debug.Log("JsonInput folder doesnt exist, creating");
            Directory.CreateDirectory(saveDir);
        }

        //check if json exists
        jsonPath = Path.Combine(saveDir, "DefaultConrtols.json");//InputJson/defaultControls.json";
        if (!File.Exists(jsonPath))
        {
            Debug.Log("Json not found, creating...");
            File.Create(jsonPath).Close(); //System.IO.File.Create(string path) is a stream, so calling .Close() on the stream allows us
                                           //to create the file and close it so we can read/write on it from other streams
        }
        //check if player input exists on the gameObject
        if (playerInput == null)
        {
            if (!TryGetComponent<PlayerInput>(out playerInput))
                playerInput = gameObject.AddComponent<PlayerInput>();
        }
        FindCamera();
        InitializeInputs();
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;


    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (previousSceneIndex == scene.buildIndex)
            return;

        previousSceneIndex = scene.buildIndex;
        if (scene.buildIndex == 3)
        {
            perspectiveType = PerspectiveType.thirdPerson;
        }
        FindCamera();
        InitializeInputs();
    }

    private void FindCamera() 
    {
        cam = Camera.main;
        camCollision = cam.GetComponent<CameraCollision>();
        camMovement = cam.GetComponentInParent<CameraMovement>();
    }

    private void InitializeInputs()
    {
        //create input actions and action maps
        CreateActions();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        //main components
        if (controller == null)
            controller = player.GetComponent<CharacterController>();

        if (anim == null)
            anim = player.GetComponent<Animator>();       

        switch (perspectiveType)
        {
            case PerspectiveType.firstPerson:
                firstPersonMovement = player.GetComponent<FirstPersonMovement>();
                break;
            case PerspectiveType.thirdPerson:
                camMovement = cam.GetComponentInParent<CameraMovement>();
                camCollision = cam.GetComponent<CameraCollision>();
                break;
        }

        ChangeActionMap(movementType);
    }

    void CreateActions()
    {
        //read the json
        Debug.Log("Reading input json...");
        using (StreamReader sr = new StreamReader(jsonPath))
        {
            mapJson = sr.ReadToEnd();
            sr.Close();
        }

        //if it's empty, create it
        if (mapJson.Length == 0)
        {
            Debug.Log("Input json was empty, creating...");
            //create action maps, one for keyboard and one for gamepad
            //with appropriate names
            keyboardMap = new InputActionMap("Keyboard");
            gamepadMap = new InputActionMap("Gamepad");
            //this makes an action inside the gamepad map that is named Move and uses the left stick of the gamepad 
            moveAction = gamepadMap.AddAction("Move", binding: "<Gamepad>/leftStick");
            //this adds another binding to the move action. the new binding is called Dpad
            //the .With() function adds more bindings
            moveAction = keyboardMap.AddAction("Move");
            moveAction.AddCompositeBinding("Dpad")
                .With("Up", "<keyboard>/w")
                .With("Down", "<keyboard>/s")
                .With("Left", "<keyboard>/a")
                .With("Right", "<keyboard>/d");

            moveAction.AddCompositeBinding("Dpad")
                .With("Up", "<keyboard>/UpArrow")
                .With("Down", "<keyboard>/DownArrow")
                .With("Left", "<keyboard>/LeftArrow")
                .With("Right", "<keyboard>/RightArrow");

            //create look action with mouse/right stick
            lookAction = keyboardMap.AddAction("Look", binding: "<Mouse>/delta");
            lookAction = gamepadMap.AddAction("Look", binding: "<Gamepad>/rightStick");

            //fire action with left click/right trigger
            fireAction = keyboardMap.AddAction("Fire", binding: "<Mouse>/leftButton");
            fireAction = gamepadMap.AddAction("Fire", binding: "<Gamepad>/rightTrigger");

            //alt fire action with right click/left trigger
            altFireAction = keyboardMap.AddAction("AltFire", binding: "<Mouse>/rightButton");
            altFireAction = gamepadMap.AddAction("AltFire", binding: "<Gamepad>/leftTrigger");

            //cancel action
            InputAction cancel = new InputAction();
            cancel = keyboardMap.AddAction("Cancel", binding: "<keyboard>/escape");
            cancel = gamepadMap.AddAction("Cancel", binding: "<Gamepad>/start");

            //when using buttons, instead of checking for values
            buttonsAction = keyboardMap.AddAction("Buttons");
            buttonsAction = gamepadMap.AddAction("Buttons");

            //jump
            jumpAction = keyboardMap.AddAction("Jump", binding: "<keyboard>/Space");
            jumpAction = gamepadMap.AddAction("Jump", binding: "<Gamepad>/ButtonSouth");

            //run
            runAction = keyboardMap.AddAction("Run", binding: "<keyboard>/leftShift");
            runAction = gamepadMap.AddAction("Run", binding: "<Gamepad>/leftStickPress");

            //zoom
            zoomAction = keyboardMap.AddAction("Zoom", binding: "<Mouse>/Scroll/Y", processors: "clamp(min=-1,max=1)");
            zoomAction = gamepadMap.AddAction("Zoom");
            zoomAction.AddCompositeBinding("1DAxis(maxValue = 5, minValue = -5)")
                .With("Positive", "<Gamepad>/rightShoulder")
                .With("Negative", "<Gamepad>/leftShoulder");

            //create the asset
            var asset = ScriptableObject.CreateInstance<InputActionAsset>();
            asset.name = "Default Controls";
            asset.AddActionMap(keyboardMap);
            asset.AddActionMap(gamepadMap);
            playerInput.actions = asset;
            playerInput.currentActionMap = keyboardMap;
            keyboardMap.Enable();
            //save the map as a json
            mapJson = asset.ToJson();
            using (StreamWriter sw = new StreamWriter(jsonPath))
            {
                sw.Write(mapJson);
                sw.Close();
            }
            Debug.Log("Created input json and saved it to " + jsonPath);
        }
        else
        {
            playerInput.actions = InputActionAsset.FromJson(mapJson);
            playerInput.actions.Enable();
            playerInput.SwitchCurrentActionMap(playerInput.actions.actionMaps[0].name);
            Debug.Log("Successfully read and applied input map to player");
        }
    }

    void InputEvents()
    {
        //find the actions necessary
        //assign functionality to the appropriate events
        //started is called when the button is pressed (same as OnButtonDown)
        //performed is called while the button is being held
        //canceled is called when the button is released (same as OnButtonUp)

        runAction = playerInput.currentActionMap.FindAction("Run");
        //runAction.started += context => isRunning = true;
        runAction.performed += context => isRunning = true;
        runAction.canceled += context => isRunning = false;

        lookAction = playerInput.currentActionMap.FindAction("Look");
        lookAction.performed += context =>
        {
            switch (perspectiveType)
            {
                case PerspectiveType.thirdPerson:
                    if (movementType == MovementType.keyboard)
                        camMovement.HandleRotationMovement(context.ReadValue<Vector2>() * mouseSensitivity);
                    else if (movementType == MovementType.gamepad)
                        camMovement.HandleRotationMovement(50 * mouseSensitivity * context.ReadValue<Vector2>());
                    break;

                case PerspectiveType.firstPerson:
                    firstPersonMovement.CameraRotation(context.ReadValue<Vector2>() * mouseSensitivity);
                    break;
            }


        };

        fireAction = playerInput.currentActionMap.FindAction("Fire");
        fireAction.performed += context =>
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (currentAgent == null) //no agent selected
                {
                    if (hit.transform.TryGetComponent(out currentAgent))
                    {
                        return;
                    }
                }
                else //agent selected
                {
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2.0f, NavMesh.AllAreas))
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = navHit.position;
                        cube.transform.localScale = Vector3.one * 0.5f;
                        cube.transform.up = hit.normal;
                        currentAgent.SetDestination(navHit.position);
                    }
                }

            }
        };
        fireAction.canceled += context => { };

        zoomAction = playerInput.currentActionMap.FindAction("Zoom");
        if (playerInput.currentActionMap.name.Equals("Keyboard"))
        {
            zoomAction.performed += context =>
            {
                if (perspectiveType == PerspectiveType.thirdPerson)
                {
                    Debug.Log(context.ReadValue<float>());
                    camCollision.Zoom(context.ReadValue<float>() * scrollSensitivity);
                }
            };
        }
        else if (playerInput.currentActionMap.name.Equals("Gamepad"))
        {
            if (perspectiveType == PerspectiveType.thirdPerson)
            {
                zoomAction.started += context => { isZooming = true; };
                zoomAction.canceled += context => { isZooming = false; };
            }
        }

        jumpAction = playerInput.currentActionMap.FindAction("Jump");
        jumpAction.performed += context => { Jump(jumpHeight); };

        moveAction = playerInput.currentActionMap.FindAction("Move");
        //playerInput.currentActionMap.FindAction("Cancel").started += context => { Timer.Instance.Pause(); };


        //jumpAction = null;
        //runAction = null;
        //lookAction = null;
        //fireAction = null;
        //zoomAction = null;
    }

    public void ChangeActionMap(MovementType type)
    {
        Debug.Log(string.Format("Previous map was {0}", playerInput.currentActionMap.name));
        switch (type)
        {
            case MovementType.keyboard:
                playerInput.SwitchCurrentActionMap("Keyboard");
                InputEvents();
                break;
            case MovementType.gamepad:
                playerInput.SwitchCurrentActionMap("Gamepad");
                InputEvents();
                break;
        }
        Debug.Log(string.Format("New map is {0}", playerInput.currentActionMap.name));
    }

    private void Update()
    {
        Move();
        ApplyGravity();
        GamepadZoom();
    }

    void ApplyGravity()
    {
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = gravity;
        else if (!controller.isGrounded)
            velocity.y += gravity * Time.deltaTime;
    }


    void Move()
    {
        Vector3 inputDirection;

        Vector2 move = moveAction.ReadValue<Vector2>();

        inputDirection = new Vector3(move.x, 0, move.y).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            Debug.Log("move");
            float currentSpeed = speed;
            //apply camera rotation to player 
            float targetAngle = cam.transform.eulerAngles.y + Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (perspectiveType == PerspectiveType.thirdPerson)
                player.transform.localRotation = Quaternion.Euler(0, angle, 0);

            if (isRunning)
            {
                currentSpeed = runSpeed;
            }

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * currentSpeed;
            controller.Move(Time.deltaTime * moveDirection);

            //anim.SetFloat("Speed", currentSpeed);
        }
        //else
        //anim.SetFloat("Speed", 0);
    }

    void Jump(float height)
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(-2 * gravity * height);
        }

    }

    void GamepadZoom()
    {
        if (isZooming)
        {
            camCollision.Zoom(zoomAction.ReadValue<float>() * scrollSensitivity * 0.01f);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Debug.Log("Lost focus");
            if (playerInput != null && playerInput.actions != null)
                Debug.Log(string.Format("{0} {1} {2}", playerInput.actions.name, playerInput.actions.enabled, playerInput.actions.actionMaps.Count));
        }
        else
        {
            Debug.Log("Regained focus");
            if (playerInput != null && playerInput.actions != null)
                playerInput.actions.Enable();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        //in case there were changes, write them when the player quits
        using (StreamWriter sw = new StreamWriter(jsonPath))
        {
            sw.Write(mapJson);
            sw.Close();
        }
    }

}
