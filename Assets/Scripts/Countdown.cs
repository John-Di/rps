using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour {

    [SerializeField] protected Animator animator;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int COUNT;
	bool done = false;
	public bool Done {
        get {
            return done;
        }
        private set {
            done = value;
        }
    }

    public IEnumerator StartCountdown() {
        done = false;
        text.gameObject.SetActive(true);
        for(int i = COUNT; i > 0; i--) {
            text.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        done = true;
        text.gameObject.SetActive(false);
    }

    public void Reset() {
        done = false;
    }

}
