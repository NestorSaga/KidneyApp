using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    
    [SerializeField] private string authenticationEndopoint = "http://127.0.0.1:12345/account";

    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button loginButton;
    

    public void OnLoginClick() {
        alertText.text = "Signing in...";
        loginButton.interactable = false;

        StartCoroutine(TryLogin());
    }

    private IEnumerator TryLogin() {


        string username = usernameInputField.text;
        string password = passwordInputField.text;

        UnityWebRequest request = UnityWebRequest.Get($"{authenticationEndopoint}?rUsername={username}&rPassword={password}");

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

            if(request.downloadHandler.text != "Invalid Credentials") {// login successful? 
                alertText.text = "Welcome!";
                loginButton.interactable = true;

            } else {
                alertText.text = "insvaslid credens";
                loginButton.interactable = true;
            }


        } else {
            alertText.text = "Error connecting to the server...";
            loginButton.interactable = true;
        }

        yield return null;
    }
}

