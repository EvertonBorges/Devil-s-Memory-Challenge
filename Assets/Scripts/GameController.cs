using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PowerUpController))]
public class GameController : MonoBehaviour {

    [SerializeField]
    private Image panelWin;
    [SerializeField]
    private Text textWin;

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
        Debug.Log("GameController: Player " + (playerEnum == PlayerEnum.PLAYER1 ? 1 : 2) + " WIN");
        panelWin.gameObject.SetActive(true);
        if (playerEnum == PlayerEnum.PLAYER1) {
            textWin.text = "Player 1 WIN";
        } else {
            textWin.text = "Player 2 WIN";
        }
    }

    public void Menu() {
        SceneController.Menu();
    }

    public void Restart() {
        SceneController.Play();
    }

}