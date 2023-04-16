using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : PlayerManager {
    public void Move() {
        choice = Random.Range(0, buttons.Length);
        HideButtons(choice);
    }
}
