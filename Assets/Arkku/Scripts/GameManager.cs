using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextAsset textJSON;
    public ExerciseArray myExcercises;

    public UIManager ui;
    public int numberOfExercisesToLoop;
   

    private static List<Exercise> allExercises;
    private static List<Exercise> exercisesToAnswer; 
    private Exercise currentExercise;
    private Exercise previousExercise;
    private float points = 66f;
    public float pointsPerCorrectAnswer = 6.66f;
    public float pointsToWin = 99f;
    

    void Start()
    {
        //Haetaan harjoitukset JSONista
        myExcercises = JsonUtility.FromJson<ExerciseArray>(textJSON.text);

        //Arvotaan joukosta tietty määrä harjoituksia, joita pyöritetään pelissä
        if (allExercises == null || allExercises.Count == 0)
        {
            allExercises = myExcercises.exercises.ToList<Exercise>();  

            exercisesToAnswer = new List<Exercise>();

            for(int i = 0; i < numberOfExercisesToLoop; i++) {
                int randomExerciseIndex = Random.Range(0, allExercises.Count);
                exercisesToAnswer.Add(allExercises[randomExerciseIndex]);
                allExercises.RemoveAt(randomExerciseIndex);
            }  
        }
        SetCurrentExercise();
 
    }

    void Update()
    {
        
    }

    public void SetCurrentExercise ()
    {
        if (exercisesToAnswer.Count > 0) { 
            
            if(currentExercise == null) {
                currentExercise = exercisesToAnswer[0];
            } 
            else 
            {
                while (previousExercise.Equals(currentExercise)) {
                int randomExerciseIndex = Random.Range(0, exercisesToAnswer.Count);
                currentExercise = exercisesToAnswer[randomExerciseIndex];
                }
            }
            ui.SetSentence(currentExercise.sentence);
            ui.SetLeftWord(currentExercise.word1);
            ui.SetRightWord(currentExercise.word2);
            
        }
   
    }

    public bool IsAnswerCorrect (string answer)
    {
        previousExercise = currentExercise;

        if (answer.Equals(currentExercise.correctAnswer)) 
        {
            points += pointsPerCorrectAnswer;
            exercisesToAnswer.Remove(currentExercise); 
            return true;
        }
        else
        {
            return false;
        }

        
    }

    public bool CheckIfGameEnded ()
    {
        return points >= pointsToWin ? true : false; 
    }

    public float GetPoints()
    {
        return points;
    }

    public float GetPointsToWin ()
    {
        return pointsToWin;
    }

    public string GetCurrentExplanation () 
    {
        return currentExercise.explanation;
    }
}
