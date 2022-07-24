using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(Transition(levelIndex));
    }

    IEnumerator Transition(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene(levelIndex);
    }
}
