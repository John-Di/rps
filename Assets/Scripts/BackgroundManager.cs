using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour {
    [SerializeField] Image backgroundCanvas;
    [SerializeField] float cycleTime;
    [SerializeField] Color[] backgroundColors;

    private int targetColor = 0;
    public Color current;

    // Start is called before the first frame update
    void Start() {
        if(backgroundColors.Length > 0) {
            backgroundCanvas.color = backgroundColors[0];
        }

        if(backgroundColors.Length > 1) {
            StartCoroutine(FadeInCoroutine(backgroundColors[0], backgroundColors[targetColor++]));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeInCoroutine(Color start, Color final) {
        float elapsedTime = 0;

        while(!backgroundCanvas.color.Equals(final)) {
            backgroundCanvas.color = Color.Lerp(start, final, elapsedTime);
            current = backgroundCanvas.color;
            elapsedTime += Time.deltaTime / cycleTime;
            yield return null;
        }

        backgroundCanvas.color = final;
        StartCoroutine(FadeInCoroutine(final, backgroundColors[++targetColor % backgroundColors.Length]));
    }

    public bool HasLerped(Vector3 position, Vector3 destination) {
        return (destination - position).sqrMagnitude <= Mathf.Epsilon;
    }

    public bool HasArrived(Vector3 position, Vector3 destination) {
        return (destination - position).sqrMagnitude <= Mathf.Epsilon;
    }


    protected IEnumerator LerpPosition(GameObject obj) {
        // NPCMovement mv = obj.GetComponent<NPCMovement>();
        // Vector3 originPos = spawnPoint;
        // Vector3 destination = spawnDestination;
        // float elapsedTime = 0;

        // while(!HasArrived(destination, obj.transform.position)) {
        //     obj.transform.position = Vector3.Lerp(
        //         originPos,
        //         destination,
        //         (elapsedTime / (SPAWNER_TIMER / 1.5f)));
        //     elapsedTime += Time.deltaTime;
            yield return null;
        // }

        // obj.transform.position = destination;
        // mv.setVelocity(destination - originPos);
        // mv.enabled = true;
    }
}
