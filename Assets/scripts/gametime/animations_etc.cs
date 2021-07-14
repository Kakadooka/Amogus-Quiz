using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class animations_etc : MonoBehaviour
{
    //normalBadAnswer time = 7.5f
    // 4.1f

    public Animator anim;
    public AudioSource audioSource, audioSource1;
    public Text pointsTextbox, livesTextbox;
    public GameObject scriptHolder;

    public Sprite[] susSprite;
    public SpriteRenderer susSpriteRenderer;

    public int addPoints;

    float animEnd, playSound, playSound1;
    int animNum = 0, pointsTotal = 0, lives = 9;
    bool soundPlayed = false, soundPlayed1 = false;



    //check every frame the time and if its bigger than at what points certain events should happen like playing the sound or stopping the animaton from being able to play in a loop.
    void NormalBadAnswer(){

        if (animEnd > Time.time){
            if(!soundPlayed &&  playSound < Time.time){
                soundPlayed = true;
                audioSource.Play();
                lives--; livesTextbox.text = lives.ToString();
            }
        }
        else{
            animNum = 0;            
            anim.SetBool("badAnswerNormal", false);
            scriptHolder.GetComponent<question_handler>().InsultPlayer();
        }


    }

    //initiate a bunch of stuff before playing the animation, then change the state to play the animation and fire function NormalBadAnswer
    public void PlayNormalBadAnswer(){
        setSusSprite(false);

        animEnd = Time.time + 7.4f;
        playSound = Time.time + 4.1f;
        soundPlayed = false;

        var newClip = Resources.Load<AudioClip>("answers/sus");
        audioSource.clip = newClip;
        
        animNum = 1;
        anim.SetBool("badAnswerNormal", true);

    }
    //same as NormalBadAnswer but theres an extra sound
    void NormalGoodAnswer(){

        if (animEnd > Time.time){
            if(!soundPlayed &&  playSound < Time.time){
                soundPlayed = true;
                audioSource.Play();
                pointsTotal += addPoints; pointsTextbox.text = pointsTotal.ToString();
            }
            if (!soundPlayed1 && playSound1 < Time.time){
                soundPlayed1 = true;
                audioSource1.Play();
            }
        }
        else{
            animNum = 0;            
            anim.SetBool("goodAnswerNormal", false);
            scriptHolder.GetComponent<question_handler>().PraisePlayer();
        }

    }

    //same as PlayNormalBadAnswer but theres an extra sound
    public void PlayNormalGoodAnswer(string answerName){
        setSusSprite(true);

        animEnd = Time.time + 7.4f;
        playSound = Time.time + 4.1f;
        playSound1 = Time.time + 4.2f;

        soundPlayed = false;
        soundPlayed1 = false;

        answerName = "answers/"+answerName;
        var newClip = Resources.Load<AudioClip>(answerName);
        audioSource.clip = newClip;

        newClip = Resources.Load<AudioClip>("other/shortapplause");
        audioSource1.clip = newClip;

        animNum = 2;
        anim.SetBool("goodAnswerNormal", true);

    }

    //set the right sprite for sus0 depending on if the answer was good or how many lives does the player have
    void setSusSprite(bool goodAnswer){
        if(goodAnswer){
            susSpriteRenderer.sprite = susSprite[5];
        }
        else{
            if(lives <= 1){
                susSpriteRenderer.sprite = susSprite[4];
            }
            else if(lives <= 3){
                susSpriteRenderer.sprite = susSprite[3];
            }
            else if(lives <= 5){
                susSpriteRenderer.sprite = susSprite[2];
            }
            else if(lives <= 7){
                susSpriteRenderer.sprite = susSprite[1];
            }
            else if(lives <= 9){
                susSpriteRenderer.sprite = susSprite[0];
            }

        }

    }

    void Start(){       
        //PlayNormalBadAnswer();
        //PlayNormalGoodAnswer("sus");
    }    

    void Update(){

        //animation states, depending on these certain checks are made.
        switch(animNum){
            case 1:
            NormalBadAnswer();
                break;
            case 2:
            NormalGoodAnswer();
                break;
            case 0:
            //just chillin'
                break;
        }

    }

}
