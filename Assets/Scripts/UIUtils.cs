using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIUtils : MonoBehaviour
{
    public bool isStreakColoringOn = false;

    private Color streakScoreLabelColor = new Color(0.81f, 0.57f, 0.24f);
    private TextShadow streakShadow = new TextShadow
    {
        color = Color.white,
        blurRadius = 3f,
        offset = new Vector2(2f, 2f)
    };

    private TextShadow normalShadow = new TextShadow
    {
        color = new Color(0.466f, 0.26f, 0.18f),
        blurRadius = 0f,
        offset = new Vector2(3f, 3f)
    };
   

    public Label ScoreLabelToStreakColoring(Label gameScore)
    { 
        gameScore.style.color = streakScoreLabelColor;
        gameScore.style.textShadow = new StyleTextShadow(streakShadow);

        isStreakColoringOn = true;

        return gameScore;
    }

    public Label ScoreLabelToNormalColoring(Label gameScore)
    {
        gameScore.style.color = Color.white;
        gameScore.style.textShadow = new StyleTextShadow(normalShadow);

        isStreakColoringOn = false;

        return gameScore;
    }

    public void SetConfirmationPanel(VisualElement root)
    {
        VisualElement exitConfirmationPanelSection = root.Q<VisualElement>("exit-confirmation-panel-section");

        Button yesButton = exitConfirmationPanelSection.Q<Button>("exit-yes-button");
        yesButton.text = "kyll�";

        Button noButton = exitConfirmationPanelSection.Q<Button>("exit-no-button");
        noButton.text = "ei";

        Label exitQuestion = exitConfirmationPanelSection.Q<Label>("exit-text");
        exitQuestion.text = "Suljetaanko synonyymipeli?";

        exitConfirmationPanelSection.style.display = DisplayStyle.Flex;

        yesButton.clicked += () => Application.Quit(); 
        noButton.clicked += () => exitConfirmationPanelSection.style.display = DisplayStyle.None;

       
    }


}
