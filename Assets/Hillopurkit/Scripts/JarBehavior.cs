using UnityEngine;
using System.Collections;

public class JarBehavior : MonoBehaviour
{
    private bool IsCorrectAnswer = false;
    [SerializeField] private GameObject brokenJar;
    [SerializeField] private GameObject jam;
    private Color color;

    private void Start()
    {
        color = GetComponentInChildren<RandomMaterial>().GetMaterial().color;
    }

    public void SetIsCorrectAnswer(bool _isCorrectAnswer)
    {
        IsCorrectAnswer = _isCorrectAnswer;
    }

    // Activates from HammerBehavior when player clicks a jar.
    // Clicking on a correct jar breaks it and starts the next round. Wrong jar only wobbles.
    // Only works when game is unpaused.
    public void HitByHammer()
    {
        if (MiniGameManager.isGamePaused)
            return;

        Score score = GameObject.Find("Score").GetComponent<Score>();
        MiniGameManager miniGameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();

        if (IsCorrectAnswer)
        {
            miniGameManager.hammer.GetComponent<HammerBehavior>().SetCanSwing(false); // stop hammer from swinging until next round starts
            score.BrokeCorrectJar(true); // update score
            StartCoroutine(miniGameManager.NextRound()); // start next round
            
            // move out of the way and spawn in broken jar
            Vector3 currentPosition = transform.position;
            GameObject breakingJar = Instantiate(brokenJar, currentPosition, Quaternion.identity);

            // screen shake effect
            ScreenShake.shakeTrigger = true;

            // set the dust cloud effect to same color as the jar
            ParticleSystem ps = breakingJar.GetComponent<BrokenJarBehavior>().dustCloud;
            ParticleSystem.MainModule main = ps.main;
            main.startColor = color + new Color(0.33f, 0.33f, 0.33f); // a little lighter than the original color

            gameObject.transform.position = currentPosition + new Vector3 (30, 0, 0); // NextRound() despawns the jar.
        }

        else
        {
            StartCoroutine(miniGameManager.hammer.GetComponent<HammerBehavior>().WrongSwing());
            score.BrokeCorrectJar(false); // update score
            transform.parent.gameObject.GetComponent<Animator>().Play("WrongJarShake");
            GetComponent<Animator>().Play("WrongJar");
        }
    }
}
