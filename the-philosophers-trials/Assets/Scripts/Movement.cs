using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform enemy;
    private CharacterController controller;
    public Transform cam;

    private float horizontal;
    private float vertical;

    public float speed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float crouchHeightFactor = 0.3f;

    private float originalCamHeight;
    private Vector3 originalCamPos;

    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCamHeight = cam.localPosition.y;
        originalCamPos = cam.localPosition;
        IsRunning = false;
        IsCrouching = false;
    }

    void Update()
    {
        GameWin();
        GameLost();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * vertical + transform.right * horizontal;

        // Running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * runSpeed * Time.deltaTime);
            IsRunning = true;
            IsCrouching = false;
        }
        // Crouching
        else if (Input.GetKey(KeyCode.C))
        {
            Crouch();
            controller.Move(move * crouchSpeed * Time.deltaTime);
            IsRunning = false;
            IsCrouching = true;
        }
        else
        {
            if (IsCrouching)
            {
                StandUp();
            }
            cam.localPosition = originalCamPos; // Reset camera position
            controller.Move(move * speed * Time.deltaTime);
            IsRunning = false;
            IsCrouching = false;
        }
    }

    void Crouch()
    {
        if (!IsCrouching)
        {
            cam.localPosition = new Vector3(cam.localPosition.x, originalCamHeight * crouchHeightFactor, cam.localPosition.z);
        }
    }

    void StandUp()
    {
        cam.localPosition = new Vector3(cam.localPosition.x, originalCamHeight, cam.localPosition.z);
    }

    private void GameLost()
    {
        if (Vector3.Distance(transform.position, enemy.position) <= 1.5)
        {
            SceneManager.LoadScene("Lose Screen");
        }
    }

    private void GameWin()
    {
        if (true == false)
        {
            SceneManager.LoadScene("Win Screen");
        }
    }
}


    // void RunCameraEffect()
    // {
    //     if (vertical != 0 || horizontal != 0) // Add camera shake when running
    //     {
    //         cam.localPosition = originalCamPos + Random.insideUnitSphere * runningShakeAmount;
    //     }
    //     else
    //     {
    //         cam.localPosition = originalCamPos;
    //     }
    // }
