using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{
    public GameObject loadingImage;
    public Slider loadingBar;

    private AsyncOperation async;


    /// <summary>
    /// Loading level method that does not deal with a loading bar or screen
    /// </summary>
    /// <param name="level"></param>
    public void LoadScene(int level)
    {
        
        GameManager.currentLevel = level;
        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Set an image active if used with a parameter of the background iamge
    /// </summary>
    /// <param name="image"></param>
    public void SetActiveImage(GameObject image)
    {
        if (!image.activeSelf)
        {
            image.SetActive(true);
        }
    }

    /// <summary>
    /// Set an image active if used with a parameter of the background iamge
    /// </summary>
    /// <param name="image"></param>
    public void SetUnactiveImage(GameObject image)
    {
        if (image.activeSelf)
        {
            image.SetActive(false);
        }
    }


    /// <summary>
    /// Loads a level in the background
    /// </summary>
    /// <param name="level"></param>
    public void ClickAsync(int level)
    {
        // Start the courotine of loading the level with a loading bar
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }


    /// <summary>
    /// Loads a level in the background and saves data
    /// </summary>
    /// <param name="level"></param>
    public void ClickAsyncAndSave(int level)
    {

        // Check to make sure the game manager exists
        if (GameObject.Find("GameHandler") != null)
        {
            // First Save Game
            GameObject.Find("GameHandler").GetComponent<GameManager>().Save();
        }
        
        // turn on the loading screen and load the level
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }


    /// <summary>
    /// Loading in the level asynchronously
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    IEnumerator LoadLevelWithBar(int level)
    {
        // Load the level asynchronously
        async = SceneManager.LoadSceneAsync(level);

        while (!async.isDone) // Check to see if the level is completely loaded
        {
            // Update the bar given the load progress
            if (loadingBar != null)
            {
                loadingBar.value = async.progress;
            }
            yield return null;
        }
    }


    /// <summary>
    /// Quit method that leaves the game if used
    /// </summary>
    public void Quit()
    {
    // IF WE ARE RUNNING IN A UNITY STANDALONE
    #if UNITY_STANDALONE
        // Quit the Application
        Application.Quit();
    #endif

    // IF WE ARE RUNNING IN the unity Editor
    #if UNITY_EDITOR
        // Stop play mode in Unity
        UnityEditor.EditorApplication.isPlaying = false;
    #endif

    }

}
