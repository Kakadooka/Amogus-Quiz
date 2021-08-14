using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class animations_etc : MonoBehaviour
{
    //Variables used by too many things to put them near any specific function.



    //Change 3 sprites used to show players score to the first three numbers the players score. The ifs are there to stop errors.
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Sprite[] numberSprite;
    public SpriteRenderer[] pointsSpriteRenderer;
    public int points = 0;

    void ChangePointsNumber(){
        
        if(points.ToString().Length == 1){
            pointsSpriteRenderer[0].sprite = numberSprite[(int)(points.ToString()[0] - '0')];
            pointsSpriteRenderer[1].sprite = numberSprite[0];
            pointsSpriteRenderer[2].sprite = numberSprite[0];
        }
        else if(points.ToString().Length == 2){
            pointsSpriteRenderer[0].sprite = numberSprite[(int)(points.ToString()[1] - '0')]; 
            pointsSpriteRenderer[1].sprite = numberSprite[(int)(points.ToString()[0] - '0')];            
            pointsSpriteRenderer[2].sprite = numberSprite[0];
        }
        else if(points.ToString().Length == 3){
            pointsSpriteRenderer[0].sprite = numberSprite[(int)(points.ToString()[2] - '0')];
            pointsSpriteRenderer[1].sprite = numberSprite[(int)(points.ToString()[1] - '0')];
            pointsSpriteRenderer[2].sprite = numberSprite[(int)(points.ToString()[0] - '0')];
        }
    }

    //Just change the sprite for lives to updated amount of lives.
    //////////////////////////////////////////////////////////////
    public Sprite[] numbersLivesSprite;
    public SpriteRenderer livesSpriteRenderer;
    int lives = 9;

    void ChangeLivesNumber(){
        livesSpriteRenderer.sprite = numbersLivesSprite[(int)(lives.ToString()[0] - '0')];
    }

    //Set the parameters for animations using arrays so that every animation can use it no matter how many variables it uses.
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    float[] timedEvent = new float[10];
    string[] audioClip = new string[10];
    bool[] soundPlayed = new bool[10];
    public AudioSource[] audioSource = new AudioSource[10];

    void SetParamsBetter(float[] timedEvent, string[] audioClip, int nextAnimNum, Animator animator, string trigger){
        for(int i = 0; i < timedEvent.Length; i++){
            this.timedEvent[i] = timedEvent[i] + Time.time;
        }
        for(int i = 0; i < audioClip.Length; i++){
            
            var clip = Resources.Load<AudioClip>(audioClip[i]);
            audioSource[i].clip = clip;
            
        }
        for(int i = 0; i < soundPlayed.Length; i++){
            soundPlayed[i] = false;
        }

        animNum = nextAnimNum;
        animator.SetTrigger(trigger);
    }

    //Check every frame current time and if its bigger than at what points certain events should happen like playing a sound or stopping the animaton from being able keep playing in a loop.
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Animator animAnswer;
    public AudioSource backgroundChatter;

    void NormalBadAnswer(){

        if (timedEvent[0] > Time.time){
            if(!soundPlayed[0] &&  timedEvent[1] < Time.time){
                soundPlayed[0] = true;
                audioSource[0].Play();
                lives--; ChangeLivesNumber();
            
            }
            if (!soundPlayed[1] && timedEvent[2] < Time.time){
                soundPlayed[1] = true;
                audioSource[1].Play();
                backgroundChatter.UnPause();
            
            }
        }
        else{
            animNum = 0;           
            gameObject.GetComponent<question_handler>().stage1 = 105;
        }
    }

    //Initialize stuff with SetParamsBetter(), one of which is changing animNum to 1 so NormalBadAnswer() can do it's checks and stuff.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
    public void PlayNormalBadAnswer(){
        SetParamsBetter(new float[]{10.4f, 7.1f, 7.2f}, new string[]{"","other/heavy chatter"}, 1, animAnswer, "triggerBad");
        setSusSprite(false);
        backgroundChatter.Pause();
        streak = 0;   
    }

    //Same as NormalBadAnswer, but good.
    ////////////////////////////////////
    public int streak = 0;

    void NormalGoodAnswer(){
        if(Input.GetKeyDown(KeyCode.F1)){
            animNum = 0;
            if (streak == 3){
                streak = 0;
                gameObject.GetComponent<question_handler>().stage1 = 107;              
            }
            else{
                gameObject.GetComponent<question_handler>().stage1 = 106;
            }
        }
        if (timedEvent[0] > Time.time){
            if(!soundPlayed[0] &&  timedEvent[1] < Time.time){
                soundPlayed[0] = true;
                audioSource[0].Play();
                ChangePointsNumber();
            }
            if (!soundPlayed[1] && timedEvent[2] < Time.time){
                soundPlayed[1] = true;
                audioSource[0].Play();
                backgroundChatter.UnPause();
            }
        }
        else{
            animNum = 0;
            if (streak == 3){
                streak = 0;
                gameObject.GetComponent<question_handler>().stage1 = 107;              
            }
            else{
                gameObject.GetComponent<question_handler>().stage1 = 106;
            }
        }
    }

    //Same as PlayNormalBadAnswer but good.
    ///////////////////////////////////////
    public void PlayNormalGoodAnswer(string answerName){
        SetParamsBetter(new float[]{10.4f, 7.2f, 7.1f}, new string[]{"other/shortapplause", "answers/"+answerName}, 2, animAnswer, "triggerGood");
        setSusSprite(true);        
        backgroundChatter.Pause();
        streak++;
    }

    //Same as NormalBadAnswer but it also has a loop that decreases each time, giving susOS the slot machine effect. Also makes stayRight bool true so the machine stays on screen.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    int j, i;

    void SusosEject(){
        timePass += Time.deltaTime;

        if (timedEvent[0] < Time.time){
            animNum = 0;
            gameObject.GetComponent<susos_handler>().active = true;
            animSusos.SetBool("stayRight", true);
        }
        if(timePass*j > i){
            i++;
            j--;
            if(j < 85){audioSource[0].Play();}
            gameObject.GetComponent<susos_handler>().SetOptions(i);
        }
    }

    //Same as PlayNormalBadAnswer.
    //////////////////////////////
    System.Random random = new System.Random(Environment.TickCount);
    public Animator animSusos;
    float timePass = 0;

    public void PlaySusosEject(){
        SetParamsBetter(new float[]{10.5f}, new string[]{"other/susosBwamp"}, 3, animSusos, "triggerGoRight");

        j = 151 + random.Next(20); i = 0;
        timePass = 0;
    }

    //Same as SusosEject, but it stops susOS from staying right.
    //////////////////////////////////////////////////////////// 
    void SusosReject(){        
        if (timedEvent[0] < Time.time){
            gameObject.GetComponent<susos_handler>().active = false;
            animSusos.SetBool("stayRight", false);
        }
        if (timedEvent[1] < Time.time){
            animNum = 0;            
            gameObject.GetComponent<question_handler>().AskQuestion();
        }
    }
    //Same as everything else...
    ////////////////////////////
    public void PlaySusosReject(){
        SetParamsBetter(new float[]{1.25f, 2f}, new string[]{}, 4, animSusos, "triggerGoLeft");
    }

    //Set the right sprite for susSprite depending on if the answer was good, or if it was bad then on how many lives the player has.
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public SpriteRenderer susSpriteRenderer;
    public Sprite[] susSprite;

    void setSusSprite(bool goodAnswer){
        if(goodAnswer){
            susSpriteRenderer.sprite = susSprite[5];
        }
        else{
            if(lives <= 1){
                susSpriteRenderer.sprite = susSprite[4];
                var newClip = Resources.Load<AudioClip>("answers/sus4");
                audioSource[0].clip = newClip;
            }
            else if(lives <= 3){
                susSpriteRenderer.sprite = susSprite[3];
                var newClip = Resources.Load<AudioClip>("answers/sus3");
                audioSource[0].clip = newClip;
            }
            else if(lives <= 5){
                susSpriteRenderer.sprite = susSprite[2];
                var newClip = Resources.Load<AudioClip>("answers/sus2");
                audioSource[0].clip = newClip;
            }
            else if(lives <= 7){
                susSpriteRenderer.sprite = susSprite[1];
                var newClip = Resources.Load<AudioClip>("answers/sus1");
                audioSource[0].clip = newClip;
            }
            else if(lives <= 9){
                susSpriteRenderer.sprite = susSprite[0];
                var newClip = Resources.Load<AudioClip>("answers/sus0");
                audioSource[0].clip = newClip;
            }
        }
    }

    void Start(){       
        
    }    

    //Plays a certain animation based on the animNum variable;
    //////////////////////////////////////////////////////////
    int animNum = 0;

    void Update(){

        switch(animNum){
            case 1:
            NormalBadAnswer();
                break;
            case 2:
            NormalGoodAnswer();
                break;
            case 3:
            SusosEject();
                break;
            case 4:
            SusosReject();
                break;
            case 0:
            //Just doing nothing.
                break;
        }
    }
}
