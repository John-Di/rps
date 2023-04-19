using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour {
    [SerializeField] protected Animator animator;
    [SerializeField] protected TextMeshProUGUI scoreText;
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

    public void ThrowRock() {
        Throw(0);
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Rock"));
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Rock"));
    }

    public void ThrowPaper() {
        Throw(1);
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Paper"));
    }

    public void ThrowScissors() {
        Throw(2);
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Scissors"));
    }

    public void ThrowRandom() {
        Throw(Random.Range(0, 3));
        // Debug.Log(string.Format("{0}: {1}", gameObject.name, "Scissors"));
    }

    public void Fail() {
        animator.SetInteger("choice", 0);
    }

    protected void Throw(int choice) {
        this.choice = choice;
        animator.SetInteger("choice", choice);
    }

    public void Win() {
        scoreText.text = (++score).ToString();
    }

    public void Lose(){
    }

    public void Draw() {
    }

    public void Reset() {
        choice = -1;
        animator.SetInteger("choice", choice);
        animator.Play("Idle", -1, 0f);
    }
}
