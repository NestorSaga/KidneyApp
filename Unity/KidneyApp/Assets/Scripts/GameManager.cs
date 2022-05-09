using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string addAchievmentEndpoint = "http://127.0.0.1:12345/account/addAchievment";
    [SerializeField] private string dataEndpoint = "http://127.0.0.1:12345/account/retrieveData";
    [SerializeField] private string keyEndpoint = "http://127.0.0.1:12345/account/getkey";
    [SerializeField] public string JSONfile;

    private string path = "";
    private string persistentPath = "";

    private static GameManager instance = null;
    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        path = Application.dataPath + Path.AltDirectorySeparatorChar + "Data/data.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Data/data.json";
    }

    public void ChangeScene(int targetSceneId) {
        SceneManager.LoadScene(targetSceneId);
    } 


    public void AddAchievment(string name){

        StartCoroutine(tryAddAchievment(name));
    }

    private IEnumerator tryAddAchievment(string name){

        string userId = PlayerPrefs.GetString("userid");

        WWWForm form = new WWWForm();
        form.AddField("rUserId", userId);
        form.AddField("rAchievmentName", name);
        form.AddField("rCompletion", 2);
    
        UnityWebRequest request = UnityWebRequest.Post(addAchievmentEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                break;
            }

            yield return null;
        }     

       
        yield return null;
    }

    public void FillJSON()
    {
        StartCoroutine(LoadToJSON());
    }
    
    public IEnumerator LoadToJSON()
    {
        WWWForm form = new WWWForm();

        form.AddField("rUserId", PlayerPrefs.GetString("userId"));
        form.AddField("rUserName", PlayerPrefs.GetString("username"));

        UnityWebRequest request = UnityWebRequest.Post(dataEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }


        if (request.result == UnityWebRequest.Result.Success)
        {

            JSONdata response = JsonUtility.FromJson<JSONdata>(request.downloadHandler.text);

            string key = "";

            #region generating api key

                WWWForm keyform = new WWWForm();
                keyform.AddField("rUserId", response._id);

                UnityWebRequest keyrequest = UnityWebRequest.Post(keyEndpoint, keyform);

                var keyhandler = keyrequest.SendWebRequest();

                float start= 0.0f;
                while (!keyhandler.isDone){
                    start += Time.deltaTime;

                    if(start > 10.0f) {
                        Debug.Log("Connection timeout at key generation");
                        break;
                    }

                    yield return null;
                }

                if(keyrequest.result == UnityWebRequest.Result.Success) {

                    LoginResponse keyresponse = JsonUtility.FromJson<LoginResponse>(keyrequest.downloadHandler.text);

                    if(keyresponse.code == 0) {

                        key = keyresponse.data._id;

                    } else {
                        key = null;
                    }

                } else {
                    key = null;
                }

            #endregion

            response.apiKey = key;

            string jsonString = JsonUtility.ToJson(response);

            SaveData(jsonString);

            Debug.Log(jsonString);

        }
        else
        {
            Debug.Log("Error generating JSON data");
        }

        yield return null;
    }

    public void SaveData(string data)
    {
        FileInfo fi = new FileInfo(persistentPath);
        if(!fi.Directory.Exists) System.IO.Directory.CreateDirectory(fi.DirectoryName);

        using StreamWriter writer = new StreamWriter(persistentPath);
        
        writer.Write(data);
    }

    public bool JSONExists()
    {
        FileInfo fi = new FileInfo(persistentPath);
        return fi.Directory.Exists;
    }

    public JSONdata LoadData()
    {
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();

        return JsonUtility.FromJson<JSONdata>(json);
    }

    public void ClearJSON()
    {
        Debug.Log("Clearing JSON data");
        FileInfo fi = new FileInfo(persistentPath);
        fi.Directory.Delete(true);
    }

    public IEnumerator GenerateApiKey(string userId) {



        yield return null;

    }

    public void SelecFromIMC(AlimentData data, int value, string alimentFamily){

    }

}
