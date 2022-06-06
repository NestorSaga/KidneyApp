using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class Login : MonoBehaviour
{
    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,32})";
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:12345/account/login";
    [SerializeField] private string autologinEndpoint = "http://127.0.0.1:12345/account/autologin";
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    private LoginRegisterMenuController controller;

    public void Awake() {
        controller = this.gameObject.GetComponent<LoginRegisterMenuController>();
    }

    public void Start()
    {
        StartCoroutine(TryAutoLogin());
    }

    public void OnLoginClick() {
        controller.ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    private IEnumerator TryLogin() {

        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24) {
            controller.ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX)) {
            controller.ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                Debug.Log("Connection timeout at login");
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success) {

            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if(response.code == 0) {// login successful

                PlayerPrefs.SetString("userId", response.data._id);
                PlayerPrefs.SetString("username", response.data.username);
                PlayerPrefs.SetInt("loginCount", PlayerPrefs.GetInt("loginCount") + 1);
                PlayerPrefs.Save();

                GameManager.Instance.FillJSON();
                GameManager.Instance.ChangeScene(2); //goto hub

            } else {
                switch(response.code) {
                    case 1:
                        controller.ActivateButtons(true);
                        break;
                    default:
                        break;
                }
            }

        } else {
            controller.ActivateButtons(true);
        }
        
        yield return null;
    }
    
    private IEnumerator TryAutoLogin() {

        Debug.Log("Attempting autologin");
        if(GameManager.Instance.JSONExists()) 
        {

            JSONdata data = GameManager.Instance.LoadData();

            WWWForm form = new WWWForm();
            form.AddField("rUserId", data._id);
            form.AddField("rKey", data.apiKey);

            UnityWebRequest request = UnityWebRequest.Post(autologinEndpoint, form);

            var handler = request.SendWebRequest();

            float startTime= 0.0f;
            while (!handler.isDone){
                startTime += Time.deltaTime;

                if(startTime > 10.0f) {
                    break;
                }

                yield return null;
            }

            if(request.result == UnityWebRequest.Result.Success) {

                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

                if(response.code == 0) {// login successful

                    PlayerPrefs.SetString("userId", response.data._id);
                    PlayerPrefs.SetString("username", response.data.username);
                    PlayerPrefs.SetInt("loginCount", PlayerPrefs.GetInt("loginCount") + 1);
                    PlayerPrefs.Save();

                    GameManager.Instance.ChangeScene(2); //goto hub

                } else {
                    switch(response.code) {
                        case 1:
                            controller.ActivateButtons(true);
                            break;
                        default:
                            break;
                    }
                }

            } else {
                controller.ActivateButtons(true);
            }



            yield return null;
        }
        Debug.Log("Autologin failed");
    }

}