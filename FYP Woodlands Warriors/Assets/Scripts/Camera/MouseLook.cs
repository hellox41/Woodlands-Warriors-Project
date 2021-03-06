using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Range(50, 300)]
    public float mouseSensitivty = 100f;

    public Transform playerBody;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManagerScript.instance.isZoomed && !GameManagerScript.instance.isInteracting && !GameManagerScript.instance.isPreparing && !GameManagerScript.instance.isShowcasing)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivty * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivty * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
