using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class type_in_textbox : MonoBehaviour
{
    public bool active = false;
    public bool hitEnter = false;
    public Text textbox;
    public AudioSource[] audios = new AudioSource[4];
    public GameObject scriptHolder;

    public string answer = "";

    float timePass;
    string[] literki = {"q","w","e","r","t","y","u","i","o","p","a","s","d","f","g","h","j","k","l","z","x","c","v","b","n","m","space","backspace","return"};


    System.Random random = new System.Random();

    void Start()
    {
        textbox.text = answer;
    }

    public void ResetAnswer(){
        answer = "";
        textbox.text = answer;
    }

    //what to do depending on which key is down.
    void whichLetter(string letter)
    {
        switch (letter)
        {
            case "RETURN":
                if(answer != ""){
                    textbox.text = answer;
                    scriptHolder.GetComponent<question_handler>().CheckAnswer(answer);
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


    void Update()
    {

            timePass += Time.deltaTime;

            //adds blinking "|" at the end of the textbox.
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
