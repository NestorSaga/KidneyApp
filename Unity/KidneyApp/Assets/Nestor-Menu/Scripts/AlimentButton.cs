using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlimentButton : MonoBehaviour
{


    public FoodData identity;


    public void Interact(){


        if(MenuController.Instance.currenState == MenuController.CurrenState.CreatingMenu){


            /*foreach(FoodData data in MenuController.Instance.newMenu.aliments){

                Debug.Log(data.name);

                    if(data == identity){
                        MenuController.Instance.removeAliment(MenuController.Instance.newMenu, identity);
                        Debug.Log("Encontrado");
                    } 
                    
            }*/
            MenuController.Instance.OnAlimentClick(identity);

        } 


        

        //else display info
    }




}
