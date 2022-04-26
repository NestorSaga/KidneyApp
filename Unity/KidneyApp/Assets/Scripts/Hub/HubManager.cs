using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class HubManager : MonoBehaviour
{

    public void FillJSON() {
        GameManager.Instance.FillJSON();
    }
}
