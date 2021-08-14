using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;


    /**
    Basically a 2D list but done this way so Unity's JsonUtililty can comprehend whats going on.
    **/
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


    /**
    Add the dissapearing and appearing "|" at the end of the textbox.
    **/
    float timePass = 0f;
    string showOnScreenText = "";
    void FlickerEndOfText()
    {
        timePass += Time.deltaTime;
        if (timePass % 1 > 0.5) { textbox.text = showOnScreenText + "|"; }
        else { textbox.text = showOnScreenText; }
    }

    /**
    Types any given text on screen letter by letter like its the most basic rpg game ever.
    **/
    public AudioSource boop, audioSource;
    int letter = 0;
    int currentText = 0;
    string[] stage0Text = { " ","Welcome Ladies and Round-bodied bipedal creatures! ", "I am this ships artificial intelligence designed solely to ask mundane trivia questions. ", "Other crewmembers deemed your past behaviour SUS. And it is your duty to prove them otherwise. ", "By partaking in this incredibely boring trivia game... ", "All you need to do is answer all of the questions correctly, with the possibility of 3 mistakes, and you shall live. ", "My co-host will make sure your answers are correct. ", "You got that? ", "No? ", "Well, get ready BECAUSE ITS TIME TO PLAY THE AMOGUS QUIZ!!!!! "};
    string[] stage1Text = { "Stop. ", "Okay, time for the first question. ", "Ekhem, " };

    void SpeakText(string[] text, int nextStage, int stageStage){

        /**
        If currently read text is the last one in the array then clean up and change to next stage.
        **/
        if (currentText == text.Length)
        {
            switch(stageStage){
                case 1:
                stage = nextStage;
                    break;
                case 2:
                stage1 = nextStage;
                    break;                   
            }
            timePass = 0;
            letter = 0;
            currentText = 0;
        }

        else if (!Input.GetKeyDown("return") && !Input.GetKeyDown("space"))
        {
            //Just do the same thing but with the last visible letter being higher each loop.
            if (letter < text[currentText].Length)
            {
                if (timePass * 12 > letter)
                {
                    showOnScreenText = "> " + text[currentText].Remove(letter);
                    if (letter != text[currentText].Length) { letter++; }
                    else { showOnScreenText = "> " + text[currentText]; }
                    boop.Play();
                }
            }
        }
        else
        {
            //If the current text is fully displayed on screen then change to the next one in the array.
            if (letter == text[currentText].Length)
            {
                currentText++;
                timePass = 0; letter = 0;
            }
            //If its not then make the text fully appear.
            else
            {
                letter = text[currentText].Length;
                showOnScreenText = "> " + text[currentText].Remove(text[currentText].Length-1);
            }
        }
        FlickerEndOfText();
    }

    /**
    Same thing as SpeakText except its a single string, it lets the player answer when its done speaking and it starts playing the music again.
    **/
    public GameObject answerBox;

    void SpeakQuestion(string text){
        if(Input.GetKeyDown(KeyCode.F1)){letter = text.Length-1; timePass = 12 * text.Length;}
            //Just do the same thing but with the last visible letter being higher each loop.
            if (letter < text.Length)
            {
                if (timePass * 12 > letter)
                {
                    showOnScreenText = "> " + text.Remove(letter);

                    if (letter != (text.Length-1)) { letter++; boop.Play();}
                    else{                                           
                        showOnScreenText = "> " + text.Remove(text.Length - 1);
                        gameObject.GetComponent<type_in_textbox>().active = true;
                        gameObject.GetComponent<music_handler>().StartPlaying();
                        stage1 = 0;
                }
            }
        }
        FlickerEndOfText();
    }

    /**
    Make a giant APPLAUSE appear on screen and blink with the audience cheering.
    **/
    float applauseSeconds;
    bool applauseFireOnce = true;
    public AudioSource applause;
    public Text textbox;
    void Applause()
    {
        //Initialize text stuff and other things before firing.
        if (applauseFireOnce) {
            textbox.alignment = TextAnchor.MiddleCenter;
            textbox.fontSize = 128;

            var newClip = Resources.Load<AudioClip>("other/applause");
            audioSource.clip = newClip;
            audioSource.Play();

            applauseSeconds = random.Next(5);
            //gameObject.GetComponent<animations_etc>().anim.SetBool("goodAnswerNormal", true); //zrob specjalna animacje dla tego pepemods
            applauseFireOnce = false;
        }

        if (timePass % 1 < 0.5) { textbox.text = "APPLAUSE"; }
        else { textbox.text = ""; }

        //Leave condition.
        if (timePass > 1f + applauseSeconds || Input.GetKeyDown("return")) {
            audioSource.Stop();
            //gameObject.GetComponent<animations_etc>().anim.SetBool("goodAnswerNormal", false); //zrob specjalna animacje dla tego pepemods
            textbox.text = " ";
            timePass = 0;
            textbox.alignment = TextAnchor.UpperLeft;
            textbox.fontSize = 32;
            applauseFireOnce = true;
            stage++;
        }
        timePass += Time.deltaTime;
    }

    /**
    Change the sprite of category and difficulty based on... category and difficulty.
    **/
    public Sprite[] spriteDiff;
    public Sprite[] spriteCat;
    public SpriteRenderer spriteRendererCat, spriteRendererDiff;
    public GameObject susosKorbka;

    void ChangeCategoryDifficulty(){
            
            switch(category){
                case "science": 
                spriteRendererCat.sprite = spriteDiff[0];
                    break;
                case "history": 
                spriteRendererCat.sprite = spriteDiff[1];
                    break;
                case "geography": 
                spriteRendererCat.sprite = spriteDiff[2];
                    break;
                case "sports": 
                spriteRendererCat.sprite = spriteDiff[3];
                    break;
                case "literature": 
                spriteRendererCat.sprite = spriteDiff[4];
                    break;
                case "religion": 
                spriteRendererCat.sprite = spriteDiff[5];
                    break;
                case "language": 
                spriteRendererCat.sprite = spriteDiff[6];
                    break;              
                case "internet": 
                spriteRendererCat.sprite = spriteDiff[7];
                    break;       
            }
            switch(difficulty){
                case "very easy": 
                spriteRendererDiff.sprite = spriteCat[0];
                susosKorbka.transform.rotation = Quaternion.Euler(0, 0, 80); 
                    break;
                case "easy": 
                spriteRendererDiff.sprite = spriteCat[1];
                susosKorbka.transform.rotation = Quaternion.Euler(0, 0, 43); 
                    break;
                case "medium": 
                spriteRendererDiff.sprite = spriteCat[2];
                susosKorbka.transform.rotation = Quaternion.Euler(0, 0, 0); 
                    break;
                case "hard": 
                spriteRendererDiff.sprite = spriteCat[3];

                susosKorbka.transform.rotation = Quaternion.Euler(0, 0, -18);                 
                    break;
                case "very hard": 
                spriteRendererDiff.sprite = spriteCat[4];
                susosKorbka.transform.rotation = Quaternion.Euler(0, 0, -60); 
                    break;  
            }
        }
    //
    /////////////////////////
    void ChangeKorbka(){
        switch(difficulty){
            case "very easy": 
            susosKorbka.transform.rotation = Quaternion.Euler(0, 0, 80); 
                break;
            case "easy": 
            susosKorbka.transform.rotation = Quaternion.Euler(0, 0, 43); 
                break;
            case "medium": 
            susosKorbka.transform.rotation = Quaternion.Euler(0, 0, 0); 
                break;
            case "hard": 
            susosKorbka.transform.rotation = Quaternion.Euler(0, 0, -18);                 
                break;
            case "very hard": 
            susosKorbka.transform.rotation = Quaternion.Euler(0, 0, -60); 
                break;  
        }
    }

    /**
    Set all of the question based things depending on the chosen question.
    **/
    System.Random random = new System.Random(Environment.TickCount);
    int qNum;
    string question;
    string answer;    
    string category;
    string difficulty = "temp";
    int points;

    void SetQuestion(StringListList q){
  
        qNum = random.Next(q.stringListList.Count);

        question = q.stringListList[qNum].stringList[0];
        answer = q.stringListList[qNum].stringList[1];
        difficulty = q.stringListList[qNum].stringList[2];
        category = q.stringListList[qNum].stringList[3];
        points = Int32.Parse(q.stringListList[qNum].stringList[4]);

        q.stringListList.RemoveAt(qNum);
    }

    //
    ///////////////////////////////////////////
    int qAmount;
    StringListList wantedQs = new StringListList();
    public string wantedCat;
    bool sayNo = false;

    void SetQuestionCategory(){

        qAmount = nextDiff.stringListList.Count;
        for (int i = 0; i < qAmount; i++){
            if(nextDiff.stringListList[i].stringList[3] == wantedCat){
                wantedQs.stringListList.Add(nextDiff.stringListList[i]);
            }
        }
        if(wantedQs.stringListList.Count == 0){SetQuestion(nextDiff); sayNo = true;}
        else{SetQuestion(wantedQs);}
        wantedCat = "";
    }

    /**
    Set the difficulty of a question based on the given percentages from chooseRandomQuestion(). The difficulty string change is spaghetti code so susosKorbka can easily change.
    **/
    StringListList nextDiff;
    void chooseQuestionByPercantage(int one, int two, int three, int four){
        
            if(randPercentage <= one){
                nextDiff = questionsVeryEasy;
                difficulty = "very easy";
            }
            else if(randPercentage > one && randPercentage <= two){
                nextDiff = questionsEasy;
                difficulty = "easy";
            }
            else if(randPercentage > two && randPercentage <= three){
                nextDiff = questionsMedium;
                difficulty = "medium";
            }
            else if(randPercentage > three && randPercentage <= four){
                nextDiff = questionsHard;
                difficulty = "hard";
            }
            else if(randPercentage > four){
                nextDiff = questionsVeryHard;
                difficulty = "very hard";
            }
    }

    /**
    Checks how many points the player has, then depending on how far they have gotten change the chances of getting each difficulty.
    **/
    int randPercentage;

    void chooseRandomQuestion()
    {

        randPercentage = random.Next(100)+1;
        Debug.Log(randPercentage);
        //last question
        if(totalPoints >= 125){
            chooseQuestionByPercantage(0, 0, 10, 70);
        }
        //brutal
        else if(totalPoints >= 100){
            chooseQuestionByPercantage(0, 10, 30, 80);
        }
        //hard
        else if(totalPoints >= 72){
            chooseQuestionByPercantage(4, 40, 76, 92);
        }
        //medium
        else if(totalPoints >= 36){
            chooseQuestionByPercantage(12, 60, 84, 96);
        }
        //easy
        else{
            chooseQuestionByPercantage(20, 80, 95, 100);
        }
    }

    /**
    Self explanatory(is this how you spell it?).
    **/
    public void AskQuestion(){
        if(difficulty == "temp"){chooseRandomQuestion();}
        if(wantedCat != "" && !sayNo){SetQuestionCategory();} else{SetQuestion(nextDiff);}
        ChangeCategoryDifficulty();

        gameObject.GetComponent<type_in_textbox>().ResetAnswer();

        if(sayNo){stage1 = 4; randomString = new List<string>(){"No. "};sayNo = false;}else{stage1 = 1;}
        timePass = 0;
    }



    /**
    Gets called by type_in_textbox after you hit enter, checks if the given answer is correct or not.
    **/
    public int totalPoints = 0;

    public void CheckAnswer(string givenAnswer)
    {       
            givenAnswer = givenAnswer.ToLower();

            if(givenAnswer.Contains(answer)){
                if(lastQuestion){
                    //todo: you win, congrats!!!!!!!!!!!!!!!!!!!!!!make this later, eg.: dont design the end in the middle.
                }
                else{
                    totalPoints += points;

                    chooseRandomQuestion();
                    ChangeKorbka();

                    gameObject.GetComponent<animations_etc>().points = totalPoints;
                    gameObject.GetComponent<animations_etc>().PlayNormalGoodAnswer(answer);
                    gameObject.GetComponent<music_handler>().StopPlaying();
                    var newClip = Resources.Load<AudioClip>("other/combined right");
                    audioSource.clip = newClip;
                    audioSource.Play();

                    stage1 = 0;
                }
            }
            else{                       
                    gameObject.GetComponent<animations_etc>().PlayNormalBadAnswer();
                    gameObject.GetComponent<music_handler>().StopPlaying();
                    var newClip = Resources.Load<AudioClip>("other/combined wrong");
                    audioSource.clip = newClip;
                    audioSource.Play();

                    stage1=0;                    
            }
        FlickerEndOfText(); 
    }

    /**
    make it change from ins0 to ins1 for example
    **/
    void ChooseBasedOnIntensity(){

    }

    /**
    Choose radom string from json, delete it, check if there are any left and replenish if there are none, and then go to the stage that reads it.
    **/
    int jsonNumber;
    List<string> randomString;

    void SetRandomStringFromJson(StringListList json, string nameOf, int nextStage){
        jsonNumber = random.Next(json.stringListList.Count);
        randomString = json.stringListList[jsonNumber].stringList;
        json.stringListList.RemoveAt(jsonNumber);

        if(json.stringListList.Count == 0){
            json = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/" + nameOf + ".json"));
        }

        letter = 0; timePass = 0;
        stage1 = nextStage;
    }


    /**
    INITIALIZE. INITIALIZE. INITIALIZE.
    **/
    StringListList  questionsVeryEasy, questionsEasy, questionsMedium, questionsHard, questionsVeryHard, 
    pra0, tra0, ins0, bon0;  
    void Start()
    {

        //Read from jsons and put data into these objects.
        questionsVeryEasy = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/qve.json"));
        questionsEasy = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/qe.json"));
        questionsMedium = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/qm.json"));
        questionsHard = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/qh.json"));
        questionsVeryHard = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/qvh.json"));

        pra0 = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/pra0.json"));
        tra0 = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/tra0.json"));
        ins0 = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/ins0.json"));
        bon0 = JsonUtility.FromJson<StringListList>(File.ReadAllText("Data/bon0.json"));

    }

    /**
    Stage system. stage = intro, stage1 = game loop.
    The ones that only fire once are there so they can be easily referenced from functions that change stage1 like SpeakText();
    **/
    public int stage = 0;
    public int stage1 = 101;
    bool lastQuestion = false;

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
                SpeakText(stage1Text, 3, 1);
                break;
            case 3:
                switch (stage1) {
                    case 0:
                        //just chillin', hbu?    
                        FlickerEndOfText();
                        break;
                    case 1:
                        //speak the damn question
                        SpeakQuestion(question);
                        break;
                    case 101:
                        //most of the time it sets the question, then goes to 1 and speaks it, when the points are above 125, it first tells the player its the last question and then goes and asks it.
                        AskQuestion();
                        break;
                    case 2:
                        //after speaking go to transition
                        SpeakText(randomString.ToArray(), 104, 2);
                        break;
                    case 4:
                        //after speaking go ask question
                        SpeakText(randomString.ToArray(), 101, 2);
                        break;
                    case 7:
                        SpeakText(randomString.ToArray(), 208, 2);
                        break;
                    case 104:
                        //set transition, go to 4 and speak it
                        if(totalPoints >= 125){stage1 = 999;}else{SetRandomStringFromJson(tra0, "tra0", 4);}
                        break;
                    case 105:
                        //set insult, go to 2 and speak it
                        SetRandomStringFromJson(ins0, "ins0", 2);
                        break;
                    case 106:  
                        //set praise, go to 2 and speak it
                        SetRandomStringFromJson(pra0, "pra0", 2);
                        break;
                    case 107:
                        SetRandomStringFromJson(bon0, "bon0", 7);
                        break;
                    case 208:
                        gameObject.GetComponent<animations_etc>().PlaySusosEject();
                        stage1 = 0;
                        break;
                    case 999:
                        lastQuestion = true;
                        SpeakText(new string[]{"Oh! ", "It looks like you've made it to he last question. ", "All you need to do is answer one more question correctly and you're deemed not SUS. ", "Here it comes. "}, 101, 2);
                        break;
               }
               break;


        }
    }
}
