using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour
{
    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,24})";
    [SerializeField] private string registerEndpoint = "http://127.0.0.1:12345/account/create";

    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    private LoginRegisterMenuController controller;

    public void Awake() {
        controller = this.gameObject.GetComponent<LoginRegisterMenuController>();
    }
    
    public void OnRegisterClick() {
        alertText.text = "Registering account...";
        controller.ActivateButtons(false);

        StartCoroutine(TryRegister());
    }

    private IEnumerator TryRegister() {

        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24) {
            alertText.text = "Invalid username.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX)) {
            alertText.text = "Password is not safe enough.";
            controller.ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(registerEndpoint, form);

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

            CreateResponse response = JsonUtility.FromJson<CreateResponse>(request.downloadHandler.text);

            if(response.code == 0) {

                alertText.text = "Account has been created.";

                GameManager.Instance.ChangeScene(2); //goto hub

            } else {
                switch(response.code) {
                    case 1:
                        alertText.text = "Invalid credentials.";
                        break;
                    case 2:
                        alertText.text = "Username already taken.";
                        break;
                    case 3:
                        alertText.text = "Password is not safe enough.";
                        break;
                    default:
                        alertText.text = "Invalid response code.";
                        break;
                }
            }

        } else {

            alertText.text = "Error connecting to the server...";
        }

            controller.ActivateButtons(true);

        yield return null;
    }
}

