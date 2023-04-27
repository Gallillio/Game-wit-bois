using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Dialogue  : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    public CanvasGroup canvasGroup;
    // private bool isShowingCanvas; //use this var as a flag to tell whether the canvas is showing or not
    
    // Start is called before the first frame update
    public void Start()
    {
        HideCanvas();
    }
    
    public void ShowCanvas() {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // isShowingCanvas = true;
    }
        
    public void HideCanvas() {
        canvasGroup.alpha = 0f; //this makes everything transparent
        canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        // isShowingCanvas = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ShowCanvas();
            if (textComponent.text == lines[index])
            {
                //instantly fill then go to next line
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //type each letter one by one like undertale
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            HideCanvas();
        }
    }
    
}
