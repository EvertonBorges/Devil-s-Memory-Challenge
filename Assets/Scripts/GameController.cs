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
    [SerializeField]
    private GroundMoviment ground = null;
    [SerializeField]
    private GroundMoviment background = null;
    [SerializeField]
    private AudioSource backgroundMusic = null;

    private bool _isPaused = false;    
    private PowerUpController _powerUpController = null;
    private MemoryController _memoryController = null;
    private List<PlayerMoviment> _players = new List<PlayerMoviment>();

    // Start is called before the first frame update
    void Start() {
        panelWin.gameObject.SetActive(false);
        _powerUpController = GetComponent<PowerUpController>();
        _memoryController = GetComponent<MemoryController>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
            _players.Add(obj.GetComponent<PlayerMoviment>());
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetPause();
        }
    }

    public void SetPause() {
        _isPaused = !_isPaused;
        ground.SetPause(_isPaused);
        background.SetPause(_isPaused);
        backgroundMusic.Stop();
        _players.ForEach(delegate (PlayerMoviment script) {
            script.SetPause(_isPaused);
        });
        _powerUpController.SetPause(_isPaused);
        _memoryController.SetPause(_isPaused);
    }

    public void Win(PlayerEnum playerEnum) {
        SetPause();
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