using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skewers : MonoBehaviour
{
    public GameObject skewer;
    public GameObject spawnedSkewer;

    public Transform spawnPos;
    public Transform pickupPos;

    public bool isSkewerMoving = false;
    [SerializeField] float transitionSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSkewerMoving)
        {
            spawnedSkewer.transform.position = Vector3.Lerp(spawnedSkewer.transform.position, pickupPos.position, Time.deltaTime * transitionSpeed);
            spawnedSkewer.transform.rotation = Quaternion.Euler(Vector3.Lerp(spawnedSkewer.transform.rotation.eulerAngles, pickupPos.localRotation.eulerAngles, transitionSpeed * Time.deltaTime));

            if (Vector3.Distance(spawnedSkewer.transform.position, pickupPos.position) < 0.05f)
            {
                isSkewerMoving = false;
            }
        }
    }

    public void PickupSkewer()
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Threading Cubes" && !GameManagerScript.instance.orders.satayPrep.isHoldingSkewer)
        {
            GameManagerScript.instance.orders.satayPrep.isHoldingSkewer = true;
            GameManagerScript.instance.orders.satayPrep.threadingUI.SetActive(true);
            spawnedSkewer = Instantiate(skewer, spawnPos.position, Quaternion.identity);
            GameObject.Find("mortar").GetComponent<Mortar>().skewerScript = spawnedSkewer.GetComponent<Skewer>();
            isSkewerMoving = true;
        }
    }
}
