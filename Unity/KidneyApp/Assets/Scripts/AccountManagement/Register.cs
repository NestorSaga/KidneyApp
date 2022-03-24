using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour
{
    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,32})";
    [SerializeField] private string registerEndpoint = "http://127.0.0.1:12345/account/create";
    [SerializeField] private string addAttributeEndpoint = "http://127.0.0.1:12345/account/addAttribute";
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField surname1InputField;
    [SerializeField] private TMP_InputField surname2InputField;
    [SerializeField] private TMP_InputField birthDateInputField;
    [SerializeField] private TMP_Dropdown sexInputField;
    [SerializeField] private TMP_InputField heightInputField;
    [SerializeField] private TMP_InputField weightInputField;
    [SerializeField] private TMP_Dropdown stateInputField;
    [SerializeField] private TMP_Dropdown attributesInputField;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField phoneInputField;
    [SerializeField] private TMP_Dropdown companionInputField;
    [SerializeField] private TMP_Dropdown expertInputField;
    [SerializeField] private TMP_Dropdown companionAccessInputField;

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
        string name = nameInputField.text;
        string surname1 = surname1InputField.text;
        string surname2 = surname2InputField.text;
        string birthDate = birthDateInputField.text;
        string sex = sexInputField.captionText.text;
        string height = heightInputField.text;
        string weight = weightInputField.text;
        string state = stateInputField.captionText.text;
        string attribute = attributesInputField.captionText.text;
        string email = emailInputField.text;
        string phone = phoneInputField.text;
        string companion = companionInputField.captionText.text;
        string expert = expertInputField.captionText.text;
        string companionAccess = companionAccessInputField.captionText.text;

        #region dataValidation

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

        if (name.Length < 1) {
            alertText.text = "Invalid name.";
            controller.ActivateButtons(true);
            yield break;
        }
        
        if (surname1.Length < 1) {
            alertText.text = "Invalid surname.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (birthDate.Length < 2) { // TODO hay que comprobarlo mejor
            alertText.text = "Invalid birth date.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (sex == "Enter your sex") {
            alertText.text = "Invalid sex selected.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (height.Length < 2) {
            alertText.text = "Invalid height.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (weight.Length < 2) {
            alertText.text = "Invalid weight.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (state == "Enter your state") {
            alertText.text = "Invalid state selected.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (attribute == "Enter your attributes") { // TODO maybe add option for multiple attributes
            alertText.text = "Invalid attribute selected.";
            controller.ActivateButtons(true);
            yield break;
        }

        if(attribute == "None") attribute = "";

        if (email.Length < 4) { // TODO Improve mail validation
            alertText.text = "Invalid email.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (phone.Length != 9) {
            alertText.text = "Invalid phone.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (companion == "Are you a patient?") {
            alertText.text = "Patient or companion not answered.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (expert == "Are you an expert?") {
            alertText.text = "Expert question not answered.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (companionAccess == "Allow data sharing?") {
            alertText.text = "Sharing data with companions not set.";
            controller.ActivateButtons(true);
            yield break;
        }

        #endregion

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);
        form.AddField("rName", name);
        form.AddField("rSurname1", surname1);
        form.AddField("rSurname2", surname2);
        form.AddField("rBirthDate", birthDate);
        form.AddField("rSex", sex);
        form.AddField("rHeight", height);
        form.AddField("rWeight", weight);
        form.AddField("rState", state);
        form.AddField("rAttributeName", attribute);
        form.AddField("rEmail", email);
        form.AddField("rPhone", phone);
        form.AddField("rCompanion", companion);
        form.AddField("rExpert", expert);
        form.AddField("rCompanionAccess", companionAccess);

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

            PlayerPrefs.SetString("userId", response.data._id);
            PlayerPrefs.SetString("username", response.data.username);
            PlayerPrefs.Save();

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
    
    private IEnumerator TryAddAttribute() {
        
        string attributeName = attributesInputField.captionText.text;
        string userId = PlayerPrefs.GetString("userId");

        if (attributeName == null) {
            alertText.text = "Null name.";
            controller.ActivateButtons(true);
            yield break;
        }

        if (attributeName == "None") {
             alertText.text = "No attribute";
             controller.ActivateButtons(true);
             yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rName", attributeName);
        form.AddField("rUserId", userId);

        UnityWebRequest request = UnityWebRequest.Post(addAttributeEndpoint, form);

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

                alertText.text = "Attribute added.";

                GameManager.Instance.ChangeScene(2); //goto hub

            } else {
                alertText.text = response.code.ToString();
            }

        } else {

            alertText.text = "Error with attributes...";
        }

            controller.ActivateButtons(true);

        yield return null;
    }
}

