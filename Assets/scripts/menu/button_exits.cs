using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_exits : MonoBehaviour
{
    float timePass = 0;
    float timeClickPass = 0f;
    bool switchLol = false;
    public GameObject napisObject;
    public Sprite[] sprajt = new Sprite[1];
    public SpriteRenderer spriteRenderer;
    public float offset;
    public AudioSource audioSource;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseOver()
    {
        spriteRenderer.sprite = sprajt[1];
    }

    void OnMouseEnter()
    {
        audioSource.Play();
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = sprajt[0];
    }


    void OnMouseDown()
    {
        audioSource.Play();
        spriteRenderer.sprite = sprajt[0];
        switchLol = true;
        timeClickPass = 0f;
    }

    void Update()
    {
        timePass += Time.deltaTime;
        napisObject.transform.Rotate(0f, 0f, Mathf.Sin(timePass + offset) * 0.1f);


        if (switchLol == true)
        {
            timeClickPass += Time.deltaTime;

            spriteRenderer.sprite = sprajt[0];
            if (timeClickPass > 0.08f)
            {
                switchLol = false;
                Application.Quit();
            }
        }
    }
}