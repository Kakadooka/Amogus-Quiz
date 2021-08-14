using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class intensity_handler : MonoBehaviour
{

    float intensitySeconds = 3f;

    public void AddIntensity(int num){
        intensity += num;

        gameObject.GetComponent<event_handler>().ChangeIntensityLevel(intensity);
        switch(intensity / 10){
            case 1:

                break;
            case 2:

                break;
            case 3:
                safetySeconds = 8f; intensitySeconds = 2f;
                break;
            case 4:

                break;
            case 5:

                break;
            case 6:
                safetySeconds = 6f;
                break;
            case 7:

                break;
            case 8:
                safetySeconds = 3f;
                break;
            case 9:
                intensitySeconds = 1f;
                break;
            case 10:
                //dead
                break;
        }
    } 


    //to do: lowers intensity: correct question, 3 correct questions lower you a level
    public void LowerIntensity(int num){
        intensity -= num;

        intensity = (intensity / 10) * 10;
    }

    bool increaseActive = false;
    public void TurnOnOffIncrease(bool on){
        if(on){
        timepass = 0f;
        increaseActive = true;
        }
        else{
            increaseActive = false;
            nSecondSafety = true;
        }
    }

    float timepass = 0f;
    float safetySeconds = 10f;
    public int intensity = 0;
    void Start()
    {
        
    }

    bool nSecondSafety = true;
    void Update()
    {
        //Add 1 intensity every 3 seconds after n second safety has passed. Used when waiting for an answer.
        if(increaseActive){
            timepass += Time.deltaTime;
            if(nSecondSafety){
                if(timepass > safetySeconds){
                    nSecondSafety = false;
                }
            }
            else if(timepass > intensitySeconds){
                AddIntensity(1);
                timepass = 0f;
            }
        }
        //--//
    }
}
