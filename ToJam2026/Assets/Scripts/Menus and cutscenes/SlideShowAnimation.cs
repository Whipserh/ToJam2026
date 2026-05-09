using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlideShowAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slideIndex = 0;
        StartCoroutine(showSlide());
        slideIndex++;
    }

    void Update()
    {
        if (!running && slideIndex < images.Length)
        {
            StartCoroutine(showSlide()); 
            slideIndex++;
        }

        //wait till the last slide runs and then move to next scene
        if(slideIndex == images.Length && !running) SceneManager.LoadScene(2);
    }


    bool running = false;
    IEnumerator showSlide()
    {
        running = true;
        //start with the first image
        spriteUI.sprite = images[slideIndex];
        float fadeTimer = 0;

        //Fade in
        while(fadeTimer <= fadeTime)
        {   
            fadeTimer += Time.deltaTime;
            spriteUI.color = new Color(spriteUI.color.r, spriteUI.color.g, spriteUI.color.b, fadeTimer/fadeTime); 
            yield return null;
        }

        //Wait
        yield return new WaitForSeconds(durationPerSlide);


        //Fade out
        fadeTimer = 0;
        while (fadeTimer <= fadeTime)
        {
            yield return null;
            fadeTimer += Time.deltaTime;
            spriteUI.color = new Color(spriteUI.color.r, spriteUI.color.g, spriteUI.color.b, 1 - (fadeTimer / fadeTime));
        }
        running = false;
    }

    public Image spriteUI;
    private int slideIndex =0;
    
    public float fadeTime = 1;
    public float durationPerSlide = 2;
    public Sprite [] images;
}
