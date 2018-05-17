using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManagement : MonoBehaviour {

    // Variables for Instructions Screen
    public GameObject instructionsScreen;
    public List<GameObject> instructionPages;
    public List<GameObject> instructionButtons;
    public int instructionsPage = 0;

    void Update()
    {
        EscapeScreenManagement();
    }


    #region Escape Screen Management
    public void EscapeScreenManagement()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (instructionsScreen.activeSelf == true))
        {
            ContinueGame();
        }
    }

    public void ContinueGame()
    {
        // Instructions Pages
        instructionsScreen.SetActive(false);
        instructionPages[0].SetActive(true);
        instructionPages[1].SetActive(false);
        instructionButtons[0].SetActive(false);
        instructionButtons[1].SetActive(true);
        instructionsPage = 0;
    }
    #endregion


    #region instructions management

    /// <summary>
    /// Handling clicking next and previous buttons for the insturction screen
    /// </summary>
    /// <param name="forward"></param>
    public void pageTurn(bool forward)
    {
        // turn off current page
        for (int i = 0; i < instructionPages.Count; i++)
        {
            if (instructionPages[instructionsPage])
            {
                instructionPages[instructionsPage].SetActive(false);
            }
        }
        if (forward)
        {
            instructionsPage++;
        }
        else
        {
            instructionsPage--;
        }

        instructionPages[instructionsPage].SetActive(true);


        // Turn off or on the next page button if needed
        if (instructionsPage < instructionButtons.Count - 1)
        {
            instructionButtons[1].SetActive(true);
        }
        else
        {
            instructionButtons[1].SetActive(false);
        }

        if (instructionsPage > 0)
        {
            instructionButtons[0].SetActive(true);
        }
        else
        {
            instructionButtons[0].SetActive(false);
        }

    }
    #endregion
}
