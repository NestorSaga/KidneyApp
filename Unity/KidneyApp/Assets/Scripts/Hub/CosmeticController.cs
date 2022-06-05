using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticController : MonoBehaviour
{
    [SerializeField] private Image hat, face, body;
    [SerializeField] private Sprite[] hats, faces, bodies;
    [SerializeField] private int[] currentCosmetics = new int[3] {0,0,0}; //Hat, face and body

    private void Awake() { // if json interno has, set currentcosmetics to json interno value, else [0,0,0]
        setCosmetics(currentCosmetics);
    }

    public void setHat(Sprite spr_hat) {
        hat.sprite = spr_hat;
    }

    public void setFace(Sprite spr_face) {
        face.sprite = spr_face;
    }

    public void setBody(Sprite spr_body) {
        body.sprite = spr_body;
    }

    public void toggleHat(bool positive) {
        if(positive && currentCosmetics[0] +1 < hats.Length) {
            currentCosmetics[0] += 1;
            hat.sprite = hats[currentCosmetics[0]];
        } else {
            currentCosmetics[0] = 0;
            hat.sprite = hats[currentCosmetics[0]];
        }
    }

    public void toggleFace(bool positive) {
        if(positive && currentCosmetics[1] +1 < faces.Length) {
            currentCosmetics[1] += 1;
            face.sprite = faces[currentCosmetics[1]];
        } else {
            currentCosmetics[1] = 0;
            face.sprite = faces[currentCosmetics[1]];
        }
    }

        public void toggleBody(bool positive) {
        if(positive && currentCosmetics[2] +1 < bodies.Length) {
            currentCosmetics[2] += 1;
            body.sprite = bodies[currentCosmetics[2]];
        } else {
            currentCosmetics[2] = 0;
            body.sprite = bodies[currentCosmetics[2]];
        }
    }

    public void setCosmetics(int[] cosmeticArray) {
        if(cosmeticArray[0] > hats.Length) {
            setHat(hats[cosmeticArray[0]]);
        }
        if(cosmeticArray[1] > faces.Length) {
            setFace(hats[cosmeticArray[1]]);
        }
        if(cosmeticArray[2] > bodies.Length) {
            setBody(hats[cosmeticArray[2]]);
        }
    }

    public void goToHub() {
        GameManager.Instance.ChangeScene(2);
    }
}
