using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintCaption : MonoBehaviour {


    public AudioClip clip;

    private AudioSource source;

    private float letterPause = 0.1f;

    private string word;

    private string text = "hello everyone! 我是你们的引导者，你们可以称呼我为Leader， wellcome to our company";



	// Use this for initialization
	void Start () {

        source = GetComponent<AudioSource>();

        word = text;
        text = null;

        StartCoroutine(TypeText());

	}


    private void OnGUI()
    {

        GUI.Label(new Rect(100,100,200,200), "text show");

        GUI.Label(new Rect(50, 50, 250, 250), text);



    }


    private IEnumerator TypeText()
    {

        foreach(char letter in word.ToCharArray())
        {
            text += letter;

            if (clip)
            {
                source.PlayOneShot(clip);
            }


            yield return new WaitForSeconds(letterPause);
        }

    }




	// Update is called once per frame
	void Update () {
		


	}




}
