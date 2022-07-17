using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skewer : MonoBehaviour
{
    public Transform[] meatPoints;

    public Transform spawnPos;

    public List<GameObject> meats = new List<GameObject>();
    public GameObject threadingMeat;

    public bool isThreading = false;

    public float threadingSpeed = 5f;

    [Header("Meat Prefabs")]
    public GameObject mixedBeef;
    public GameObject mixedChicken;
    public GameObject mixedMutton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isThreading)
        {
            threadingMeat.transform.position = Vector3.Lerp(threadingMeat.transform.position, meatPoints[meats.Count - 1].position, Time.deltaTime * threadingSpeed);

            if (Vector3.Distance(threadingMeat.transform.position, meatPoints[meats.Count - 1].position) < 0.05f)
            {
                threadingMeat.transform.parent = transform;
                isThreading = false;
            }
        }
    }

    public void ThreadMeatOntoSkewer(string meatType)
    {
        if (!isThreading)
        {
            if (meatType == "Beef")
            {
                threadingMeat = Instantiate(mixedBeef, spawnPos.position, spawnPos.rotation);
            }

            else if (meatType == "Chicken")
            {
                threadingMeat = Instantiate(mixedChicken, spawnPos.position, spawnPos.rotation);
            }

            else if (meatType == "Mutton")
            {
                threadingMeat = Instantiate(mixedMutton, spawnPos.position, spawnPos.rotation);
            }

            meats.Add(threadingMeat);
            isThreading = true;
        }
    }
}
