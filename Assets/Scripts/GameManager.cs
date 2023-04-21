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
    [SerializeField] GameObject undoButton;
    [SerializeField] Button[] gameplayButtons;
    [SerializeField] GameObject[] RPSButtons;
    [Header("Gameplay Scoreboard")]
    [SerializeField] GameObject[] scoreboardObjs;
    [Header("Animations")]
    [SerializeField] Animator actionButtonsAnim;
    [SerializeField] AnimationClip buttonStandbyClip;
    #endregion

    Result turnResult;
    int drawScore = 0;
    const int NO_SELECTION = -1;
    Button[] buttons;
    GameObject[] buttonContainers;
    private IEnumerator countdownRoutine;

    // Start is called before the first frame update
    void Start() {
        buttons = RPSButtons.Select(obj => obj.GetComponent<Button>()).ToArray();
        buttonContainers = RPSButtons.Select(obj => obj.transform.parent.gameObject).ToArray();
        StartGame();
    }

    void StartGame() {
        rps_countdown.Reset();
        ToggleScoreBoard(true);
        playAgainButton.SetActive(false);
        StartCoroutine(StandByPhase());
    }

    public void PlayAgain() {
        actionButtons.SetActive(false);
        ResetPlayers();
        ResetChoice();
        StartGame();
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
        DisableChoices();
        actionButtons.SetActive(true);
        yield return new WaitForSeconds(buttonStandbyClip.length);
        actionButtonsAnim.SetBool("done", true);
        EnableChoices();
        countdownRoutine = rps_countdown.StartCountdown();
        rps_countdown.gameObject.SetActive(true);
        StartCoroutine(countdownRoutine);
    }

    IEnumerator PlayerTurn() {
        yield return new WaitUntil(PlayerMoved);
        HighlightChoice(player.Choice);
        DisableChoices();
    }

    void StopCountdown() {
        StopCoroutine(countdownRoutine);
        rps_countdown.gameObject.SetActive(false);
        rps_countdown.Reset();
    }

    void BattlePhase() {
        Result result = GetResult(player.Choice, opponent.Choice);
        RevealChoices();
        ResultPhase(result);
    }

    void HighlightChoice(int choice) {
        for(int i = 0; i < buttonContainers.Length; i++) {
            buttons[i].interactable = false;
            if(i == choice) { continue; }
            buttonContainers[i].SetActive(false);
        }

        undoButton.SetActive(true);
    }
    void ToggleChoices(bool state) {
        actionButtonsAnim.SetBool("enabled", state);
    }

    void EnableChoices() {
        ToggleChoices(true);
    }

    void DisableChoices() {
        ToggleChoices(false);
    }

    void RevealChoices() {
        player.RevealChoice();
        opponent.RevealChoice();
    }

    Result GetResult(int player, int opponent) {
        if(IsDraw(player, opponent)) { return Result.Draw; }
        else if(IsTimeOut(player)) { return Result.Lose; }
        else if(IsTimeOut(opponent) || IsWin(player, opponent)) { return Result.Win; }
        else { return Result.Lose; }
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

    bool IsAITurn() {
        return rps_countdown.Done;
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

    bool IsDraw(int player, int opponent) {
        return player == opponent;
    }

    bool IsWin(int player, int opponent) {
        return (player == 0 && opponent == 2) || (player == 2 && opponent == 1) || (player == 1 && opponent == 0);
    }

    bool IsTimeOut(int player) {
        return player < 0 || player > 2;
    }

    public void UndoChoice() {
        player.Undo();
        ResetChoice();
        StartCoroutine(PlayerTurn());
    }

    public void ResetPlayers() {
        player.Reset();
        opponent.Reset();
    }

    public void ResetChoice() {
        undoButton.SetActive(false);
        EnableChoices();

        for(int i = 0; i < buttonContainers.Length; i++) {
            buttonContainers[i].SetActive(true);
        }
    }
}
