using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Movement : MonoBehaviour
{
    private StoneCollection collection;
    [SerializeField] Transform enemy;
    private CharacterController controller;
    public Transform cam;
    public AudioClip heavyBreathingSound; // Assign this in the Inspector
    private AudioSource audioSource;
    private float horizontal;
    private float vertical;

    public float speed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float crouchHeightFactor = 0.3f;

    private float originalCamHeight;
    private Vector3 originalCamPos;
    private float runKeyHoldTime = 0f;

    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCamHeight = cam.localPosition.y;
        originalCamPos = cam.localPosition;
        IsRunning = false;
        IsCrouching = false;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = heavyBreathingSound;
        audioSource.loop = true;
        audioSource.volume = 0f; // Start with volume at 0
    }

    void Update()
    {
        GameWin();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * vertical + transform.right * horizontal;

    // Running
    if (Input.GetKey(KeyCode.LeftShift))
    {
        controller.Move(move * runSpeed * Time.deltaTime);
        IsRunning = true;
        IsCrouching = false;

        runKeyHoldTime += Time.deltaTime;

        if (runKeyHoldTime > 1f && !audioSource.isPlaying)
        {
            audioSource.Play();
            StartCoroutine(FadeIn(audioSource, 1f)); // Start fading in after 1 second of running
        }
    }
    else
    {
        if (IsRunning && audioSource.isPlaying)
        {
            runKeyHoldTime = 0f; // Reset the timer
            StopCoroutine(FadeIn(audioSource, 1f)); // Stop any ongoing fade-in
            StartCoroutine(FadeOut(audioSource, 1f)); // Immediately start fading out
            IsRunning = false;
        }

            // Crouching
            if (Input.GetKey(KeyCode.C))
            {
                Crouch();
                controller.Move(move * crouchSpeed * Time.deltaTime);
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
                IsCrouching = false;
            }
        }
    }

private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
{
    float targetVolume = 1f;
    audioSource.volume = 0;

    while (audioSource.volume < targetVolume && IsRunning)
    {
        audioSource.volume += Time.deltaTime / fadeTime;
        yield return null;
    }

    if (!IsRunning)
    {
        audioSource.Stop();
    }
}

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset the volume for the next play
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


    private void GameWin()
    {
        if (StoneCollection.crystalCount == 3)
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
