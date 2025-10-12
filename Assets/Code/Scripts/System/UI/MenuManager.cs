using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        if(continueButton == null) return;
        
        if(!PlayerPrefs.HasKey("CurrentScene"))
            continueButton.interactable = false;
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        ChangeScene(1);
    }
    
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CurrentScene"))
        {
            ChangeScene(PlayerPrefs.GetInt("CurrentScene"));
        }
    }

    public void ToMainMenu()
    {
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
        ChangeScene(0);
    }
    
    public void ChangeScene(int sceneIndex)
    {
        StartCoroutine(ChangeSceneDelayed(sceneIndex));
    }

    private IEnumerator ChangeSceneDelayed(int sceneIndex)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneIndex);
    }
    
    // public void ChangeScene(string sceneName)
    // {
    //     SceneManager.LoadScene(sceneName);
    // }
}
