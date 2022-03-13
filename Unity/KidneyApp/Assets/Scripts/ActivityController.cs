using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityController : MonoBehaviour
{

    public void AddAchievment(string name){

        GameManager.Instance.AddAchievment(name);
    }

}
