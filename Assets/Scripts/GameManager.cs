using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] GameRules rules;
    [SerializeField] Gamepad gamepad;
    [Header("Players")]
    [SerializeField] PlayerManager player;
    [SerializeField] OpponentManager opponent;
    [Header("Gameplay Texts")]
    [SerializeField] TextMeshProUGUI drawScoreText;
    [SerializeField] TextMeshProUGUI resultText;
    [Header("Gameplay Scoreboard")]
    [SerializeField] GameObject[] scoreboardObjs;
    #endregion

    int drawScore = 0;

    // Start is called before the first frame update
    void Start() {
        StartGame();
    }

    void StartGame() {
        ToggleScoreBoard(true);
        StartCoroutine(StandByPhase());
    }

    public void ToggleScoreBoard(bool gameStarted) {
        foreach(GameObject panel in scoreboardObjs) {
            panel.SetActive(!gameStarted);
        }

        if(gameStarted) {
            resultText.text = "Choose Now";
        }
    }

    IEnumerator StandByPhase() {
        yield return StartCoroutine(rules.StandByPhase());
        BattlePhase();
    }

    IEnumerator PlayerTurn() {
        yield return rules.PlayerTurn();
    }

    void BattlePhase() {
        int result = rules.GetResult(player.Choice, opponent.Choice);
        rules.BattlePhase(result);
        ResultPhase(result);
    }

    public void UndoChoice() {
        player.Undo();
        gamepad.ResetChoice();
        StartCoroutine(PlayerTurn());
    }

    void ResultPhase(int result) {
        switch(result) {
            case 0:  Win(); break;
            case 1: Lose(); break;
            case 2: Draw(); break;
        }
        rules.ResultPhase(result);
        ToggleScoreBoard(false);
    }

    void Win() {
        player.Win();
        opponent.Lose();
        resultText.text = "You Win!";
    }

    void Lose(){
        player.Lose();
        opponent.Win();
        resultText.text = "You Lose!";
    }

    void Draw() {
        player.Draw();
        opponent.Draw();
        drawScoreText.text = (++drawScore).ToString();
        resultText.text = "Draw!";
    }

    public void PlayAgain() {
        rules.EndRound();
        StartGame();
    }
}
