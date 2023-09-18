using System;
using System.Collections;
using System.Collections.Generic;
using IsoRush.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField]
    private SceneFader _sceneFader;

    private bool _clicked = false;

    private WaitForSeconds _waiter = new WaitForSeconds(1.5f);

    public void Play()
    {
        if (_clicked)
        {
            return;
        }

        _clicked = true;

        _sceneFader.FadeIn();

        StartCoroutine(WaitAndSwitchScene());
    }

    private IEnumerator WaitAndSwitchScene()
    {
        yield return _waiter;

        // game scene
        SceneManager.LoadScene(1);
    }
}
