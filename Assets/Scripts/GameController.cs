using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PowerUpController))]
public class GameController : MonoBehaviour {

    [SerializeField]
    private Image panelWin = null;
    [SerializeField]
    private Text textWin = null;

    private bool _isPaused = false;
    private PowerUpController _powerUpController = null;

    // Start is called before the first frame update
    void Start() {
        //panelWin.gameObject.SetActive(false);
        _powerUpController = GetComponent<PowerUpController>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetPause();
        }
    }

    public void SetPause() {
        _isPaused = !_isPaused;
        _powerUpController.SetPause(_isPaused);
    }

    public void Win(PlayerEnum playerEnum) {
        panelWin.gameObject.SetActive(true);
        if (playerEnum == PlayerEnum.PLAYER1) {
            textWin.text = "PLAYER 1 WIN";
        } else {
            textWin.text = "PLAYER 2 WIN";
        }
    }

    public void Menu() {
        SceneController.Menu();
    }

    public void Restart() {
        SceneController.Play();
    }

}