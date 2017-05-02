using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
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
