using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrepStatus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript.instance.prepStatusText = GetComponent<TMP_Text>();
        transform.parent.gameObject.SetActive(false);
        transform.parent.parent.gameObject.SetActive(false);
    }
}
