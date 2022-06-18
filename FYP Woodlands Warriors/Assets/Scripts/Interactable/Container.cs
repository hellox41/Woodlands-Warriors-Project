using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Transform placePoint;
    public bool isContainingItem = false;
    public bool isShowingPreview = false;

    public GameObject itemContained;

    [Range(0f, 1f)]
    [SerializeField] float previewAlpha = 0.5f;

    public GameObject previewedFood;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPreview()
    {
        if (!isShowingPreview)
        {
            previewedFood = Instantiate(GameManagerScript.instance.playerInventory.currentItemHeld, placePoint.position, placePoint.rotation);
            //previewedFood.transform.parent = transform;

            previewedFood.layer = LayerMask.NameToLayer("Ignore Raycast");

            if (previewedFood.GetComponent<Collider>() != null)
            {
                previewedFood.GetComponent<Collider>().isTrigger = true;
            }
            Color previewColor = previewedFood.GetComponent<MeshRenderer>().material.color;
            previewColor.a = previewAlpha;
            previewedFood.GetComponent<MeshRenderer>().material.color = previewColor;
            isShowingPreview = true;
        }
    }

    public void HidePreview()
    {
        Destroy(previewedFood);
        isShowingPreview = false;
    }

    public void Contain(GameObject itemToContain)
    {
        Destroy(previewedFood);
        GameObject itemContained = Instantiate(itemToContain, placePoint.position, placePoint.rotation);

        itemContained.layer = LayerMask.NameToLayer("Ignore Raycast");
        itemContained.transform.parent = transform;
        this.itemContained = itemContained;
        isContainingItem = true;
    }
}
