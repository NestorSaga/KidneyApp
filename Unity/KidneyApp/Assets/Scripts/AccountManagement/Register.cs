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

    //References 
    
    [SerializeField] private string registerEndpoint = "http://127.0.0.1:12345/account/create";
    [SerializeField] private string addAttributeEndpoint = "http://127.0.0.1:12345/account/addAttribute";

    [SerializeField] private TextMeshProUGUI alertText1;
    [SerializeField] private TextMeshProUGUI alertText2;
    [SerializeField] private TextMeshProUGUI alertText3;
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
    [SerializeField] private HorizontalSelector stateSelector;
    private TMP_Dropdown attributesInputField;
    [SerializeField] private Toggle dailyExerciseInputField;
    [SerializeField] private Toggle expertInputField;
    [SerializeField] private Toggle companionAccessInputField;



    //Variables    

    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,32})";
    private LoginRegisterMenuController controller;
    public string sexVal = "Male";
    public bool isCompanion = false;


    //FirstStep
    string username;
    string password;
    string firstName;
    string surname1;
    string surname2;

    //SecondStep
    string birthDate;
    string birthDateDay;
    string birthDateMonth;
    string birthDateYear;
    string email;
    string phone;
    string sex;
    string companion;

    //ThirdStep
    string height;
    string weight;
    string attribute = "";
    string state;
    string dailyExercise;
    string expert;
    string companionAccess;

    string IMCValue;
    


    private List<string> totalAttributes = new List<string>();

    public void Awake() {
        controller = this.gameObject.GetComponent<LoginRegisterMenuController>();

        birthDateInputFieldDay.characterLimit = 2;
        birthDateInputFieldMonth.characterLimit = 2;
        birthDateInputFieldYear.characterLimit = 4;

        usernameInputField.onEndEdit.AddListener(delegate{CheckDone(usernameInputField, passwordInputField);});
        passwordInputField.onEndEdit.AddListener(delegate{CheckDone(passwordInputField, repeatPasswordInputField);});
        repeatPasswordInputField.onEndEdit.AddListener(delegate{CheckDone(repeatPasswordInputField, nameInputField);});
        nameInputField.onEndEdit.AddListener(delegate{CheckDone(nameInputField, surname1InputField);});
        surname1InputField.onEndEdit.AddListener(delegate{CheckDone(surname1InputField, surname2InputField);});
        

        birthDateInputFieldDay.onEndEdit.AddListener(delegate{CheckDone(birthDateInputFieldDay, birthDateInputFieldMonth);});
        birthDateInputFieldMonth.onEndEdit.AddListener(delegate{CheckDone(birthDateInputFieldMonth, birthDateInputFieldYear);});
        birthDateInputFieldYear.onEndEdit.AddListener(delegate{CheckDone(birthDateInputFieldYear, emailInputField);});
        emailInputField.onEndEdit.AddListener(delegate{CheckDone(emailInputField, phoneInputField);});

        heightInputField.onEndEdit.AddListener(delegate{CheckDone(heightInputField, weightInputField);});


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

    public void AddAttribute(string atrName){

        if(!totalAttributes.Contains(atrName)) totalAttributes.Add(atrName);
        else totalAttributes.Remove(atrName);

    }
    public void RegisterFirstStep(){

        username = usernameInputField.text;
        password = passwordInputField.text;
        firstName = nameInputField.text;
        surname1 = surname1InputField.text;
        surname2 = surname2InputField.text;

        alertText1.text = "";

        bool firstStepGood = true;

        if (username.Length < 3 || username.Length > 24) {
            alertText1.text += "Nombre de usuario no válido. ";
            controller.ActivateButtons(true);

            firstStepGood = false;

        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX)) {
            alertText1.text += "La contraseña no es suficientemente segura. ";
            controller.ActivateButtons(true);

            firstStepGood = false;
        }

        if(password != repeatPasswordInputField.text) {
            alertText1.text += "Las contraseñas no coinciden. ";
            controller.ActivateButtons(true);

            firstStepGood = false;         
        }

        if (firstName.Length < 1) {
            alertText1.text += "Nombre no válido. ";
            controller.ActivateButtons(true);

            firstStepGood = false;          
        }
        
        if (surname1.Length < 1) {
            alertText1.text += "Apellido no válido. ";
            controller.ActivateButtons(true);

            firstStepGood = false;           
        }

        if(firstStepGood){
            controller.GoToRegister2();
        }

    }
    public void RegisterSecondStep(){

        birthDate = "";
        birthDateDay = birthDateInputFieldDay.text;
        birthDateMonth = birthDateInputFieldMonth.text;
        birthDateYear = birthDateInputFieldYear.text;
        email = emailInputField.text;
        phone = phoneInputField.text;

        alertText2.text = "";
        bool secondStepGood = true;



        int birthValue = Int32.Parse(birthDateDay);
        
        if(birthValue>31 || birthValue<1){
             alertText2.text += "Día no válido. ";
             controller.ActivateButtons(true);

             secondStepGood = false;
         }
                

         birthValue = Int32.Parse(birthDateMonth);
         if(birthValue>12 || birthValue<1){
            alertText2.text += "Mes no válido. ";
            controller.ActivateButtons(true);

            secondStepGood = false;
         }

        
        birthValue = Int32.Parse(birthDateYear);
        if (birthValue < 1920 || birthValue > DateTime.Now.Year) {
            alertText2.text += "Año no válido. ";
            controller.ActivateButtons(true);
            
            secondStepGood = false;
        }

        birthDate = birthDateYear + "-" + birthDateMonth + "-" +  birthDateDay;


        if (email.Length < 4) { // TODO Improve mail validation
            alertText2.text += "Email no válido. ";
            controller.ActivateButtons(true);
            
            secondStepGood = false;
        }

        if (phone.Length != 9) {
            alertText2.text += "Teléfono no válido. ";
            controller.ActivateButtons(true);
            
            secondStepGood = false;
        }

        sex = genderSelector.itemList[genderSelector.index].itemTitle;

        if(companionSelector.index == 0)  companion = "False";
        else companion = "True";

        if(secondStepGood){
            controller.GoToRegister3();
        }
    }

    public void RegisterThirdStep(){

        height = heightInputField.text;
        weight = weightInputField.text;
        state = stateSelector.itemList[stateSelector.index].itemTitle;

        dailyExercise = dailyExerciseInputField.isOn == true ? "True" : "False";

        expert = expertInputField.isOn == true ? "True" : "False";
        companionAccess = companionAccessInputField.isOn == true ? "True" : "False";


        bool thirdStepGood = true;


        if (height.Length < 2) {
            alertText3.text += "Altura no válida. ";
            controller.ActivateButtons(true);
            
            thirdStepGood = false;
        }

        if (weight.Length < 2) {
            alertText3.text += "Peso no válido. ";
            controller.ActivateButtons(true);
            
            thirdStepGood = false;
        }

        if (state == "Enter your state") {
            alertText3.text += "Atributos no válidos. ";
            controller.ActivateButtons(true);
            
            thirdStepGood = false;
        }


        foreach (string atr in totalAttributes) attribute += atr + "|";

        if(attribute.Length != 0) {
        attribute = attribute.Remove(attribute.Length-1);
        } else attribute = "";



        if(thirdStepGood){
            
            alertText3.text = "Registrando cuenta...";
            controller.ActivateButtons(false);

            CalculateIMC();

            StartCoroutine(TryRegister());
        }

    }


    public void CalculateIMC(){

        // TODO

        int intWeight = Int32.Parse(weight);
        var intHeight = (Int32.Parse(height));
        var intHeight2 = (float)intHeight/100;
        float IMCAnswer = (intWeight/(Mathf.Pow(intHeight2,2)));

        int weightState;

        if(IMCAnswer < 18.5f) weightState = 0; // Not enough weight
        else if(IMCAnswer >= 18.5f && IMCAnswer <= 24.9f) weightState = 1; // Normal weight
        else if(IMCAnswer >= 25) weightState = 2; // Overweight
        else weightState = 1;

        Debug.Log("El estado es " + weightState + "debido a que el calculo es " + IMCAnswer + "Mi altura es " + intHeight2 + "y mi peso es " + intWeight);

        //Calculate values

        if(weightState==1 && dailyExercise == "True" && state=="Pre-diálisis" && !attribute.Contains("Diabetes")) IMCValue = "1";
        else if(weightState==1 && dailyExercise == "True" && state=="Pre-diálisis" && attribute.Contains("Diabetes")) IMCValue = "2";
        else if(weightState==1 && dailyExercise == "True" && (state=="Diálisis per." || state=="Hemodiálisis") && !attribute.Contains("Diabetes")) IMCValue = "3";
        else if(weightState==1 && dailyExercise == "True" && (state=="Diálisis per." || state=="Hemodiálisis") && attribute.Contains("Diabetes")) IMCValue = "4";
        else if(weightState==1 && dailyExercise == "False" && state=="Pre-diálisis" && !attribute.Contains("Diabetes")) IMCValue = "5";
        else if(weightState==1 && dailyExercise == "False" && state=="Pre-diálisis" && attribute.Contains("Diabetes")) IMCValue = "6";
        else if(weightState==1 && dailyExercise == "False" && (state=="Diálisis per." || state=="Hemodiálisis") && !attribute.Contains("Diabetes")) IMCValue = "7";
        else if(weightState==1 && dailyExercise == "False" && (state=="Diálisis per." || state=="Hemodiálisis") && attribute.Contains("Diabetes")) IMCValue = "8";
        //
        else if(weightState==0 && dailyExercise == "True" && state=="Pre-diálisis" && !attribute.Contains("Diabetes")) IMCValue = "9";
        else if(weightState==0 && dailyExercise == "True" && state=="Pre-diálisis" && attribute.Contains("Diabetes")) IMCValue = "10";
        else if(weightState==0 && dailyExercise == "True" && (state=="Diálisis per." || state=="Hemodiálisis") && !attribute.Contains("Diabetes")) IMCValue = "11";
        else if(weightState==0 && dailyExercise == "True" && (state=="Diálisis per." || state=="Hemodiálisis") && attribute.Contains("Diabetes")) IMCValue = "12";
        else if(weightState==0 && dailyExercise == "False" && state=="Pre-diálisis" && !attribute.Contains("Diabetes")) IMCValue = "13";
        else if(weightState==0 && dailyExercise == "False" && state=="Pre-diálisis" && attribute.Contains("Diabetes")) IMCValue = "14";
        else if(weightState==0 && dailyExercise == "False" && (state=="Diálisis per." || state=="Hemodiálisis") && !attribute.Contains("Diabetes")) IMCValue = "15";
        else if(weightState==0 && dailyExercise == "False" && (state=="Diálisis per." || state=="Hemodiálisis") && attribute.Contains("Diabetes")) IMCValue = "16";
        //
        else if(weightState==2 && dailyExercise == "True" && state=="Pre-diálisis" && !attribute.Contains("Diabetes")) IMCValue = "17";
        else if(weightState==2 && dailyExercise == "True" && state=="Pre-diálisis" && attribute.Contains("Diabetes")) IMCValue = "18";
        else if(weightState==2 && dailyExercise == "True" && (state=="Diálisis per." || state=="Hemodiálisis") && !attribute.Contains("Diabetes")) IMCValue = "19";
        else if(weightState==2 && dailyExercise == "True" && (state=="Diálisis per." || state=="Hemodiálisis") && attribute.Contains("Diabetes")) IMCValue = "20";
        else if(weightState==2 && dailyExercise == "False" && state=="Pre-diálisis" && !attribute.Contains("Diabetes")) IMCValue = "21";
        else if(weightState==2 && dailyExercise == "False" && state=="Pre-diálisis" && attribute.Contains("Diabetes")) IMCValue = "22";
        else if(weightState==2 && dailyExercise == "False" && (state=="Diálisis per." || state=="Hemodiálisis") && !attribute.Contains("Diabetes")) IMCValue = "23";
        else if(weightState==2 && dailyExercise == "False" && (state=="Diálisis per." || state=="Hemodiálisis") && attribute.Contains("Diabetes")) IMCValue = "24";


    }

    private IEnumerator TryRegister() {

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);
        form.AddField("rName", firstName);
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
        form.AddField("rDailyExercise", dailyExercise);
        form.AddField("rExpert", expert);
        form.AddField("rCompanionAccess", companionAccess);
        form.AddField("rIMCValue", IMCValue);
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

                PlayerPrefs.SetString("userId", response.data._id);
                PlayerPrefs.SetString("username", response.data.username);
                PlayerPrefs.SetInt("IMCValue", response.data.IMCValue);
                PlayerPrefs.SetInt("loginCount", 0);
                PlayerPrefs.SetInt("videoCount", 0);
                PlayerPrefs.SetInt("menuCount", 0);
                PlayerPrefs.SetInt("quizCount", 0);
                PlayerPrefs.SetInt("hat", 0);
                PlayerPrefs.SetInt("face", 0);
                PlayerPrefs.SetInt("body", 0);
                PlayerPrefs.Save();

                alertText1.text = "Cuenta creada.";

                GameManager.Instance.FillJSON();
                GameManager.Instance.ChangeScene(2); //goto hub

            } else {
                switch(response.code) {
                    case 1:
                        alertText1.text = "Credenciales inválidos.";
                        break;
                    case 2:
                        alertText1.text = "Nombre de usuario ya en uso.";
                        break;
                    case 3:
                        alertText1.text = "La contraseña no es lo suficientemente segura.";
                        break;
                    default:
                        alertText1.text = "Código de respuesta inválido.";
                        break;
                }
            }

        } else {

            alertText1.text = "Error al conectar con el servidor...";
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

