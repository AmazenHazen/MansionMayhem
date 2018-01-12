using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOptionScript : MonoBehaviour
{
    public ResponseType currentResponseType;
    public GameObject currentNPC;

    public void ClickedResponse()
    {
        currentNPC.GetComponent<NPC>().ChooseDialogOption(currentResponseType);
    }

}
