using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,24})";
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:12345/account/login";
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    private LoginRegisterMenuController controller;

    public void Awake() {
        controller = this.gameObject.GetComponent<LoginRegisterMenuController>();
    }

    public void OnLoginClick() {
        alertText.text = "Signing in...";
        controller.ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    private IEnumerator TryLogin() {

        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24) {
            alertText.text = "Invalid username.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX)) {
            alertText.text = "Invalid password.";
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
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success) {

            Debug.Log(request.downloadHandler.text);

            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if(response.code == 0) {// login successful? 

                alertText.text = "Welcome!";

                GameManager.Instance.ChangeScene(2); //goto hub

            } else {
                switch(response.code) {
                    case 1:
                        alertText.text = "Invalid credentials";
                        controller.ActivateButtons(true);
                        break;
                    default:
                        alertText.text = "Invalid response code";
                        break;
                }
            }

        } else {
            alertText.text = "Error connecting to the server...";
            controller.ActivateButtons(true);

        }
        
        yield return null;
    }
}

