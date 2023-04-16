using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour {

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
        choice = 0;
        HideButtons(choice);
        Debug.Log(string.Format("{0}: {1}", gameObject.name, "Rock"));
    }

    public void ThrowPaper() {
        choice = 1;
        HideButtons(choice);
        Debug.Log(string.Format("{0}: {1}", gameObject.name, "Paper"));
    }

    public void ThrowScissors() {
        choice = 2;
        HideButtons(choice);
        Debug.Log(string.Format("{0}: {1}", gameObject.name, "Scissors"));
    }

    public void Win() {
        scoreText.text = (++score).ToString();
    }

    public void Lose(){
    }

    public void Draw() {
    }

    public void HideButtons(int choice) {
        for(int i = 0; i < buttonObjs.Length; i++) {
            buttons[i].enabled = false;

            if(i == choice) {
                continue;
            }

            buttonObjs[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void Reset() {
        choice = -1;
        for(int i = 0; i < buttonObjs.Length; i++) {
            buttons[i].enabled = true;
            buttonObjs[i].transform.parent.gameObject.SetActive(true);
        }
    }
}
