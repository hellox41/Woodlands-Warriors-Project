using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController charController;

    public float moveSpeed = 5f;

    public bool isSprinting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManagerScript.instance.isZoomed && !GameManagerScript.instance.isPreparing)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            charController.Move(move * moveSpeed * Time.deltaTime);
        }
    }
}
