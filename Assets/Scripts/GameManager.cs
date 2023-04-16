using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    enum Result { Win, Lose, Draw }

    #region Serialized Fields
    [SerializeField] TextMeshProUGUI drawScoreText, resultText;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] PlayerManager player;
    [SerializeField] OpponentManager opponent;
    [SerializeField] bool AIMode = true;
    #endregion

    Result turnResult;
    int drawScore = 0;
    const int NO_SELECTION = -1;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(StandByPhase());
    }

    IEnumerator StandByPhase() {
        if(AIMode) {
            yield return new WaitUntil(PlayerMoved);
            StartCoroutine(AITurn());
        }

        yield return new WaitUntil(AllPlayersMoved);

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

    public void PlayAgain() {
        player.HideChoice();
        opponent.HideChoice();

        resultText.text = "Rock Paper Scissors";
        playAgainButton.SetActive(false);

        StartCoroutine(StandByPhase());
    }
}
