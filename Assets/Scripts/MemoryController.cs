using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryController : MonoBehaviour {

    [SerializeField]
    private GameObject[] memoryPrefabs = null;

    [SerializeField]
    private Transform spawnMemory = null;

    [SerializeField]
    private float timeToSpawnMemory = 0f;

    private bool _isPaused = false;
    private float _currentTimeToSpawnMemory = 0f;
    private List<GameObject> _memories = new List<GameObject>();

    // Start is called before the first frame update
    void Awake() {
        _currentTimeToSpawnMemory = timeToSpawnMemory;
    }

    // Update is called once per frame
    void Update() {
        if (_isPaused) return;

        _currentTimeToSpawnMemory -= Time.deltaTime;
        if (_currentTimeToSpawnMemory < 0f) {
            int sortedPowerUpIndex = Random.Range(0, memoryPrefabs.Length);
            float sortedZPosition = SortedZPosition();
            Vector3 spawnPosition = new Vector3(spawnMemory.position.x, spawnMemory.position.y, sortedZPosition);
            GameObject powerUp = Instantiate(memoryPrefabs[sortedPowerUpIndex], spawnPosition, Quaternion.identity, spawnMemory);
            _memories.Add(powerUp);
            _currentTimeToSpawnMemory = timeToSpawnMemory;
        }
    }

    private float SortedZPosition() {
        int sortedNumber = Random.Range(0, 100);
        if (sortedNumber < 25) {
            return -1.875f;
        } else if (sortedNumber < 50) {
            return -0.625f;
        } else if (sortedNumber < 75) {
            return 0.625f;
        }
        return 1.875f;
    }

    public void RemoveMemory(GameObject memory) {
        _memories.Remove(memory);
    }

    public void SetPause(bool isPaused) {
        _isPaused = isPaused;
        _memories.ForEach(delegate (GameObject powerUp) {
            PowerUpMoviment powerUpScript = powerUp.GetComponent<PowerUpMoviment>();
            powerUpScript.SetPause(_isPaused);
        });
    }

}