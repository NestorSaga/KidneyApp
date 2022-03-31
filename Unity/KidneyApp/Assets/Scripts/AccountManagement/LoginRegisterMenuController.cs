using UnityEngine;
using UnityEngine.UI;

public class LoginRegisterMenuController : MonoBehaviour
{
    [SerializeField] private Button[] interactableButtons;
    [SerializeField] private GameObject canvas1, canvas2, canvas3;

    public void ChangeScene(int id) {
        GameManager.Instance.ChangeScene(id);
    }

    public void ActivateButtons(bool state) {
        foreach(Button b in interactableButtons) {
            b.interactable = state;
        }
    }

    public void GoToRegister1() {
        canvas1.SetActive(true);
        canvas2.SetActive(false);
        canvas3.SetActive(false);
    } 

    public void GoToRegister2() {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        canvas3.SetActive(false);
    }

    public void GoToRegister3() {
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        canvas3.SetActive(true);
    }

}
