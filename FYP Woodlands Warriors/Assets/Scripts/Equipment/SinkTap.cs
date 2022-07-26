using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SinkTap : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject tapHandle;
    public Transform pivot;

    Sink sink;

    public bool isTapTurnedLeft = false;
    public bool isTapTurnedRight = false;
    bool isResetting = false;

    [SerializeField] float timeToNextStall;

    Quaternion originalRot;

    void Awake()
    {
        sink = GetComponentInParent<Sink>();
    }

    void Start()
    {
        timeToNextStall = Random.Range(5, 8);
        originalRot = tapHandle.transform.rotation;
    }

    void FixedUpdate()
    {
        if (timeToNextStall > 0 && sink.isPouringWater)
        {
            timeToNextStall -= Time.fixedDeltaTime;
        }

        if (timeToNextStall <= 0)
        {
            StallTap();
            sink.waterOutputModifier = 75;
        }

        if (isResetting)
        {
            tapHandle.transform.rotation = Quaternion.Lerp(tapHandle.transform.rotation, originalRot, Time.fixedDeltaTime * 5);

            if (Quaternion.Angle(tapHandle.transform.rotation, originalRot) < 0.1f)
            {
                isResetting = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    //Turn the tap while clicking and dragging
    public void OnDrag(PointerEventData eventData)
    {
        //Idk why but localRotation.y = z rotation value shown in inspector / 180.  *In other words, if localRotation.y = 0.5, z rotation in inspector = 90
        if (((tapHandle.transform.localRotation.y >= -0.5f && eventData.delta.x > 0) ||
            (tapHandle.transform.localRotation.y <= 0.5f && eventData.delta.x < 0)) && !isResetting)
        {
            tapHandle.transform.RotateAround(pivot.position, -Vector3.up, eventData.delta.x);  //Rotate around pivot point set in inspector according to mouse movement
        }

        if (tapHandle.transform.localRotation.y <= -0.5f)
        {
            isTapTurnedRight = true;
        }

        if (isTapTurnedRight && tapHandle.transform.localRotation.y > -0.5f)
        {
            isTapTurnedRight = false;
        }

        if (tapHandle.transform.localRotation.y >= 0.5f)
        {
            isTapTurnedLeft = true;
        }

        if (isTapTurnedLeft && tapHandle.transform.localRotation.y < 0.5f)
        {
            isTapTurnedLeft = false;
        }
    }

    //Check the tap turned status when stopping the dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!sink.isPouringWater)
        {
            if ((CheckTap() == false) && (isTapTurnedLeft || isTapTurnedRight))
            {
                Debug.Log("You turned the tap in the wrong direction!");
                sink.waterOutputModifier = sink.waterOutputModifier / 2;
                GameManagerScript.instance.orders.dishQualityBar.AddProgress(-15f);
            }

            if (isTapTurnedLeft || isTapTurnedRight)
            {
                sink.isPouringWater = true;
                sink.waterPour.SetActive(true);
                GameManagerScript.instance.tapTurnedCount++;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }


    //Returns true if player turns tap correctly as stated in the recipe book
    bool CheckTap()
    {
        //If no strikes were made
        if (GameManagerScript.instance.strikes == 0)
        {
            if (GameManagerScript.instance.tapTurnedCount % 2 == 0) //If no. of times previously turned is even
            {
                if (isTapTurnedLeft)
                {
                    return true;
                }

                else return false;
            }

            else //If no. of times previously turned is odd
            {
                if (isTapTurnedRight)
                {
                    return true;
                }

                else return false;
            }
        }

        //If 1 strike was made
        else if (GameManagerScript.instance.strikes == 1)
        {
            if (GameManagerScript.instance.tapTurnedCount % 2 == 0) //If no. of times previously turned is even
            {
                if (isTapTurnedRight)
                {
                    return true;
                }

                else return false;
            }

            else //If no. of times previously turned is odd
            {
                if (isTapTurnedLeft)
                {
                    return true;
                }

                else return false;
            }
        }

        //If 2 strikes were made
        else if (GameManagerScript.instance.strikes == 2)
        {
            if (GameManagerScript.instance.tapTurnedCount % 2 == 0) //If no. of times previously turned is even
            {
                if (isTapTurnedRight)
                {
                    return true;
                }

                else return false;
            }

            else //If no. of times previously turned is odd
            {
                return true;
            }
        }

        else return false;
    }

    //Stop the water flow and reset the handle to the middle
    void StallTap()
    {
        Debug.Log("The tap has stalled!");
        isResetting = true;

        sink.isPouringWater = false;
        sink.waterPour.SetActive(false);
        isTapTurnedLeft = false;
        isTapTurnedRight = false;

        timeToNextStall = Random.Range(5, 8);
    }
}



