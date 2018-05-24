using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchManager : MonoBehaviour
{
    // Attributes
    public List<GameObject> upgradePanels; // holds the different panels for weapons, trinkets, and equipement

    /// <summary>
    /// Method for changing the selection for upgrades
    /// </summary>
    /// <param name="panelSelected"></param>
    public void SwitchSection(GameObject panelSelected)
    {
        // Hide the other Panels
        for(int i=0; i<upgradePanels.Count; i++)
        {
            // Loop through the panels and select the one that is passed in
            if(panelSelected == upgradePanels[i])
            {
                upgradePanels[i].SetActive(true);
            }
            else
            {
                upgradePanels[i].SetActive(false);
            }
        }


    }

}
