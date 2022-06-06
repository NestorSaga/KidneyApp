using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private string getCategoriesEndpoint = "http://127.0.0.1:12345/categories/names";
    [SerializeField] private string getQuizzesEndpoint = "http://127.0.0.1:12345/quiz/getQuizzesFromCategoryName";
    [SerializeField] private string getQuestionsEndpoint = "http://127.0.0.1:12345/quiz/getQuestionsFromQuiz";
    [SerializeField] private string saveQuizEndpoint = "http://127.0.0.1:12345/quiz/saveQuiz";
    [SerializeField] private string quizScoreEndpoint = "http://127.0.0.1:12345/quiz/getHighscore";
    [SerializeField] private GameObject CategoryUI, quizSelector, quizDisplay, quizResult;
    [SerializeField] private GameObject buttonPrefab, quizButtonPrefab;
    [SerializeField] private GameObject categoryParent, quizParent;
    [SerializeField] private TMP_Text[] textAnswer1, textAnswer2, textAnswer3;
    [SerializeField] private TMP_Text progress, statement, resultText;
    [SerializeField] private Toggle[] answerToggles;
    [SerializeField] private Sprite noStars, oneStar, twoStars, ThreeStars;
    [SerializeField] private Image scoreDisplay;
    private string[] categories;
    private Quiz[] quizzes;
    private int currentQuestion = 0;
    private int[] answers;
    private Question[] questions;
    private string currentQuizId;

    private void Start() {
        Debug.Log("Attempting to get categories");
        StartCoroutine(TryGetCategories());
    }

    public void goToCategories() {
        populateCategories();
        quizSelector.SetActive(false);
        quizDisplay.SetActive(false);
        CategoryUI.SetActive(true);
        quizResult.SetActive(false);
    }

    public void goToQuizSelector(string category) {
        StartCoroutine(TryGetQuizzes(category));
        quizSelector.SetActive(true);
        quizDisplay.SetActive(false);
        CategoryUI.SetActive(false);
        quizResult.SetActive(false);
    }

    public void goToQuizDisplay(string quizId) {
        currentQuizId = quizId;
        StartCoroutine(TryGetQuestions(quizId));
        quizSelector.SetActive(false);
        quizDisplay.SetActive(true);
        CategoryUI.SetActive(false);
        quizResult.SetActive(false);
    }

    public void goToQuizResult(int score) {
        displayResult(score);
        quizSelector.SetActive(false);
        quizDisplay.SetActive(false);
        CategoryUI.SetActive(false);
        quizResult.SetActive(true);
    }

    private IEnumerator TryGetCategories() {

        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Post(getCategoriesEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                Debug.Log("Timed out");
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success) {

            CategoryResponse response = JsonUtility.FromJson<CategoryResponse>(request.downloadHandler.text);

            if(response.code == 0) {

                categories = response.names;
                populateCategories();

            } else {
                Debug.Log("Request was not successful");
            }

        } else {

            Debug.Log("Request failed");
        }

        yield return null;
    }

    private IEnumerator TryGetQuizzes(string category) {

        Debug.Log("Attempting to get quizzes");

        WWWForm form = new WWWForm();
        form.AddField("rCategory", category);
        UnityWebRequest request = UnityWebRequest.Post(getQuizzesEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                Debug.Log("Timed out");
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success) {

            QuizResponse response = JsonUtility.FromJson<QuizResponse>("{\"quizzes\":" + request.downloadHandler.text + "}");

            quizzes = response.quizzes;

            

        } else {

            Debug.Log("Request failed");
        }

        foreach(Quiz quiz in quizzes)
        {
            form = new WWWForm();
            form.AddField("rUserId", PlayerPrefs.GetString("userId"));
            form.AddField("rQuizId", quiz._id);
            request = UnityWebRequest.Post(quizScoreEndpoint, form);

            handler = request.SendWebRequest();

            startTime = 0.0f;
            while (!handler.isDone)
            {
                startTime += Time.deltaTime;

                if (startTime > 10.0f)
                {
                    Debug.Log("Timed out");
                    break;
                }

                yield return null;
            }

            if (request.result == UnityWebRequest.Result.Success)
            {

                ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(request.downloadHandler.text);

                if(response.code == 0)
                {
                    quiz.score = response.score;
                }

            }
            else
            {

                Debug.Log("Request failed");
            }
        }

        populateQuizzes();
        yield return null;
    }

    private IEnumerator TryGetQuestions(string quizId) {

        WWWForm form = new WWWForm();
        form.AddField("rQuizId", quizId);
        UnityWebRequest request = UnityWebRequest.Post(getQuestionsEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                Debug.Log("Timed out");
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success) {

            QuestionResponse response = JsonUtility.FromJson<QuestionResponse>("{\"questions\":" + request.downloadHandler.text + "}");

            questions = response.questions;
            startQuiz();

        } else {

            Debug.Log("Request failed");
        }

        yield return null;
    }

    public void populateCategories() {

        foreach (Transform child in categoryParent.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string cat in categories)
        {
            GameObject button = Instantiate(buttonPrefab, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
            button.GetComponent<ButtonManagerWithIcon>().buttonText = cat;
            button.transform.SetParent(categoryParent.transform, false);
            button.GetComponent<Button>().onClick.AddListener(delegate{goToQuizSelector(cat);});
        }

    }

    public void populateQuizzes() {

        foreach (Transform child in quizParent.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Quiz quiz in quizzes)
        {
            GameObject button = Instantiate(quizButtonPrefab, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
            button.GetComponent<ButtonManagerWithIcon>().buttonText = quiz.name;
            button.GetComponent<ButtonManagerWithIcon>().buttonIcon = getQuizIcon(quiz);
            button.transform.SetParent(quizParent.transform, false);
            button.GetComponent<Button>().onClick.AddListener(delegate{goToQuizDisplay(quiz._id);});
        }

    }

    public Sprite getQuizIcon(Quiz quiz)
    {
        if (quiz.score == 1)
        {
            return oneStar;
        } else if (quiz.score == 2)
        {
            return twoStars;
        } else if(quiz.score == 3)
        {
            return ThreeStars;
        } else
        {
            return noStars;
        }
    }

    public void startQuiz() {
        answers = new int[questions.Length];
        currentQuestion = 0;

        populateQuestionText();
    }

    public void next() {
        if(currentQuestion + 1 == questions.Length) {
            saveCurrentAnswer();
            int score = calculateScore();
            PlayerPrefs.SetInt("quizCount", PlayerPrefs.GetInt("quizCount") + 1);
            StartCoroutine(SaveScore(score));
            goToQuizResult(score);

        } else {
            saveCurrentAnswer();
            currentQuestion++;
            populateQuestionText();
        }
    }

    public void saveCurrentAnswer()
    {
        if (answerToggles[0].isOn)
        {
            answers[currentQuestion] = 1;
        }
        else if (answerToggles[1].isOn)
        {
            answers[currentQuestion] = 2;
            answerToggles[1].isOn = false;
            answerToggles[0].isOn = true;
        }
        else
        {
            answers[currentQuestion] = 3;
            answerToggles[2].isOn = false;
            answerToggles[0].isOn = true;
        }
    }

    public void back() {
        if(currentQuestion == 0) {
            goToCategories();
        } else {
            currentQuestion--;
            populateQuestionText();
        }
    }

    public void populateQuestionText() {

        progress.text = "Question " + (currentQuestion + 1) + "/" + questions.Length;
        statement.text = questions[currentQuestion].statement;

        textAnswer1[0].text = questions[currentQuestion].answers._1;
        textAnswer1[1].text = questions[currentQuestion].answers._1;

        textAnswer2[0].text = questions[currentQuestion].answers._2;
        textAnswer2[1].text = questions[currentQuestion].answers._2;

        textAnswer3[0].text = questions[currentQuestion].answers._3;
        textAnswer3[1].text = questions[currentQuestion].answers._3;
    }

    public void goToHub() {
        GameManager.Instance.ChangeScene(2);
    }

    public int calculateScore() {
        int score = 1;

        /* 
            Score matches the number of stars in a quiz.
            1 star: Quiz completed.
            2 stars: 50% or more correct answers
            3 stars: 100% correct answers         
        */

        int points = 0;
        for (int i = 0; i < questions.Length; i++)
        {
            if (answers[i] == questions[i].correctAnswer)
            {
                points++;
            }
        }

        if (points == questions.Length)
        {
            score = 3;

        }
        else if (points >= questions.Length / 2)
        {
            score = 2;
        }

        return score;
    }

    public IEnumerator SaveScore(int score)
    {
        Debug.Log("Saving score");

        WWWForm form = new WWWForm();
        form.AddField("rScore", score);
        form.AddField("rUserId", PlayerPrefs.GetString("userId"));
        form.AddField("rQuizId", currentQuizId);
        UnityWebRequest request = UnityWebRequest.Post(saveQuizEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                Debug.Log("Timed out");
                break;
            }

            yield return null;
        }


        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score saved");
        }
        else
        {
            Debug.Log("Request failed");
        }

        yield return null;
    }

    public void displayResult(int score) {
        if(score == 3) {
            scoreDisplay.sprite = ThreeStars;
            resultText.text = "You made no mistakes!";
        } else if( score == 2) {
            scoreDisplay.sprite  = twoStars;
            resultText.text = "Very well!";
        } else {
            scoreDisplay.sprite = oneStar;
            resultText.text = "You made some mistakes!";
        }
    }

}
