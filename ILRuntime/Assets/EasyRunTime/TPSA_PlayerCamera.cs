using UnityEngine;
using System.Collections;
using System;

public class TPSA_PlayerCamera : MonoBehaviour
{
    public bool IsAimed = false;
    public bool IsAimedInCoverLeft = false;
    public bool IsAimedInCoverRight = false;
    public bool IsCrouched = false;
    public bool CanControll = false;
    public bool LockCursor = true;

    public Transform LookAt;
    public Transform MCamera;
    public Camera CameraObject;
    public Transform AimTarget;

    public Transform Player;

    [Range(0.0f, 10F)]
    public float targetHeight = 1.7f;
    [Range(0.0f, 10F)]
    public float distance = 4.0f;
    [Range(0.0f, 10F)]
    public float maxDistance = 4f;
    [Range(0.0f, 1F)]
    public float minDistance = 1f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int yAimMinLimit = -36;
    public int yAimMaxLimit = 58;
    public int zoomRate = 40;
    public float rotationDampening = 20f;
    public float zoomDampening = 5f;

    public Vector3 Pivot = new Vector3(-0.32f, 0.11f, 1.14f);
    public Vector3 PivotCrouch = new Vector3(-0.32f, 0.10f, 1.14f);
    public Vector3 AimPivot = new Vector3(-0.21f, 0.03f, 0.29f);
    public Vector3 AimCrouchPivot = new Vector3(-0.27f, 0.16f, 0.3f);

    public Vector3 AimInCoverLeftPivot = new Vector3(-0.21f, 0.03f, 0.29f);

    public float AdjustAimUpAfter = -24f;
    public Vector3 AdjustAimUp = new Vector3(0f, -0.05f, -0.11f);
    public float AdjustAimDownAfter = 24f;
    public Vector3 AdjustAimDown = new Vector3(0f, -0.08f, -0.33f);

    private float timer = 0.0f;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    public float midpoint = 2.0f;

    public float AimY
    {
        get { return this.yAim; }
    }

    public float AimX
    {
        get { return this.xAim; }
    }
    
    private float x = 0.0f;
    private float y = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;
    private float yAim = 0.0f;
    private float xAim = 0.0f;
    private Vector3 diffPivot = Vector3.zero;
    private Vector3 AdjustAimVector = Vector3.zero;

	void Start()
    {
        CameraObject = this.GetComponent<Camera>();
        MCamera = transform;

        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = 9;

        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        if (!CanControll)
            CanControll = true;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /**
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    void LateUpdate()
    {
        /*if (TPSA_GameManager.Instance.IsPaused)
            CanControll = false;
        else
            CanControll = true;*/

        if (!CanControll)
            return;

        // Don't do anything if target is not defined
        if (!LookAt)
            return;

        ThirdPersonCamera();
    }

    /*void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            LockCursor = false;

        if (LockCursor)
            Screen.lockCursor = true;
        else
            Screen.lockCursor = false;
    }*/

    void ThirdPersonCamera()
    {
        if (!IsAimed)
        {
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            yAim = y;
        }
        else
        {
            float aimDirVert = Input.GetAxis("Mouse Y");
            yAim -= aimDirVert * ySpeed * 0.010f;
            yAim = ClampAngle(yAim, yAimMinLimit, yAimMaxLimit);
        }

        float aimDirHor = Input.GetAxis("Mouse X");
        xAim -= aimDirHor * xSpeed * 0.010f;

        x = -xAim;

        Quaternion rotation = Quaternion.identity;
        // set camera rotation
        if(!IsAimed)
            rotation = Quaternion.Euler(y, x, 0);
        else
            rotation = Quaternion.Euler(yAim, x, 0);

        //desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;

        AdjustCameraAim();

        Vector3 selectedPivot = Vector3.zero;

        if (IsAimed)
        {
            if (!IsCrouched)
                selectedPivot = AimPivot;
            else
            {
                if (IsAimedInCoverLeft)
                    selectedPivot = AimInCoverLeftPivot;
                else
                    selectedPivot = AimCrouchPivot;
            }
        }
        else
        {
            if (IsCrouched)
                selectedPivot = PivotCrouch;
            else
                selectedPivot = Pivot;
        }

        diffPivot = Vector3.Lerp(diffPivot, selectedPivot, 0.1f);//

        // calculate desired camera position
        Vector3 position = LookAt.position - (rotation * (diffPivot + AdjustAimVector) * desiredDistance + new Vector3(0, -targetHeight, 0));

        // check for collision using the true target's desired registration point as set by user using height
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(LookAt.position.x, LookAt.position.y + targetHeight, LookAt.position.z);

        // if there was a collision, correct the camera position and calculate the corrected distance
        bool isCorrected = false;

        if (Physics.Linecast(trueTargetPosition, position, out collisionHit))
        {
            if (collisionHit.collider && collisionHit.collider.tag != "Player" && collisionHit.collider.tag != "MainCamera")
            {
                position = collisionHit.point;
                correctedDistance = Vector3.Distance(trueTargetPosition, position);
                isCorrected = true;
            }
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
        currentDistance = !isCorrected || correctedDistance > currentDistance ? correctedDistance : currentDistance; //Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;


        /*if (!IsAimed)
            position.y += CameraBob().y * 0.1f;*/

        MCamera.rotation = rotation;
        MCamera.position = position;

        AimTarget.position = MCamera.position + MCamera.forward * 10f;
    }

    Vector3 CameraBob()
    {
        float waveslice = 0.0f;
        float horizontal = 0f;
        float vertical = Input.GetAxis("Vertical");
        Vector3 cSharpConversion = MCamera.position;
        if (Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            cSharpConversion.y = midpoint + translateChange;
        }
        else
        {
            cSharpConversion.y = midpoint;
        }

        return cSharpConversion;
    }

    private void AdjustCameraAim()
    {
        if (AdjustAimUpAfter != 0f && AdjustAimUp != Vector3.zero)
        {
            if (yAim < AdjustAimUpAfter)
            {
                AdjustAimVector = Vector3.Lerp(AdjustAimVector, AdjustAimUp, 0.1f);
            }
            else
            {
                AdjustAimVector = Vector3.Lerp(AdjustAimVector, Vector3.zero, 0.08f);
            }
        }

        if (AdjustAimUpAfter != 0f && AdjustAimUp != Vector3.zero)
        {
            if (yAim > AdjustAimDownAfter)
            {
                AdjustAimVector = Vector3.Lerp(AdjustAimVector, AdjustAimDown, 0.1f);
            }
            else
            {
                AdjustAimVector = Vector3.Lerp(AdjustAimVector, Vector3.zero, 0.08f);
            }
        }
    }
    
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}