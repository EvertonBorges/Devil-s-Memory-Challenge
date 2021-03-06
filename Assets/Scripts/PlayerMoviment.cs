﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerEnum { 
    PLAYER1, PLAYER2
}

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerMoviment : MonoBehaviour {

    [SerializeField]
    private float speedMoviment = 0f;

    [SerializeField]
    private PlayerEnum playerEnum = PlayerEnum.PLAYER1;

    [SerializeField]
    private float jumpForce = 0f;

    [SerializeField]
    private Transform groundCheck = null;

    [SerializeField]
    private LayerMask groundLayer = 0;

    [SerializeField]
    private Text textMemory = null;

    [SerializeField]
    private int maxMemories = 0;

    [SerializeField]
    private Image pushButton = null;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource jumpAudio = null;
    [SerializeField]
    private AudioSource dieAudio = null;
    [SerializeField]
    private AudioSource hitAudio = null;
    [SerializeField]
    private AudioSource powerUpSound = null;

    // Push Parameters
    private int _maxPushCount = 8;
    private int _pushCount = 0;
    private int _powerPush = 1;

    // Z Moviment
    private bool _isGoingFoward = false;
    private float _timeInZMoviment = 0f;
    private float _finalZPosition = 0f;
    private bool _isInZMoviment = false;

    // Speed Parameters
    private float _multiplierFast = 1.75f;
    private float _multiplierSlow = 0.65f;
    private float _currentSpeed = 0f;

    // Memory Parameters
    private int _memories = 0;

    private bool _isOnGround = true;
    private bool _isPaused = false;
    private bool _isAlive = true;
    private Animator _animator = null;
    private Rigidbody _rigidbody = null;

    private float _currentTimeToFinishPowerUpDoublePush = 2f;
    private float _currentTimeToFinishPowerUpFast = 2f;
    private float _currentTimeToFinishPowerUpSlow = 2f;

    private float _speedFast = 0f;
    private float _speedSlow = 0f;

    private GameController _gameController;

    void Awake() {
        _pushCount = _maxPushCount;
        _currentSpeed = speedMoviment;
        _speedFast = speedMoviment * _multiplierFast;
        _speedSlow = speedMoviment * _multiplierSlow;

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        UpdatePushPanel();
    }

    void Update() {

        if (_isPaused) return;

        _animator.SetBool("IsAlive", _isAlive);

        if ((transform.position.z >= 3.125f || transform.position.z <= -3.125f) && _isAlive) {
            Die();
            return;
        }

        PowerUps();
        JumpMoviment();
        HorizontalMoviment();
        VerticalMoviment();
    }

    private void PowerUps() {
        PowerUpDoublePush();
        PowerUpFast();
        PowerUpSlow();
    }

    private void PowerUpDoublePush() { 
        if (_powerPush == 2) {
            _currentTimeToFinishPowerUpDoublePush -= Time.deltaTime;
            if (_currentTimeToFinishPowerUpDoublePush < 0f) {
                _powerPush = 1;
            }
        }
    }

    private void PowerUpFast() { 
        if (_currentSpeed == _speedFast) {
            _currentTimeToFinishPowerUpFast -= Time.deltaTime;
            if (_currentTimeToFinishPowerUpFast < 0f) {
                _animator.speed = 1f;
                _currentSpeed = speedMoviment;
            }
        }
    }

    private void PowerUpSlow() { 
        if (_currentSpeed == _speedSlow) {
            _currentTimeToFinishPowerUpSlow -= Time.deltaTime;
            if (_currentTimeToFinishPowerUpSlow < 0f) {
                _animator.speed = 1f;
                _currentSpeed = speedMoviment;
            }
        }
    }

    private void JumpMoviment() {
        _isOnGround = Physics.Linecast(transform.position, groundCheck.position, groundLayer);
        _animator.SetBool("OnGround", _isOnGround);

        bool isToJump = (playerEnum == PlayerEnum.PLAYER1) ? Input.GetKeyDown(KeyCode.Space) : Input.GetKeyDown(KeyCode.LeftShift);

        if (_isOnGround && isToJump) {
            jumpAudio.Play();
            _animator.SetTrigger("Jump");
            _rigidbody.AddForce(Vector3.up * jumpForce);
        }
    }

    private void HorizontalMoviment() { 
        float horizontal = (playerEnum == PlayerEnum.PLAYER1) ? Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal2");

        if (horizontal != 0f) {
            if (transform.position.x >= -7.5f && transform.position.x <= 15f) {
                transform.position += Vector3.right * horizontal * _currentSpeed * Time.deltaTime;
            } else { 
                if (transform.position.x <= -7.5f && horizontal > 0f) {
                    transform.position += Vector3.right * horizontal * _currentSpeed * Time.deltaTime;
                } else if (transform.position.x >= 15f && horizontal < 0f) {
                    transform.position += Vector3.right * horizontal * _currentSpeed * Time.deltaTime;
                }
            }
        }
    }

    private void VerticalMoviment() { 
        if (!_isInZMoviment) {
            float vertical = (playerEnum == PlayerEnum.PLAYER1) ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical2");

            if (vertical == 0f) return;
            if (transform.position.z >= 1.875f && vertical > 0) return;
            if (transform.position.z <= -1.875f && vertical < 0) return;

            DetermineFinalZPosition(vertical);
            _isGoingFoward = vertical > 0f;
            _isInZMoviment = true;
            _timeInZMoviment = 0f;
        } else {
            if (_finalZPosition == 0f) return;

            transform.position += Vector3.forward * _currentSpeed * (_isGoingFoward ? 1 : -1) * Time.deltaTime;
            _timeInZMoviment += Time.deltaTime;

            if (_isGoingFoward) {
                if (transform.position.z >= _finalZPosition) {
                    StopMoviment();
                }
            } else { 
                if (transform.position.z <= _finalZPosition) {
                    StopMoviment();
                }
            }
        }
    }

    private void StopMoviment() {
        transform.position = new Vector3(transform.position.x, transform.position.y, _finalZPosition);
        _finalZPosition = 0f;
        _isInZMoviment = false;
        _timeInZMoviment = 0f;
    }

    private void DetermineFinalZPosition(float vertical) { 
        if (vertical > 0f) { 
            if (transform.position.z <= -1.875f) {
                _finalZPosition = -0.625f;
            } else if (transform.position.z <= -0.625f) {
                _finalZPosition = 0.625f;
            } else if (transform.position.z <= 0.625f) {
                _finalZPosition = 1.875f;
            }
        } else { 
            if (transform.position.z >= 1.875f) {
                _finalZPosition = 0.625f;
            } else if (transform.position.z >= 0.625f) {
                _finalZPosition = -0.625f;
            } else if (transform.position.z >= -0.625f) {
                _finalZPosition = -1.875f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Collider collider = collision.collider;

        if (collider.CompareTag("Player")) {
            PlayerMoviment scriptEnemy = collider.GetComponent<PlayerMoviment>();
            bool hitByTop = (scriptEnemy.transform.position.z - transform.position.z) > 0;

            if (scriptEnemy.CompareTimeZInMoviment(_timeInZMoviment)) {
                float nextZPosition = transform.position.z + scriptEnemy.GetPowerPush() * (hitByTop ? -1.25f : 1.25f);
                transform.position = new Vector3(transform.position.x, transform.position.y, nextZPosition);
                hitAudio.Play();
                _animator.SetTrigger("Get");
            } else {

                _animator.SetTrigger("Push");
            }

            if (hitByTop) _animator.SetTrigger("Up");
            else _animator.SetTrigger("Down");
        }
    }

    public bool CompareTimeZInMoviment(float timeInMoviment) {
        if (_timeInZMoviment > timeInMoviment) {
            return true;
        }
        return false;
    }

    private void Pause() {
        if (_isPaused) {
            _animator.speed = 0f;
        } else {
            _animator.speed = 1f;
        }
    }

    private void Die() {
        dieAudio.Play();
        _isAlive = false;
        _animator.SetTrigger("Die");
    }

    private void PostDie() {
        _animator.speed = 0f;
        PlayerEnum winPlayer = (playerEnum == PlayerEnum.PLAYER1) ? PlayerEnum.PLAYER2 : PlayerEnum.PLAYER1;
        //winAudio.Play();
        Win(winPlayer);
    }

    private void Win(PlayerEnum winPlayer) {
        _gameController.Win(winPlayer);
        Destroy(gameObject);
    }

    public PlayerEnum GetPlayerEnum() {
        return playerEnum;
    }

    public int GetPowerPush() {
        _pushCount--;
        if (_pushCount < 0) {
            _pushCount = 0;
        }
        UpdatePushPanel();
        return _powerPush;
    }

    public void GetPowerUp(PowerUpEnum powerUp) {
        powerUpSound.Play();

        switch (powerUp) {
            case PowerUpEnum.DOUBLE_PUSH:
                _pushCount++;
                if (_pushCount > _maxPushCount) {
                    _pushCount = _maxPushCount;
                }
                _currentTimeToFinishPowerUpDoublePush = 2f;
                _powerPush = 2;
                UpdatePushPanel();
                break;
            case PowerUpEnum.FAST:
                _currentTimeToFinishPowerUpFast = 2f;
                _animator.speed = _multiplierFast;
                _currentSpeed = _speedFast;
                break;
            case PowerUpEnum.SLOW:
                _currentTimeToFinishPowerUpSlow = 2f;
                _animator.speed = _multiplierSlow;
                _currentSpeed = _speedSlow;
                break;
        }
    }

    public void GetMemory() {
        _memories++;
        textMemory.text = _memories.ToString();

        if (_memories == maxMemories) {
            Win(playerEnum);
        }
    }

    private void UpdatePushPanel() {
        pushButton.fillAmount = (float)_pushCount / (float)_maxPushCount;
    }

    public void SetPause(bool pause) {
        _isPaused = pause;
        Pause();
    }

}