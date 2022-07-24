using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReceiptNouns : MonoBehaviour
{
    TMP_Text paperText;

    public List<string> apparatusList = new List<string>();
    public List<string> nounsList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        paperText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        paperText.text = $"<align=center><u>NOUNS</u></align>";

        if (GameManagerScript.instance.levelNo == 1)  //apparatus for kaya toast and eggs
        {
            paperText.text += $"\n<align=left>KNIFE: " + $"<size=80%>{GameManagerScript.instance.knifeNoun}</size>";
            paperText.text += $"\n<align=left>SPATULA: " + $"<size=80%>{GameManagerScript.instance.spatulaNoun}</size>";
            paperText.text += $"\n<align=left>STRAINER: " + $"<size=80%>{GameManagerScript.instance.strainerNoun}</size>";
        }

        else if (GameManagerScript.instance.levelNo == 2)  //apparatus for eggs and satay
        {
            paperText.text += $"\n<align=left>SPATULA: " + $"<size=80%>{GameManagerScript.instance.spatulaNoun}</size>";
            paperText.text += $"\n<align=left>STRAINER: " + $"<size=80%>{GameManagerScript.instance.strainerNoun}</size>";
            paperText.text += $"\n<align=left>PESTLE: " + $"<size=80%>{GameManagerScript.instance.pestleNoun}</size>";
        }

        else if (GameManagerScript.instance.levelNo == 3)  //apparatus for satay and nasi lemak
        {
            paperText.text += $"\n<align=left>SPATULA: " + $"<size=80%>{GameManagerScript.instance.spatulaNoun}</size>";
            paperText.text += $"\n<align=left>PESTLE: " + $"<size=80%>{GameManagerScript.instance.pestleNoun}</size>";
            paperText.text += $"\n<align=left>PADDLE: " + $"<size=80%>{GameManagerScript.instance.paddleNoun}</size>";
        }
    }
}
