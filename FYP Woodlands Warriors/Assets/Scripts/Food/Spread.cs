using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : MonoBehaviour
{
    public Material butterSpread;
    public Material kayaSpread;
    public Material butterAndKayaSpread;

    public MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    public void UpdateSpread(string condimentToSpread)
    {
        if (condimentToSpread == "Butter")
        {
            mr.material = butterSpread;
        }

        if (condimentToSpread == "Kaya")
        {
            mr.material = kayaSpread;
        }

        if (condimentToSpread == "Kaya and Butter")
        {
            mr.material = butterAndKayaSpread;
        }
    }
}
