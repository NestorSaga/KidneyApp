using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class HubController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI welcomeText;



    private void Start(){

        welcomeText.text = "Welcome " + PlayerPrefs.GetString("username") + "!";


    }
}
