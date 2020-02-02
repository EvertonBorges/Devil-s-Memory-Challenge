using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMoviment : MonoBehaviour {

    [SerializeField]
    private float speed = 0f;

    private MeshRenderer _renderer = null;
    private bool _isPaused = false;

    void Awake() {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Update() {
        if (_isPaused) return;

        _renderer.material.mainTextureOffset -= Vector2.left * speed * Time.deltaTime;
    }

    public void SetPause(bool isPaused) {
        _isPaused = isPaused;
    }

}