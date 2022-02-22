using UnityEngine;
using UnityEngine.UI;

public class LoginRegisterMenuController : MonoBehaviour
{
    [SerializeField] private Button[] interactableButtons;

    public void ChangeScene(int id) {
        GameManager.Instance.ChangeScene(id);
    }

    public void ActivateButtons(bool state) {
        foreach(Button b in interactableButtons) {
            b.interactable = state;
        }
    } 

}
