using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Michsky.UI.ModernUIPack;

public class Register : MonoBehaviour
{
    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,32})";
    [SerializeField] private string registerEndpoint = "http://127.0.0.1:12345/account/create";
    [SerializeField] private string addAttributeEndpoint = "http://127.0.0.1:12345/account/addAttribute";

    [SerializeField] private TextMeshProUGUI alertText1;
    [SerializeField] private TextMeshProUGUI alertText2;
    [SerializeField] private TextMeshProUGUI alertText;

  
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField repeatPasswordInputField;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField surname1InputField;
    [SerializeField] private TMP_InputField surname2InputField;


    [SerializeField] private TMP_InputField birthDateInputFieldDay;
    [SerializeField] private TMP_InputField birthDateInputFieldMonth;
    [SerializeField] private TMP_InputField birthDateInputFieldYear;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField phoneInputField;
    [SerializeField] private HorizontalSelector genderSelector;
    [SerializeField] private HorizontalSelector companionSelector;


    [SerializeField] private TMP_InputField heightInputField;
    [SerializeField] private TMP_InputField weightInputField;
    [SerializeField] private TMP_Dropdown stateInputField;
    [SerializeField] private TMP_Dropdown attributesInputField;
  
  
    [SerializeField] private Toggle expertInputField;
    [SerializeField] private Toggle companionAccessInputField;
    [SerializeField] private Button attributeButton;

    

    public string sexVal = "Male";
    public bool isCompanion = false;

    private LoginRegisterMenuController controller;

    private List<string> totalAttributes = new List<string>();

    public void Awake() {
        controller = this.gameObject.GetComponent<LoginRegisterMenuController>();

        birthDateInputFieldDay.characterLimit = 2;
        birthDateInputFieldMonth.characterLimit = 2;
        birthDateInputFieldYear.characterLimit = 4;
        birthDateInputFieldDay.onEndEdit.AddListener(delegate{CheckDone(birthDateInputFieldDay, birthDateInputFieldMonth);});
        birthDateInputFieldDay.onEndEdit.AddListener(delegate{CheckDone(birthDateInputFieldMonth, birthDateInputFieldYear);});

        // TODO Para todos los botones en el orden finalmente escogido

    }
    void CheckDone(TMP_InputField sourceField, TMP_InputField targetField) {targetField.Select();}


    public void setVariable(string name){
        switch(name){

            case "Male":
                sexVal = name;
                break;
            
            case "Female":
                sexVal = name;
                break;
            
            case "isPacient":
                isCompanion = false;
                break;
            case "isCompanion":
                isCompanion = true;
                break;
        }
    }

    public void setSex(string value) {
        sexVal = value;
    }

    public void setIsCompanion(bool value) {
        isCompanion = value;
    }
    

    void deleteFromAttributes(string name){
        totalAttributes.Remove(name);
        Destroy(EventSystem.current.currentSelectedGameObject);
    }

    public void OnRegisterClick() {
        alertText.text = "Registering account...";
        controller.ActivateButtons(false);

        StartCoroutine(TryRegister());
    }

    public void RegisterFirstStep(){

    string username = usernameInputField.text;
    string password = passwordInputField.text;
    string name = nameInputField.text;
    string surname1 = surname1InputField.text;
    string surname2 = surname2InputField.text;

    alertText1.text = "";

    bool firstStepGood = true;

        if (username.Length < 3 || username.Length > 24) {
            alertText1.text += "Invalid username. ";
            controller.ActivateButtons(true);

            firstStepGood = false;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX)) {
            alertText1.text += "Password is not safe enough. ";
            controller.ActivateButtons(true);

            firstStepGood = false;
        }

        if(password != repeatPasswordInputField.text) {
            alertText1.text += "Passwords do not match. ";
            controller.ActivateButtons(true);

            firstStepGood = false;         
        }

        if (name.Length < 1) {
            alertText1.text += "Invalid name. ";
            controller.ActivateButtons(true);

            firstStepGood = false;          
        }
        
        if (surname1.Length < 1) {
            alertText1.text += "Invalid surname. ";
            controller.ActivateButtons(true);

            firstStepGood = false;           
        }

        if(firstStepGood){
            controller.GoToRegister2();
        }else{
            alertText1.enabled = true;
        }

    }
    public void RegisterSecondStep(){

        string birthDate = "";
        string birthDateDay = birthDateInputFieldDay.text;
        string birthDateMonth = birthDateInputFieldMonth.text;
        string birthDateYear = birthDateInputFieldYear.text;
        string email = emailInputField.text;
        string phone = phoneInputField.text;
        string sex;
        string companion;

        alertText2.text = "";
        bool secondStepGood = true;



        int birthValue = Int32.Parse(birthDateDay);
        if(birthValue>31 || birthValue<1){
             alertText2.text += "Invalid birth day. ";
             controller.ActivateButtons(true);

             secondStepGood = false;
         }
                

         birthValue = Int32.Parse(birthDateMonth);
         if(birthValue>12 || birthValue<1){
            alertText2.text += "Invalid birth month. ";
            controller.ActivateButtons(true);

            secondStepGood = false;
         }

        
        birthValue = Int32.Parse(birthDateYear);
        if (birthValue < 1920 || birthValue > DateTime.Now.Year) {
            alertText2.text += "Invalid birth year. ";
            controller.ActivateButtons(true);
            
            secondStepGood = false;
        }

        birthDate = birthDateYear + "-" + birthDateMonth + "-" +  birthDateDay;


        if (email.Length < 4) { // TODO Improve mail validation
            alertText2.text += "Invalid email. ";
            controller.ActivateButtons(true);
            
            secondStepGood = false;
        }

        if (phone.Length != 9) {
            alertText2.text += "Invalid phone. ";
            controller.ActivateButtons(true);
            
            secondStepGood = false;
        }

        
        switch(genderSelector.index){

            case 0:
            sex = "Male";
            break;
            
            case 1: 
            sex = "Female";
            break;
        }

        if(companionSelector.index == 0)  companion = "False";
        else companion = "True";


        if(secondStepGood){
            controller.GoToRegister3();
        }else{

        }
    }

    private IEnumerator TryRegister() {

        string username = usernameInputField.text;
        string password = passwordInputField.text;
        string name = nameInputField.text;
        string surname1 = surname1InputField.text;
        string surname2 = surname2InputField.text;

        string birthDate = "";
        string birthDateDay = birthDateInputFieldDay.text;
        string birthDateMonth = birthDateInputFieldMonth.text;
        string birthDateYear = birthDateInputFieldYear.text;

        string sex = sexVal;
        string height = heightInputField.text;
        string weight = weightInputField.text;
        string state = stateInputField.captionText.text;
        string attribute = "";
        string email = emailInputField.text;
        string phone = phoneInputField.text;
        string companion = isCompanion == true ? "True" : "False";
        string expert = expertInputField.isOn == true ? "True" : "False";
        string companionAccess = companionAccessInputField.isOn == true ? "True" : "False";

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

        if(password != repeatPasswordInputField.text) {
            alertText.text = "Passwords do not match.";
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


        int birthValue = Int32.Parse(birthDateDay);
        if(birthValue>31 || birthValue<1){
             alertText.text = "Invalid birth day.";
             controller.ActivateButtons(true);
             yield break;
         }
                

         birthValue = Int32.Parse(birthDateMonth);
         if(birthValue>12 || birthValue<1){
            alertText.text = "Invalid birth month.";
            controller.ActivateButtons(true);
            yield break;
         }

        
        birthValue = Int32.Parse(birthDateYear);
        if (birthValue < 1920 || birthValue > DateTime.Now.Year) {
            alertText.text = "Invalid birth year.";
            controller.ActivateButtons(true);
            yield break;
        }

        birthDate = birthDateYear + "-" + birthDateMonth + "-" +  birthDateDay;

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


        #endregion


        foreach (string atr in totalAttributes) attribute += atr + "|";

        if(attribute.Length != 0) {
        attribute = attribute.Remove(attribute.Length-1);
        } else attribute = "";


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
        form.AddField("rAttributeNames", attribute);
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

