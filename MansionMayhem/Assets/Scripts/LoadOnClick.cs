using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOnClick : MonoBehaviour
{
    public GameObject loadingImage;

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
        GameManager.currentLevel = level;
    }
    public void SetActiveImage(GameObject image)
    {
        image.SetActive(true);
    }

}
