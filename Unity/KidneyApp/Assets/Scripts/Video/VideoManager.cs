using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
        videoSelector.SetActive(false);
        videoPlayer.SetActive(false);
        categoryUI.SetActive(true);
        populateCategories();
    }

    public void goToVideoSelector(string category) {
        StartCoroutine(TryGetVideos(category));
        videoSelector.SetActive(true);
        videoPlayer.SetActive(false);
        categoryUI.SetActive(false);
    }

    public void goToVideoPlayer(string url) {
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
                Debug.Log("Video: ");
                Debug.Log(videos.videos[0].name);
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

        foreach (Transform child in categoryParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

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

        foreach (Transform child in videoParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Video video in videos.videos)
        {
            GameObject button = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            button.GetComponent<ButtonManagerWithIcon>().buttonText = video.displayName.es;
            button.transform.SetParent(videoParent.transform, false);
            button.GetComponent<Button>().onClick.AddListener(delegate {goToVideoPlayer(video.url);});
        }

    }

    public void playVideo(string url)
    {

        //play video url. Implement when video urls are provided. 
    }

    public void goToHub() {
        GameManager.Instance.ChangeScene(2);
    }
}
