using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gamepad : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] RPSButtons;
    [SerializeField] Button[] gameplayButtons;
    [SerializeField] GameObject undoButton;
    [SerializeField] GameObject playAgainButton;

    Button[] buttons;
    GameObject[] buttonContainers;

    void Awake() {
        buttons = RPSButtons.Select(obj => obj.GetComponent<Button>()).ToArray();
        buttonContainers = RPSButtons.Select(obj => obj.transform.parent.gameObject).ToArray();
    }

    void Start() {
        ToggleChoices(false);
    }

    public void Enable() {
        animator.SetBool("done", true);
        ToggleChoices(true);
    }

    public void HighlightChoice(int choice) {
        for(int i = 0; i < buttonContainers.Length; i++) {
            buttons[i].interactable = false;
            print(i == choice);
            buttonContainers[i].SetActive(i == choice);
        }

        undoButton.SetActive(true);
        ToggleChoices(false);
    }

    public void ToggleChoices(bool state) {
        animator.SetBool("enabled", state);
    }

    public void ResetChoice() {
        undoButton.SetActive(false);

        for(int i = 0; i < buttonContainers.Length; i++) {
            buttonContainers[i].SetActive(true);
        }

        ToggleChoices(true);
    }
}
