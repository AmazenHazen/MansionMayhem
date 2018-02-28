using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHoverMouseChange : MonoBehaviour {

    // Reference to the image changes
    public Sprite imageChange;
    public TextAsset descriptionChange;

    // Reference to the text and image to change
    public GameObject imageToChange;
    public GameObject descriptionToChange;

	// Use this for initialization
	void Start () {
        imageToChange = GameObject.Find("InformationImage");
        descriptionToChange = GameObject.Find("InformationText");
    }

    public void OnMouseEnter()
    {
        //Debug.Log("Hovering + " + gameObject);
        imageToChange.GetComponent<Image>().sprite = imageChange;
        descriptionToChange.GetComponent<Text>().text = descriptionChange.text;
    }
}
