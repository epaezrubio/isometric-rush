using System;
using System.Collections;
using System.Collections.Generic;
using IsoRush.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [SerializeField]
    private SceneFader _sceneFader;

    private bool _clicked = false;

    private WaitForSeconds _waiter = new WaitForSeconds(1.5f);

    public void Exit()
    {
        if (_clicked)
        {
            return;
        }

        _clicked = true;

        _sceneFader.FadeIn();

        StartCoroutine(WaitAndExit());
    }

    private IEnumerator WaitAndExit()
    {
        yield return _waiter;

        Application.Quit();
    }
}
