using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DisplayName {
    public string en;
    public string es;
}

[System.Serializable]
public class Quiz {
    public string _id;
    public string name;
    public string categoryId;
    public DisplayName displayName;
}

[System.Serializable]
public class QuizResponse
{
    public Quiz[] quizzes;
}
