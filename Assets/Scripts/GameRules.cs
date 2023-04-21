using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameRules : MonoBehaviour {
    public abstract int GetResult(int player, int opponent);
    protected abstract bool IsDraw(int player, int opponent);
    protected abstract bool IsWin(int player, int opponent);

    protected abstract bool IsLose(int player, int opponent);

    protected abstract bool IsTimeOut(int player);
}
