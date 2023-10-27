using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    /// <summary>
    /// This script is used to follow the player and move the camera around the player with the mouse.
    /// In order to use, it should be setup as follows:
    ///     Camera base (with this script attached)
    ///         Camera (with camera collision script attached)
    ///         
    /// Change ManagedClass to MonoBehavior and uncomment Update and LateUpdate methods for both scripts to
    /// if you dont want to use a manager
    /// </summary>

    [SerializeField] private float CameraMoveSpeed = 120.0f;      // How fast the camera moves.
    public GameObject camFollowObj;                               // The follow target.
    public float clampAngleMin,clampAngleMax;                              // How high/low we want to be able to look.
    public float inputSensitivity = 150.0f;
    public GameObject cam;


    //Distance from player
    private float camDistanceXtoPlayer;
    private float camDistanceYtoPlayer;
    private float camDistanceZtoPLayer;
    //Mouse inputs
    //public float mouseX;
    //public float mouseY;

    private float finalInputX;
    private float finalInputZ;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    [Header("Camera smoothing")]
    public bool smoothCam = false;
    [Range(1, 5)]
    public int smoothAmount = 1;

    public bool AltPressed = false;                               //Used to determine when alt's pressed, so the cursor can appear                   
    private Vector3 followPos;



    public delegate void CameraDel();

    public event CameraDel CameraChangeRotation;

    public void OnCameraChangedRotation()
    {
        CameraChangeRotation?.Invoke();
    }


    void Start()
    {
        //Get the starting angles and save them in the variables created.
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetButton("Fire3"))
    //    {
    //        HandleRotationMovement();
    //    }
    //}

    //public override void OnUpdate()
    //{
    //    //if (Input.GetButton("Fire3"))
    //    //{
    //    //    HandleRotationMovement();
    //    //}

    //}

    public void LateUpdate()
    { 
        CameraUpdater();
    }
    //private void LateUpdate()
    //{
    //    CameraUpdater();
    //}

    public void HandleRotationMovement(Vector2 delta)
    {

        //float inputX = Input.GetAxis("RightStickHorizontal");
        //float inputZ = Input.GetAxis("RightStickVertical");

        float mouseX = delta.x;
        float mouseY = delta.y;

        // Get either the analogue stick input or mouse input (this can be improved)
        finalInputX = mouseX;
        finalInputZ = mouseY;

        //if (finalInputX != 0 || finalInputZ != 0)
        //{
        //    OnCameraChangedRotation();
        //}

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        //Clamp rotation x between -clampAngle and clampAngle
        rotX = Mathf.Clamp(rotX, -clampAngleMin, clampAngleMax);

        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0.0f);

        transform.rotation = localRot;
    }

    private void CameraUpdater()
    {
        //Set target object
        Transform target = camFollowObj.transform;

        //Move towards the object that is the target
        float step = CameraMoveSpeed * Time.deltaTime;
        if (!smoothCam)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        else
        {
            Vector3 vel = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref vel, (smoothAmount / 20f));
        }

    }


    private void OnApplicationQuit()
    {
        //Unsubscrible all functions from the event.
        CameraChangeRotation = null;
    }
}
