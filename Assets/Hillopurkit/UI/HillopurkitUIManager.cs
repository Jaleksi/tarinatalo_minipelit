using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HillopurkitUIManager : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    private VisualElement root;
    private VisualElement panelSection;
    private Label panelHeadline;
    private Label panelText;
    private Button panelButton;
    private VisualElement instructions;
    private ProgressBar progressBar;
    private Label topText;
    private Label tallyText;
    private Label clickedWrong;
    private Label clickedRight;

    private string continueButtonText = "<allcaps>jatka</allcaps>";
    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private string winningHeadline = "Läpäisit pelin!";
    private string winningText = "Löysit ja rikoit kaikki joukkoon kuulumattomat purkit!";

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        ResetProgressBar();

        Button instructionButton = root.Q<Button>("instruction-button");
        Button exitButton = root.Q<Button>("exit-button");

        panelSection = root.Q<VisualElement>("panel-section");
        panelHeadline = panelSection.Q<Label>("panel-headline");
        panelText = panelSection.Q<Label>("panel-text");
        panelButton = panelSection.Q<Button>("panel-button");

        clickedRight = root.Q<Label>("feedback-right");
        clickedWrong = root.Q<Label>("feedback-wrong");

        SetInstructions();

        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => SetPanelExit();
        exitButton.clicked += () => Application.Quit();

    }

    private void SetInstructions()
    {
        // Tälleen nyt, lopullisessa varmaan parempi jos luetaan txt tiedostosta tms. ohjeet
        string instructionTextText = "Kaappiin on kasattu hillopurkkeja, joiden kyljessä lukee synonyymejä. "
                                    + "Mutta purkkien joukkoon on eksynyt sana, joka ei kuulu joukkoon. "
                                    + "Etsi joukoon kuulumaton purkki, ja riko se vasaralla!";

        miniGameManager.PauseGame();

        instructions = root.Q<VisualElement>("panel-section");
        Label instructionHeadline = instructions.Q<Label>("panel-headline");
        instructionHeadline.text = instructionHeadlineText;
        Label instructionText = instructions.Q<Label>("panel-text");
        instructionText.text = instructionTextText;
        Button gotItButton = instructions.Q<Button>("panel-button");
        gotItButton.text = gotItButtonText;

        instructions.style.display = DisplayStyle.Flex;
    }

    private void SetPanelExit()
    {
        if (panelButton.text.Equals(continueButtonText)) // onko aina false?
        {
            ContinueGame();
        }

        else if (panelButton.text.Equals(endGameButtonText)) // minipelin lopussa oleva nappi "Palaa pääpeliin"
        {
            Application.Quit();
        }

        else
        {
            instructions.style.display = DisplayStyle.None; // kysymysmerkistä poistuminen, pelin alun "Selvä!"
            miniGameManager.UnpauseGame();
        }
    }

    private void ContinueGame()
    {
        panelSection.style.display = DisplayStyle.None;
    }

    public void DeclareWin()
    {
        int[] tally = miniGameManager.GetTally();
        int total = tally[0] + tally[1];

        panelHeadline.text = winningHeadline;
        panelText.text = winningText
            + ("\nArvausten määrä: " + total)
            + ("\nOikeat arvaukset: " + tally[0])
            + ("\nVäärät arvaukset: " + tally[1]);

        panelButton.text = endGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;
    }

    public void UpProgressBar(float points)
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = points;

        if (progressBar.value >= 33f)
        {
            VisualElement star1 = root.Q<VisualElement>("star1");
            star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        }

        if (progressBar.value >= 66f)
        {
            VisualElement star2 = root.Q<VisualElement>("star2");
            star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        }

        if (progressBar.value >= 98f)
        {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        }
    }

    public void ResetProgressBar()
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = 0;
        VisualElement star1 = root.Q<VisualElement>("star1");
        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
        VisualElement star2 = root.Q<VisualElement>("star2");
        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
        VisualElement star3 = root.Q<VisualElement>("star3");
        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
    }

    public void SetFeedback(bool result)
    {
        //topText = root.Q<Label>("instructions");

        int[] tally = miniGameManager.GetTally();

        if (result == true)
        {
            //topText.text = ("Yhdessä hillopurkissa oleva sana ei kuulu joukkoon. Etsi se, ja klikkaa se rikki! \nRIKOIT OIKEAN PURKIN!"); //tämän projektin purkkamaisin ratkaisu
            tallyText = root.Q<Label>("click-tally-right");
            tallyText.text = ("Särjetyt purkit: " + tally[0]);
            clickedRight.visible = true;
            StartCoroutine(FeedbackTurnOffDelay(clickedRight));

        }
        else
        {
            //topText.text = ("Yhdessä hillopurkissa oleva sana ei kuulu joukkoon. Etsi se, ja klikkaa se rikki! \nVÄÄRÄ PURKKI, YRITÄ UUDESTAAN");
            tallyText = root.Q<Label>("click-tally-wrong");
            tallyText.text = ("Väärät arvaukset: " + tally[1]);
            clickedWrong.visible = true;
            StartCoroutine(FeedbackTurnOffDelay(clickedWrong));
        }
    }

    private IEnumerator FeedbackTurnOffDelay(Label feedbackMsg)
    {
        yield return new WaitForSeconds(1f);
        feedbackMsg.visible = false;    
    }
}