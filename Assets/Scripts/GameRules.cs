using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameRules : MonoBehaviour {
    public abstract IEnumerator StandByPhase();
    public abstract IEnumerator PlayerTurn();
    public abstract void ResultPhase(int result);
    public abstract void BattlePhase(int result);
    public abstract void EndRound();
    public abstract int GetResult(int player, int opponent);
    protected abstract bool IsDraw(int player, int opponent);
    protected abstract bool IsWin(int player, int opponent);

    protected abstract bool IsLose(int player, int opponent);

    protected abstract bool IsTimeOut(int player);
}
