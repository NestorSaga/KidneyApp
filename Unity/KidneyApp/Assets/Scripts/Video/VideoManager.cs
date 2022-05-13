using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private string getCategoriesEndpoint = "http://127.0.0.1:12345/categories/names";
    [SerializeField] private GameObject CategoryUI;
    [SerializeField] private GameObject VideoSelector;
    [SerializeField] private GameObject VideoPlayer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject categoryParent;
    private string currentVideoURL = "file://C:/Users/iguan/Documents/TFG/KidneyApp/Unity/KidneyApp/Assets/Sprites/Video/Baalakut Teaser.mp4";
    private string[] categories;

    private void Start() {
        Debug.Log("Attempting to get categories");
        StartCoroutine(TryGetCategories());
    }

    public void goToCategories() {
        foreach (Transform child in categoryParent.transform) {
            GameObject.Destroy(child.gameObject);
        }
        VideoSelector.SetActive(false);
        VideoPlayer.SetActive(false);
        CategoryUI.SetActive(true);
        populateCategories();
    }

    public void goToVideoSelector(string category) {
        Debug.Log("Current category: " + category);
        VideoSelector.SetActive(true);
        VideoPlayer.SetActive(false);
        CategoryUI.SetActive(false);
        //populateVideos(category); //implement when videos are provided
    }
    public void goToVideoPlayer() {
        VideoSelector.SetActive(false);
        VideoPlayer.SetActive(true);
        CategoryUI.SetActive(false);
        //playVideo();
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

    public void populateCategories() {

        foreach (string cat in categories)
        {
            GameObject button = Instantiate(buttonPrefab, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
            button.GetComponent<ButtonManagerWithIcon>().buttonText = cat;
            button.transform.SetParent(categoryParent.transform, false);
            button.GetComponent<Button>().onClick.AddListener(delegate{goToVideoSelector(cat);});
        }

    }
}
