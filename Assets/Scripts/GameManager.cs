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
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI resultText;
    [Header("Gameplay Buttons")]
    [SerializeField] GameObject actionButtons;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject[] buttonObjs;
    #endregion

    Result turnResult;
    int drawScore = 0;
    const int NO_SELECTION = -1;
    Button[] buttons;

    // Start is called before the first frame update
    void Start() {
        buttons = buttonObjs.Select(obj => obj.GetComponent<Button>()).ToArray();
    }

    public void PlayGame() {
        actionButtons.SetActive(true);
        StartGame(startGameButton.gameObject);
    }

    public void PlayAgain() {
        ResetChoice();
        resultText.text = "Rock Paper Scissors";

        StartGame(playAgainButton);
    }

    void StartGame(GameObject playButton) {
        rps_countdown.Reset();
        playButton.SetActive(false);
        StartCoroutine(StandByPhase());
    }

    IEnumerator StandByPhase() {
        StartCoroutine(rps_countdown.StartCountdown());

        if(AIMode) {
            yield return new WaitUntil(IsAITurn);
            StartCoroutine(AITurn());
        }

        yield return new WaitUntil(AllPlayersMoved);

        ShowChoice(player.Choice);
        BattlePhase();
    }

    void BattlePhase() {
        Result result = GetResult(player.Choice, opponent.Choice);

        ResultPhase(result);
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
        return PlayerMoved() || rps_countdown.Done;
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

    void ShowChoice(int choice) {

        for(int i = 0; i < buttonObjs.Length; i++) {
            buttons[i].interactable  = false;

            if(i == choice) {
                continue;
            }

            buttonObjs[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void ResetChoice() {
        player.Reset();
        opponent.Reset();

        for(int i = 0; i < buttonObjs.Length; i++) {
            buttons[i].interactable = true;
            buttonObjs[i].transform.parent.gameObject.SetActive(true);
        }
    }
}
