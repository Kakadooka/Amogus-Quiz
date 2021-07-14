using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

//holds information about questions
// [Serializable]
// public class Question {
//     public List<string> question = new List<string>();
//     public List<string> answer = new List<string>();
//     public List<string> difficulty = new List<string>();
//     public List<string> category = new List<string>(); 
// }

//basically a list of a list but done this way so unitys JsonUtililty can comprehend whats going on
    [Serializable]
    public class StringList{
        public List<string> stringList = new List<string>();
    }
    [Serializable]
    public class StringListList{
        [SerializeField]
        public List<StringList> stringListList = new List<StringList>();
    }

public class question_handler : MonoBehaviour
{
    //variables for many things
    public Text textbox, categoryTextbox, difficultyTextbox;
    public AudioSource boop;
    public AudioSource usSource;
    int stage = 3;
    int stage2 = 101;
    float timePass = 0f;
    string showOnScreenText;
    System.Random random = new System.Random(Environment.TickCount);
    int questionNumber;
    public GameObject scriptHolder;

    //variables for SpeakText()
    int letter = 0;
    int currentText = 0;
    //bool speakTextFireOnce = true;

    string[] stage0Text = { " ","Welcome Ladies and Round-bodied bipedal creatures! ", "I am this ships artificial intelligence designed solely to ask mundane trivia questions. ", "Other crewmembers deemed your past behaviour SUS. And it is your duty to prove them otherwise. ", "By partaking in this incredibely boring trivia game... ", "All you need to do is answer all of the questions correctly, with the possibility of 3 mistakes, and you shall live. ", "My co-host will make sure your answers are correct. ", "You got that? ", "No? ", "Well, get ready BECAUSE ITS TIME TO PLAY THE AMOGUS QUIZ!!!!! "};
    string[] stage2Text = { "Stop. ", "Okay, time for the first question. ", "Ekhem, " };

    //variables for Applause()
    float applauseSeconds;
    bool applauseFireOnce = true;
    public AudioSource applause;
    

    //variables for AskQuestion()
    StringListList questionsJson;

    //variables for SpeakQuestion()
    public GameObject answerBox;

    //variables for WaitForAnswer()
    string answer;

    //variables for PraisePlayer(), IsultPlayer() etc.
    StringListList praiseJson;
    StringListList transitionJson;
    StringListList insultJson;
    List<string> randomString;
    int jsonNumber;


    //////////////////////////////////////////////////
    //    END             OF           VARIABLES    //
    //////////////////////////////////////////////////
    //    START            OF            CLASSES    //
    //////////////////////////////////////////////////

    

    //add the dissapearing and appearing "|" at the end of the textbox.
    void FlickerEndOfText()
    {
        timePass += Time.deltaTime;
        if (timePass % 1 > 0.5) { textbox.text = showOnScreenText + "|"; }
        else { textbox.text = showOnScreenText; }
    }

    //types any given text on screen like its the most basic rpg game ever
    void SpeakText(string[] text, int nextStage, int stageStage){
        

        //if currently read text is the last one in the array then clean up change to next stage
        if (currentText == text.Length)
        {
            switch(stageStage){
                case 1:
                stage = nextStage;
                    break;
                case 2:
                stage2 = nextStage;
                    break;                   
            }

            timePass = 0;
            letter = 0;
            currentText = 0;
        }

        else if (!Input.GetKeyDown("return"))
        {

            //just do the same thing but with the last visible letter being higher each loop.
            if (letter < text[currentText].Length)
            {

                if (timePass * 12 > letter)
                {
                    showOnScreenText = text[currentText].Remove(letter);

                    if (letter != text[currentText].Length) { letter++; }
                    else { showOnScreenText = text[currentText]; }

                    boop.Play();
                }
            }
        }
        else
        {
            //if the current text is fully displayed on screen then change to the next one in the array
            if (letter == text[currentText].Length)
            {
                currentText++;
                timePass = 0; letter = 0;
            }
            //if its not then make the text appear fully
            else
            {
                letter = text[currentText].Length;
                showOnScreenText = text[currentText].Remove(text[currentText].Length-1);
            }
        }

        FlickerEndOfText();
    }

    //same thing as SpeakText except its a single string and it lets the player answer when its done speaking
    void SpeakQuestion(string text){

        if (!Input.GetKeyDown("return"))
        {
            //just do the same thing but with the last visible letter being higher each loop.
            if (letter < text.Length)
            {
                if (timePass * 12 > letter)
                {
                    showOnScreenText = text.Remove(letter);

                    if (letter != (text.Length-1)) { letter++; boop.Play();}
                    else{                                           
                        showOnScreenText = text.Remove(text.Length - 1);
                        //activate textbox so the player can type
                        scriptHolder.GetComponent<type_in_textbox>().active = true;
                        stage2 = 0;
                        
                    }  
                }
            }
        }
        else
        {
            letter = text.Length;
            showOnScreenText = text.Remove(text.Length - 1);
            //activate textbox so the player can type
            scriptHolder.GetComponent<type_in_textbox>().active = true;
            stage2 = 0;
        }
        FlickerEndOfText();
    }

    //applauses
    void Applause()
    {
        //initialize stuff before firing
        if (applauseFireOnce) {
            textbox.alignment = TextAnchor.MiddleCenter;
            textbox.fontSize = 128;
            applause.Play();
            applauseSeconds = random.Next(5);

            applauseFireOnce = false;
        }

        //APPLAUSE
        
        if (timePass % 1 < 0.5) { textbox.text = "APPLAUSE"; }
        else { textbox.text = ""; }

        
        //leave condition
        if (timePass > 1f + applauseSeconds || Input.GetKeyDown("return")) {
            applause.Stop();

            textbox.text = " ";
            timePass = 0;
            textbox.alignment = TextAnchor.UpperLeft;
            textbox.fontSize = 48;
            applauseFireOnce = true;
            stage++;
        }
    }

    //choose random question
    void AskQuestion(){
        questionNumber = random.Next(questionsJson.stringListList.Count);

        scriptHolder.GetComponent<type_in_textbox>().ResetAnswer();
        scriptHolder.GetComponent<animations_etc>().addPoints = Int32.Parse(questionsJson.stringListList[questionNumber].stringList[4]);

        difficultyTextbox.text = questionsJson.stringListList[questionNumber].stringList[2];
        categoryTextbox.text = questionsJson.stringListList[questionNumber].stringList[3];

        stage2 = 1;
    }

    //gets called by type_in_textbox after you hit enter, checks if the given answer is correct or not
    public void CheckAnswer(string answer)
    {
            scriptHolder.GetComponent<type_in_textbox>().active = false;
            
            answer = answer.ToLower();
            //Debug.Log(questionsJson.stringListList[questionNumber].stringList[1]);

            if(answer == questionsJson.stringListList[questionNumber].stringList[1]){

                scriptHolder.GetComponent<animations_etc>().PlayNormalGoodAnswer(answer);
                stage2 = 0;
            }
            else{                       
                    scriptHolder.GetComponent<animations_etc>().PlayNormalBadAnswer();
                    stage2=0;
            }
            
            

        FlickerEndOfText();
         
    }

    public void PraisePlayer(){

        jsonNumber = random.Next(praiseJson.stringListList.Count);
        randomString = praiseJson.stringListList[jsonNumber].stringList;

        letter = 0;
        timePass = 0;
        stage2 = 2;
    }

    public void InsultPlayer(){

        jsonNumber = random.Next(insultJson.stringListList.Count);
        randomString = insultJson.stringListList[jsonNumber].stringList;

        letter = 0;
        timePass = 0;
        stage2 = 2;
    }

    void TransitionToQuestion(){
        jsonNumber = random.Next(transitionJson.stringListList.Count);
        randomString = transitionJson.stringListList[jsonNumber].stringList;

        letter = 0;
        timePass = 0;
        stage2 = 4;
    }


    void Start()
    {

        //read from jsons and put data into these objects
        questionsJson = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/questions.json"));
        praiseJson = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/praises0.json"));
        transitionJson = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/transitions0.json"));
        insultJson = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/insults0.json"));
    }



    void Update()
    {
        switch (stage) {
            case 0:
                SpeakText(stage0Text, 1, 1);
                break;
            case 1:
                Applause();
                break;
            case 2:
                SpeakText(stage2Text, 3, 1);
                break;
            case 3:
                switch (stage2) {
                    case 0:
                    //chillin', hbu?
                                            
                        FlickerEndOfText();
                        break;
                    case 1:
                        SpeakQuestion(questionsJson.stringListList[questionNumber].stringList[0]);
                        break;
                    case 101:
                        AskQuestion();
                        break;
                    case 2:
                        SpeakText(randomString.ToArray(), 104, 2);
                        break;
                    case 4:
                        //basically TransitionToQuestion()
                        SpeakText(randomString.ToArray(), 101, 2);
                        break;
                    case 104:
                        TransitionToQuestion();
                        break;
                    
               
               }
               break;


        }
    }
}
