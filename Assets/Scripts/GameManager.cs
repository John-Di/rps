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
    [SerializeField] TextMeshProUGUI drawScoreText, resultText;
    [SerializeField] GameObject[] buttonObjs;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] PlayerManager player;
    [SerializeField] OpponentManager opponent;
    [SerializeField] bool AIMode = true;
    #endregion

    Result turnResult;
    int drawScore = 0;
    const int NO_SELECTION = -1;
    Button[] buttons;

    // Start is called before the first frame update
    void Start() {
        buttons = buttonObjs.Select(obj => obj.GetComponent<Button>()).ToArray();
        StartCoroutine(StandByPhase());
    }

    IEnumerator StandByPhase() {
        if(AIMode) {
            yield return new WaitUntil(PlayerMoved);
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
        else if(IsWin(player, opponent)) { return Result.Win; }
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

    bool PlayerMoved() {
        return player.Choice > NO_SELECTION;
    }

    bool AllPlayersMoved() {
        return PlayerMoved() && opponent.Choice > NO_SELECTION;
    }

    bool IsDraw(int player, int opponent) {
        return player == opponent;
    }

    bool IsWin(int player, int opponent) {
        return (player == 0 && opponent == 2) || (player == 2 && opponent == 1) || (player == 1 && opponent == 0);
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

    public void PlayAgain() {
        ResetChoice();

        resultText.text = "Rock Paper Scissors";
        playAgainButton.SetActive(false);

        StartCoroutine(StandByPhase());
    }
}
