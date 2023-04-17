using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour {
    [SerializeField] protected Animator animator;
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] protected GameObject[] buttonObjs;
    protected Button[] buttons;
	protected int choice = -1;
	public int Choice {
        get {
            return choice;
        }
        private set {
            choice = value;
        }
    }
	public int score { get; set; }

    void Start() {
        buttons = buttonObjs.Select(obj => obj.GetComponent<Button>()).ToArray();
    }

    public void ThrowRock() {
        Throw(0);
        ShowChoice(choice);
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Rock"));
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Rock"));
    }

    public void ThrowPaper() {
        Throw(1);
        ShowChoice(choice);
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Paper"));
    }

    public void ThrowScissors() {
        Throw(2);
        ShowChoice(choice);
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Scissors"));
    }

    protected void Throw(int choice) {
        this.choice = choice;
        animator.SetInteger("choice", choice);
        ShowChoice(choice);
    }

    public void Win() {
        scoreText.text = (++score).ToString();
    }

    public void Lose(){
    }

    public void Draw() {
    }

    void ShowChoice(int choice) {
        for(int i = 0; i < buttonObjs.Length; i++) {
            buttons[i].enabled = false;

            if(i == choice) {
                continue;
            }

            buttonObjs[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void HideChoice() {
        choice = -1;
        animator.SetInteger("choice", choice);

        for(int i = 0; i < buttonObjs.Length; i++) {
            buttons[i].enabled = true;
            buttonObjs[i].transform.parent.gameObject.SetActive(true);
        }
    }
}
