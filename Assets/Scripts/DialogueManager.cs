using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {


    public Text nameText;
    public Text dialogueText;
    public Button continueButton;

    public Animator animator;

    private Queue<string> sentences;


	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}

    public void StartDialogue (Dialogue dialogue) {

        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence () {
        if (sentences.Count == 0) {
            return;
        }
        string sentence = sentences.Dequeue();
        if (sentences.Count == 0) {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
            continueButton.gameObject.SetActive(false);
            return;
        }
        continueButton.gameObject.SetActive(true);


        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue() {
        animator.SetBool("IsOpen", false );
    }



}
