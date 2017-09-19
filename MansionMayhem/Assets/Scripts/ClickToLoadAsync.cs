using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour {

    public Slider loadingBar;
    public GameObject loadingImage;

    private AsyncOperation async;

    public void ClickAsync(int level)
    {
        // Set current level in the gameManager
        //GameManager.currentLevel = level;

        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }


    public void ClickAsyncAndSave(int level)
    {
        if (GameObject.Find("GameHandler") != null)
        {
            // First Save Game
            GameObject.Find("GameHandler").GetComponent<GameManager>().Save();
        }

        // Set current level in the gameManager
        //GameManager.currentLevel = level;

        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }


    IEnumerator LoadLevelWithBar (int level)
    {
        async = Application.LoadLevelAsync(level);

        while(!async.isDone) // Check to see if the level is completely loaded
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
