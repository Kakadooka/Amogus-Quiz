using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class type_in_textbox : MonoBehaviour
{




    //Start().
    //////////
    public string answer = "";
    public Text textbox;

    void Start()
    {
        textbox.text = answer;
    }

    //Clear the answer.
    ///////////////////
    public void ResetAnswer(){
        answer = "";
        textbox.text = answer;
    }

    //Does different things depending on which special key was pressed. Default is any normal letter key.
    /////////////////////////////////////////////////////////////////////////////////////////////////////
    string[] literki = {"q","w","e","r","t","y","u","i","o","p","a","s","d","f","g","h","j","k","l","z","x","c","v","b","n","m", "space","backspace","return"};
    public AudioSource[] audios = new AudioSource[4]; //<- click sound audios
    System.Random random = new System.Random();

    void whichLetter(string letter)
    {
        switch (letter)
        {
            case "RETURN":
                if(answer != ""){
                    textbox.text = answer;
                    gameObject.GetComponent<question_handler>().CheckAnswer(answer);
                    active = false;
                }
                break;
            case "BACKSPACE":
                if (answer.Length != 0) {
                    answer = answer.Remove(answer.Length - 1);
                }
                audios[2].Play();
                break;
            case "SPACE":
                if(answer.Length <= 16){
                    answer += " ";
                    audios[random.Next(2)].Play();
                }
                break;
            default:
                if(answer.Length <= 16){
                    answer += letter;
                    audios[random.Next(2)].Play();
                }
                break;
        }
    }

    //Every update checks which keys were pressed.
    //////////////////////////////////////////////
    public bool active = false;
    float timePass;

    void Update()
    {
            //Adds blinking "|" at the end of the textbox.
            timePass += Time.deltaTime;
            textbox.text = (timePass % 1 > 0.5) ? answer : answer + "|";

            if (active) {
            foreach (string literka in this.literki)
            {
                if (Input.GetKeyDown(literka))
                {
                    whichLetter(literka.ToUpper());
                }
            } 
        } 
    }
}
