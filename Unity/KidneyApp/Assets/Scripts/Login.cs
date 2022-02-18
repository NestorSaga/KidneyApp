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

        if (username.Length < 3 || username.Length > 24) {
            alertText.text = "Invalid username.";
            loginButton.interactable = true;
            yield break;
        }

        if (password.Length < 3 || password.Length > 24) {
            alertText.text = "Invalid password.";
            loginButton.interactable = true;
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(authenticationEndopoint, form);

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

            if(request.downloadHandler.text != "Invalid credentials") {// login successful? 
                loginButton.interactable = true;
                UserAccount user = JsonUtility.FromJson<UserAccount>(request.downloadHandler.text); 
                alertText.text = $"{user._id} Welcome {user.username}!";

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

