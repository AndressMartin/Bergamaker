using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour

    
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    private Queue<string> sentences;
    private DialogueTrigger dialogueTrigger;
    private TextAsset textAsset;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        dialogueTrigger = GameObject.FindObjectOfType<DialogueTrigger>();
        textAsset = dialogueTrigger.textAsset;
    }

    void ReadCsv()
    {
        string[] data = textAsset.text.Split(new string[] { ";", "\r\n" }, StringSplitOptions.None);
        int tablesize = data.Length / 1 - 1;
        nameText.text = data[0];
        for (int i = 0; i < tablesize-1 ; i ++)
        {
            sentences.Enqueue(data[0 + i]); 
        }
    }


    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        ReadCsv();

        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count==0)
        {
            EndDialogue();
            return;
        }
        string sentece = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentece));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

}
