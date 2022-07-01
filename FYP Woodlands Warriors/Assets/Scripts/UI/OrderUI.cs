using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    public bool isChangingSize = false;
    [Range(1f, 10f)]
    public float transitionTime = 1f;
    public Vector3 sizeToTransitionTo;
    public Vector2 posToTransitionTo;
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(rect.anchoredPosition, posToTransitionTo) < 0.5f)
        {
            isChangingSize = false;

            if (sizeToTransitionTo == new Vector3(1, 1, 1))
            {
                GameManagerScript.instance.isOrderUIShrunk = false;
            }

            else
            {
                GameManagerScript.instance.isOrderUIShrunk = true;
            }
        }

        if (isChangingSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, sizeToTransitionTo, transitionTime * Time.deltaTime);
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, posToTransitionTo, transitionTime * Time.deltaTime);
        }
    }
}
