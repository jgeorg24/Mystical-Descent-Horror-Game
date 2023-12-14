using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform orientation;

    public float xSens;
    public float ySens;

    private float xRot;
    private float yRot;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * xSens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * xSens * Time.deltaTime;

        xRot -= mouseY;
        yRot += mouseX;

        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
