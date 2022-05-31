using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTransition : MonoBehaviour
{
    [SerializeField] float transitionSpeed = 1f;
    [SerializeField] bool isTransitioning = false;

    public Transform pointToMoveTo;

    // Update is called once per frame
    void Update()
    {
        if (isTransitioning == true)
        {
            transform.position = Vector3.Lerp(transform.position, pointToMoveTo.position, transitionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, pointToMoveTo.localRotation.eulerAngles, transitionSpeed * Time.deltaTime));

            if (transform.position == pointToMoveTo.position)
            {
                isTransitioning = false;
            }
        }
    }

    public void MoveCamera(Transform transitionPoint)  //Detach children and parents, start the camera transition, and enable cursor
    {
        Debug.Log(transitionPoint.position);
        pointToMoveTo = transitionPoint;
        GameManagerScript.instance.isPreparing = true;

        transform.DetachChildren();
        transform.parent = null;
        isTransitioning = true;

        GameManagerScript.instance.ChangeCursorLockedState(false);
    }
}
