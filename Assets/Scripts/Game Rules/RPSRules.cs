using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSRules : GameRules {
    public override int GetResult(int player, int opponent) {
        if(IsDraw(player, opponent)) { return 1; }
        else if(IsTimeOut(player)) { return 0; }
        else if(IsTimeOut(opponent) || IsWin(player, opponent)) { return 2; }
        else { return 0; }
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
