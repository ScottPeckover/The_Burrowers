using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talking : MonoBehaviour {

    public string character;
    public string speech;
    public Text characterText;
    public Text speechText;
    public GameObject bubble;
    private bool talking;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void talk(bool talking)
    {
        if (talking)
        {
            bubble.SetActive(true);
            characterText.text = character;
            speechText.text = speech;
        }
        else
            bubble.SetActive(false);
        this.talking = talking;
    }

    public void toggleTalk()
    {
        talk(!talking);
    }
}
