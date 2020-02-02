using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

    [SerializeField]
    private GameObject[] powerUpPrefabs = null;

    [SerializeField]
    private Transform spawnPowerUps = null;

    [SerializeField]
    private float timeToSpawnPowerUps = 0f;

    private bool _isPaused = false;
    private float _currentTimeToSpawnPowerUps = 0f;
    private List<GameObject> _powerUps = new List<GameObject>();

    // Start is called before the first frame update
    void Awake() {
        _currentTimeToSpawnPowerUps = timeToSpawnPowerUps;
    }

    // Update is called once per frame
    void Update() {
        if (_isPaused) return;

        _currentTimeToSpawnPowerUps -= Time.deltaTime;
        if (_currentTimeToSpawnPowerUps < 0f) {
            int sortedPowerUpIndex = Random.Range(0, powerUpPrefabs.Length);
            float sortedZPosition = SortedZPosition();
            Vector3 spawnPosition = new Vector3(spawnPowerUps.position.x, spawnPowerUps.position.y, sortedZPosition);
            GameObject powerUp = Instantiate(powerUpPrefabs[sortedPowerUpIndex], spawnPosition, Quaternion.identity, spawnPowerUps);
            _powerUps.Add(powerUp);
            _currentTimeToSpawnPowerUps = timeToSpawnPowerUps;
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

    public void RemovePowerUp(GameObject powerUp) {
        _powerUps.Remove(powerUp);
    }

    public void SetPause(bool isPaused) {
        _isPaused = isPaused;
        _powerUps.ForEach(delegate (GameObject powerUp) {
            PowerUpMoviment powerUpScript = powerUp.GetComponent<PowerUpMoviment>();
            powerUpScript.SetPause(_isPaused);
        });
    }

}