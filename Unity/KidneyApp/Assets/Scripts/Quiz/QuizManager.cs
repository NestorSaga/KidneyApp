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
    [SerializeField] private GameObject CategoryUI;
    [SerializeField] private GameObject quizSelector;
    [SerializeField] private GameObject quizDisplay;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject categoryParent;
    [SerializeField] private GameObject quizParent;
    [SerializeField] private TMP_Text[] textAnswer1, textAnswer2, textAnswer3;
    [SerializeField] private TMP_Text progress, statement;
    [SerializeField] private Toggle[] answerToggles;
    private string[] categories;
    private Quiz[] quizzes;
    private int currentQuestion = 0;
    private int[] answers;
    private Question[] questions;
    private string currentCategory;

    private void Start() {
        Debug.Log("Attempting to get categories");
        StartCoroutine(TryGetCategories());
    }

    public void goToCategories() {
        populateCategories();
        quizSelector.SetActive(false);
        quizDisplay.SetActive(false);
        CategoryUI.SetActive(true);

    }

    public void goToQuizSelector(string category) {
        StartCoroutine(TryGetQuizzes(category));
        quizSelector.SetActive(true);
        quizDisplay.SetActive(false);
        CategoryUI.SetActive(false);
    }

    public void goToQuizDisplay(string quizId) {
        StartCoroutine(TryGetQuestions(quizId));
        quizSelector.SetActive(false);
        quizDisplay.SetActive(true);
        CategoryUI.SetActive(false);
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
            populateQuizzes();
            

        } else {

            Debug.Log("Request failed");
        }

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
            GameObject button = Instantiate(buttonPrefab, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
            button.GetComponent<ButtonManagerWithIcon>().buttonText = quiz.name;
            button.transform.SetParent(quizParent.transform, false);
            button.GetComponent<Button>().onClick.AddListener(delegate{goToQuizDisplay(quiz._id);});
        }

    }

    public void startQuiz() {
        answers = new int[questions.Length];
        currentQuestion = 0;

        populateQuestionText();
    }

    public void next() {
        if(currentQuestion + 1 == questions.Length) { // Quiz finished
            goToCategories();
        } else {

            if(answerToggles[0].isOn) {
                answers[currentQuestion] = 1;
            } else if(answerToggles[1].isOn) {
                answers[currentQuestion] = 2;
            } else {
                answers[currentQuestion] = 3;
            }

            currentQuestion++;
            populateQuestionText();
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
}
