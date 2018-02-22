using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOptionScript : MonoBehaviour
{
    public GameObject currentNPC;
    public ResponseType currentResponseType;
    public int lineJumpNumber;

    // Scrolling Autotyping variables
    private bool isTyping = false;
    private bool cancelTyping = false;

    public void ClickedResponse()
    {
        currentNPC.GetComponent<NPC>().ChooseDialogOption(currentResponseType, lineJumpNumber);
    }

}
