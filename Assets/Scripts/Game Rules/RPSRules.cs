using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RPSRules : GameRules {
    enum Result { Win, Lose, Draw }
    [SerializeField] PlayerManager player;
    [SerializeField] OpponentManager opponent;
    [SerializeField] Countdown countdown;
    [SerializeField] Gamepad gamepad;
    [SerializeField] GameObject actionButtons;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] bool AIMode = true;
    const int NO_SELECTION = -1;
    Button[] buttons;
    GameObject[] buttonContainers;
    private IEnumerator countdownRoutine;
    [Header("Animations")]
    [SerializeField] AnimationClip gamepadStandbyClip;

    #region Phases
    public override IEnumerator StandByPhase() {
        yield return StartCoroutine(StandByAnimations());

        if(AIMode) {
            StartCoroutine(AITurn());
        }

        StartCoroutine(PlayerTurn());

        yield return new WaitUntil(CountdownDone);
        DisableChoices();
        StopCountdown();
    }

    public override void ResultPhase(int r) {
        playAgainButton.SetActive(true);
    }

    public override void BattlePhase(int r) {
        RevealChoices();
    }

    bool PlayerMoved() {
        return player.Choice > NO_SELECTION;
    }

    public override IEnumerator PlayerTurn() {
        yield return new WaitUntil(PlayerMoved);
        gamepad.HighlightChoice(player.Choice);
    }

    IEnumerator AITurn() {
        opponent.Move();
        yield return null;
    }

    bool CountdownDone() {
        return countdown.Done;
    }

    void RevealChoices() {
        player.RevealChoice();
        opponent.RevealChoice();
    }


    void DisableChoices() {
        gamepad.ToggleChoices(false);
    }

    IEnumerator StandByAnimations() {
        actionButtons.SetActive(true);
        yield return new WaitForSeconds(gamepadStandbyClip.length);
        gamepad.Enable();
        countdownRoutine = countdown.StartCountdown();
        countdown.gameObject.SetActive(true);
        StartCoroutine(countdownRoutine);
    }

    void StopCountdown() {
        StopCoroutine(countdownRoutine);
        countdown.gameObject.SetActive(false);
        countdown.Reset();
    }

    void Reset() {
        countdown.Reset();
        gamepad.Reset();
        ResetPlayers();
    }

    public override void EndRound() {
        // actionButtons.SetActive(false);
        Reset();
    }

    public void ResetPlayers() {
        player.Reset();
        opponent.Reset();
    }

    #endregion

    public override int GetResult(int player, int opponent) {
        if(IsDraw(player, opponent)) { return 2; }
        else if(IsTimeOut(player)) { return 1; }
        else if(IsTimeOut(opponent) || IsWin(player, opponent)) { return 0; }
        else { return 1; }
    }

    protected override bool IsDraw(int player, int opponent) {
        return player == opponent;
    }

    protected override bool IsWin(int player, int opponent) {
        return (player == 0 && opponent == 2) || (player == 2 && opponent == 1) || (player == 1 && opponent == 0);
    }

    protected override bool IsLose(int player, int opponent) {
        return (player == 0 && opponent == 1) || (player == 1 && opponent == 2) || (player == 2 && opponent == 0);
    }

    protected override bool IsTimeOut(int player) {
        return player < 0 || player > 2;
    }
}
