﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    [SerializeField]
    private AudioSource audioStart;
    [SerializeField]
    private AudioSource audioLoop;
    [SerializeField]
    private AudioSource audioFinal;

    private bool _playTime = false;
    private bool _isPlaying = false;

    void Awake() {
        
    }

    void Update() {
        if (!audioStart.isPlaying && !audioLoop.isPlaying && !_playTime) {
            audioLoop.Play();
        } else if (_isPlaying && _playTime && !audioFinal.isPlaying) {
            SceneController.Play();
        } else if (!_isPlaying && _playTime && audioLoop.isPlaying) {
            audioFinal.Play();
            _isPlaying = true;
        }
    }

    public void Play() {
        _playTime = true;
    }

}