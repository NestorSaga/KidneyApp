using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using Michsky.UI.ModernUIPack;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private string getCategoriesEndpoint = "http://127.0.0.1:12345/categories/names";
    [SerializeField] private string getVideosEndpoint = "http://127.0.0.1:12345/videos/getByCategory";
    [SerializeField] private GameObject categoryUI;
    [SerializeField] private GameObject videoSelector;
    [SerializeField] private GameObject videoPlayer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject categoryParent, videoParent;
    private string[] categories;
    private VideoResponse videos;

    private void Start() {
        Debug.Log("Attempting to get categories");
        StartCoroutine(TryGetCategories());
    }

    public void goToCategories() {
        foreach (Transform child in categoryParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        videoSelector.SetActive(false);
        videoPlayer.SetActive(false);
        categoryUI.SetActive(true);
        populateCategories();
    }

    public void goToVideoSelector(string category) {
        foreach (Transform child in videoParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        StartCoroutine(TryGetVideos(category));
        videoSelector.SetActive(true);
        videoPlayer.SetActive(false);
        categoryUI.SetActive(false);
    }

    public void goToVideoPlayer(string url) {
        PlayerPrefs.SetInt("videoCount", PlayerPrefs.GetInt("videoCount") + 1);
        videoSelector.SetActive(false);
        videoPlayer.SetActive(true);
        categoryUI.SetActive(false);
        playVideo(url);
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

    private IEnumerator TryGetVideos(string category)
    {

        WWWForm form = new WWWForm();
        form.AddField("rCategory", category);
        UnityWebRequest request = UnityWebRequest.Post(getVideosEndpoint, form);

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

            VideoResponse response = JsonUtility.FromJson<VideoResponse>(request.downloadHandler.text);

            if (response.code == 0)
            {

                videos = response;
                populateVideos();

            }
            else
            {
                Debug.Log("Request was not successful");
            }

        }
        else
        {

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

    public void populateVideos()
    {

        foreach (Video video in videos.videos)
        {
            GameObject button = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            button.GetComponent<ButtonManagerWithIcon>().buttonText = video.name;
            button.transform.SetParent(videoParent.transform, false);
            button.GetComponent<Button>().onClick.AddListener(delegate {goToVideoPlayer(video.url);});
        }

    }

    public void playVideo(string url)
    {
        videoPlayer.GetComponent<VideoPlayer>().url = url;
        videoPlayer.GetComponent<VideoPlayer>().Prepare();
    }

    public void goToHub() {
        GameManager.Instance.ChangeScene(2);
    }
}
