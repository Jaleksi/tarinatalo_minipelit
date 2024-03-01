using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextAsset textJSON;
    public ExerciseArray myExcercises;

    private static List<Exercise> unansweredExercises;
    private Exercise currentExercise;

    [SerializeField]
    private Text sentence;

    [SerializeField]
    private Text leftButtonText;

    [SerializeField]
    private Text rightButtonText;


    // Start is called before the first frame update
    void Start()
    {
        //myExcercises = new ExerciseArray();
        myExcercises = JsonUtility.FromJson<ExerciseArray>(textJSON.text);

        if (unansweredExercises == null || unansweredExercises.Count == 0)
        {
            unansweredExercises = myExcercises.exercises.ToList<Exercise>();
           
        }

        SetCurrentExercise();
        Debug.Log(currentExercise.sentence);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetCurrentExercise ()
    {
        int randomExerciseIndex = Random.Range(0, unansweredExercises.Count);
        currentExercise = unansweredExercises[randomExerciseIndex];
   
        sentence.text = currentExercise.sentence;
        leftButtonText.text = currentExercise.word1;
        rightButtonText.text = currentExercise.word2;    
        unansweredExercises.RemoveAt(randomExerciseIndex);
    }

    public void UserSelectsLeftButton ()
    {
        if (leftButtonText.text.Equals(currentExercise.rightAnswer))
        {
            Debug.Log("Correct answer");
        } else
        {
            Debug.Log("Wrong answer");
        }
    }

    public void UserSelectRightButton ()
    {
        if (rightButtonText.text.Equals(currentExercise.rightAnswer))
        {
            Debug.Log("Correct answer");
        }else
        {
            Debug.Log("Wrong answer");
        }
       
    }
}
