using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void ChangeScene(int id) {
        GameManager.Instance.ChangeScene(id);
    }
}
