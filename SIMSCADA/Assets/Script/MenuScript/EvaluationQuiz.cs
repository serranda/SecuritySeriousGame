using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationQuiz : MonoBehaviour
{
    public void GoToEvaluationQuiz()
    {
        Application.OpenURL(StaticDb.evaluationQuizLink);
    }
}
