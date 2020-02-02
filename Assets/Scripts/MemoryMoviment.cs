using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMoviment : MonoBehaviour {
    
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private PlayerEnum playerEnum = PlayerEnum.PLAYER1;

    private float _realSpeed = 0f;
    private MemoryController _memoryController;
    private Transform _destroyPosition;

    private bool _isPaused = false;

    void Awake() {
        _realSpeed = SortSpeed();
        _memoryController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MemoryController>();
        _destroyPosition = GameObject.FindGameObjectWithTag("DestroyObstaclePosition").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        if (_isPaused) return;

        this.transform.position -= Vector3.right * _realSpeed * Time.deltaTime;

        if (transform.position.x <= _destroyPosition.position.x) {
            DestroyMemory();
        }
    }

    private float SortSpeed() {
        return Random.Range(speed - 0.2f, speed + 0.2f);
    }

    public void SetPause(bool isPaused) {
        _isPaused = isPaused;
    }

    private void DestroyMemory() {
        _memoryController.RemoveMemory(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            PlayerMoviment playerScript = collider.GetComponent<PlayerMoviment>();
            if (playerEnum == playerScript.GetPlayerEnum()) {
                playerScript.GetMemory();
                DestroyMemory();
            }
        }
    }

}