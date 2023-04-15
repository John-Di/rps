using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerScoreText, opponentScoreText, drawScoreText, resultText;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject[] playerButtons, opponentButtons;
    int playerScore = 0, opponentScore = 0, drawScore = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(int choice) {
        int opponent = Random.Range(0, 3);
        Output("Player", choice, playerButtons);
        Output("Opponent", opponent, opponentButtons);
        Result(choice, opponent);
    }

    void Result(int player, int opponent) {
        string result;
        if(IsDraw(player, opponent)) { result = "Draw!"; drawScore++; print("draw"); }
        else if(isWin(player, opponent)) { result = "You Win!"; playerScore++; print("Win"); }
        else { result = "You Lose!"; opponentScore++; print("Lose"); }

        resultText.text = result;
        playerScoreText.text = playerScore.ToString();
        drawScoreText.text = drawScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
    }

    bool IsDraw(int player, int opponent) {
        return player == opponent;
    }

    bool isWin(int player, int opponent) {
        return (player == 0 && opponent == 2) || (player == 2 && opponent == 1) || (player == 1 && opponent == 0);
    }

    void Output(string player, int choice, GameObject[] buttons) {
        string choiceText = "";
        switch(choice) {
            case 0: choiceText = "Rock"; break;
            case 1: choiceText = "Paper"; break;
            case 2: choiceText = "Scissors"; break;
        }

        for(int i = 0; i < buttons.Length; i++) {
            GameObject button = buttons[i];
            button.GetComponent<Button>().enabled = false;

            if(i == choice) {
                continue;
            }

            button.transform.parent.gameObject.SetActive(false);
        }

        playAgainButton.SetActive(true);
        Debug.Log(string.Format("{0}: {1}", player, choiceText));
    }

    public void PlayAgain() {
        resultText.text = "Rock Paper Scissors";

        foreach(GameObject buttonObj in playerButtons) {
            Button button = buttonObj.GetComponent<Button>();
            buttonObj.GetComponent<Button>().enabled = true;
            button.transform.parent.gameObject.SetActive(true);
        }

        foreach(GameObject buttonObj in opponentButtons) {
            Button button = buttonObj.GetComponent<Button>();
            button.transform.parent.gameObject.SetActive(true);
        }

        playAgainButton.SetActive(false);
    }
}
