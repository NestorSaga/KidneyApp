using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answers {
    public string _1;
    public string _2;
    public string _3;
}

[System.Serializable]
public class Question {
    public string quizId;
    public string statement;
    public Answers answers;
    public int correctAnswer;
    public string language;
}

[System.Serializable]
public class QuestionResponse
{
    public Question[] questions;
}
