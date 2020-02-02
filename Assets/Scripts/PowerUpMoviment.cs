using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpEnum { 
    DOUBLE_PUSH, FAST, SLOW
}

public class PowerUpMoviment : MonoBehaviour {

    [SerializeField]
    private float speed = 6f;

    [SerializeField]
    private PowerUpEnum powerUpEnum = PowerUpEnum.DOUBLE_PUSH;

    [SerializeField]
    private PlayerEnum playerEnum = PlayerEnum.PLAYER1;

    private float _realSpeed = 0f;
    private PowerUpController _powerUpController;
    private Transform _destroyPosition;

    private bool _isPaused = false;

    void Awake() {
        _realSpeed = SortSpeed();
        _powerUpController = GameObject.FindGameObjectWithTag("GameController").GetComponent<PowerUpController>();
        _destroyPosition = GameObject.FindGameObjectWithTag("DestroyObstaclePosition").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        if (_isPaused) return;

        this.transform.position -= Vector3.right * _realSpeed * Time.deltaTime;

        if (transform.position.x <= _destroyPosition.position.x) {
            DestroyPowerUp();
        }
    }

    private float SortSpeed() {
        return Random.Range(speed - 0.2f, speed + 0.2f);
    }

    public void SetPause(bool isPaused) {
        _isPaused = isPaused;
    }

    private void DestroyPowerUp() {
        _powerUpController.RemovePowerUp(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            PlayerMoviment playerScript = collider.GetComponent<PlayerMoviment>();
            if (playerEnum == playerScript.GetPlayerEnum()) {
                playerScript.GetPowerUp(powerUpEnum);
                DestroyPowerUp();
            }
        }
    }

}