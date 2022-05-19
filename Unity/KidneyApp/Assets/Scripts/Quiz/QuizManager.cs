using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private string getCategoriesEndpoint = "http://127.0.0.1:12345/categories/names";
    [SerializeField] private string getQuizzesEndpoint = "http://127.0.0.1:12345/quiz/getQuizzesFromCategoryName";
    [SerializeField] private GameObject CategoryUI;
    [SerializeField] private GameObject quizSelector;
    [SerializeField] private GameObject quizDisplay;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject categoryParent;
    [SerializeField] private GameObject quizParent;
    private string[] categories;
    private Quiz[] quizzes;

    private void Start() {
        Debug.Log("Attempting to get categories");
        StartCoroutine(TryGetCategories());
    }

    public void goToCategories() {
        quizSelector.SetActive(false);
        //quizDisplay.SetActive(false);
        CategoryUI.SetActive(true);
        populateCategories();
    }

    public void goToQuizSelector(string category) {
        quizSelector.SetActive(true);
        //quizDisplay.SetActive(false);
        CategoryUI.SetActive(false);
        StartCoroutine(TryGetQuizzes(category));
    }
    public void goToQuizDisplay(string url) {
        quizSelector.SetActive(false);
        //quizDisplay.SetActive(true);
        CategoryUI.SetActive(false);
        //playVideo(url);
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
            //button.GetComponent<Button>().onClick.AddListener(/* start quiz */);
        }

    }

    public void goToHub() {
        GameManager.Instance.ChangeScene(2);
    }
}
