using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    enum Result { Win, Lose, Draw }

    #region Serialized Fields
    [SerializeField] GameRules rules;
    [SerializeField] Gamepad gamepad;
    [Header("Players")]
    [SerializeField] PlayerManager player;
    [SerializeField] OpponentManager opponent;
    [SerializeField] bool AIMode = true;
    [Header("Managers")]
    [SerializeField] Countdown rps_countdown;
    [Header("Gameplay Texts")]
    [SerializeField] TextMeshProUGUI drawScoreText;
    [SerializeField] TextMeshProUGUI resultText;
    [Header("Gameplay Buttons")]
    [SerializeField] GameObject actionButtons;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject[] RPSButtons;
    [Header("Gameplay Scoreboard")]
    [SerializeField] GameObject[] scoreboardObjs;
    [Header("Animations")]
    [SerializeField] AnimationClip buttonStandbyClip;
    #endregion

    int drawScore = 0;
    const int NO_SELECTION = -1;
    Button[] buttons;
    GameObject[] buttonContainers;
    private IEnumerator countdownRoutine;

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
        yield return StartCoroutine(StandByAnimations());

        if(AIMode) {
            StartCoroutine(AITurn());
        }

        StartCoroutine(PlayerTurn());

        yield return new WaitUntil(CountdownDone);
        DisableChoices();
        StopCountdown();
        BattlePhase();
    }

    IEnumerator StandByAnimations() {
        actionButtons.SetActive(true);
        yield return new WaitForSeconds(buttonStandbyClip.length);
        gamepad.Enable();
        countdownRoutine = rps_countdown.StartCountdown();
        rps_countdown.gameObject.SetActive(true);
        StartCoroutine(countdownRoutine);
    }

    IEnumerator PlayerTurn() {
        yield return new WaitUntil(PlayerMoved);
        gamepad.HighlightChoice(player.Choice);
    }

    void StopCountdown() {
        StopCoroutine(countdownRoutine);
        rps_countdown.gameObject.SetActive(false);
        rps_countdown.Reset();
    }

    void BattlePhase() {
        Result result = (Result)rules.GetResult(player.Choice, opponent.Choice);
        RevealChoices();
        ResultPhase(result);
    }

    public void UndoChoice() {
        player.Undo();
        gamepad.ResetChoice();
        StartCoroutine(PlayerTurn());
    }

    void DisableChoices() {
        gamepad.ToggleChoices(false);
    }

    void RevealChoices() {
        player.RevealChoice();
        opponent.RevealChoice();
    }

    void ResultPhase(Result result) {
        switch(result) {
            case Result.Win:   Win(); break;
            case Result.Lose: Lose(); break;
            case Result.Draw: Draw(); break;
        }

        playAgainButton.SetActive(true);
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

    IEnumerator AITurn() {
        opponent.Move();
        yield return null;
    }

    bool CountdownDone() {
        return rps_countdown.Done;
    }

    bool PlayerMoved() {
        return player.Choice > NO_SELECTION;
    }

    bool AllPlayersMoved() {
        return (PlayerMoved() || rps_countdown.Done) && opponent.Choice > NO_SELECTION;
    }

    public void ResetPlayers() {
        player.Reset();
        opponent.Reset();
    }

    public void PlayAgain() {
        EndRound();
        StartGame();
    }

    void EndRound() {
        actionButtons.SetActive(false);
        playAgainButton.SetActive(false);
        rps_countdown.Reset();
        gamepad.ResetChoice();
        ResetPlayers();
    }
}
