using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    private static readonly int Start = Animator.StringToHash("Start");

    public static LevelLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings));
    }

    public void ReloadCurrentLevel()
    {
        StartCoroutine(ReloadLevel());
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger(Start);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator ReloadLevel()
    {
        transition.SetTrigger(Start);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
