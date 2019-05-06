﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviourScript : MonoBehaviour
{

    #region Movement Variables.
    [Header("Movement Parameters")]
    [Range(0, 10)]
    public float moveSpeed;
    #endregion

    #region Camera Variables.
    Vector2 mouseLook;
    [Range(0.1f, 10)]
    public float mouseSensitivity = 5.0f;
    #endregion

    #region Physics Variables.
    private Vector3 velocityY = Vector3.zero;
    #endregion

    #region State Variables.
    private bool isGrounded = false;
    private bool disableInput = false;
    #endregion

    #region Reference Variables.
    private CharacterController _charController;
    private GameObject _player;
    private GameObject _groundCheck;
    private GameObject _fpscam;
    private GameObject deathText;
    #endregion

    #region Health Variables.
    [Tooltip("Max Player Lives.")]
    public int lives;
    [Tooltip("Maximum Player Health")]
    public int maxHealth = 100;
    private int health;
    private Slider healthBar;
    #endregion

    #region Respawn
    private Vector3 spawnPosition;
    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        LoadComponentReferences();
    }

    private void LoadComponentReferences()
    {
        _player = this.transform.parent.gameObject;
        _charController = _player.GetComponent<CharacterController>();
        _groundCheck = this.transform.parent.transform.GetChild(4).gameObject;
        _fpscam = this.transform.parent.transform.GetChild(3).gameObject;
        health = maxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<Slider>();
        healthBar.value = health;
        deathText = GameObject.FindGameObjectWithTag("DeathTxt");
        deathText.SetActive(false);
        spawnPosition = transform.parent.position;
    }

    void Update()
    {
        CustomPhysics();
        if (!disableInput)
            CheckForPlayerInput();
        UpdateHealthBarAndCheckPlayerDeath();
    }

    private void CheckForPlayerInput()
    {
        UnlockCursor();
        PlayerMovementInput();
        Rotate();
    }

    private void UnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void PlayerMovementInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        MovePlayer(x, z);
    }

    private void MovePlayer(float x, float z)
    {
        Vector3 forwardMove = transform.forward * z;
        Vector3 strafeMove = transform.right * x;
        Vector3 move = forwardMove + strafeMove;
        _charController.Move(move * moveSpeed * Time.deltaTime);
        //Debug.Log(move);
    }

    private void CustomPhysics()
    {
        GroundCheck();
        AdvancedGravity();
    }

    private void SimpleGravity()
    {
        Vector3 gravity = new Vector3(0, Physics.gravity.y, 0);
        _charController.Move(gravity * Time.deltaTime);
    }

    private void AdvancedGravity()
    {
        if (isGrounded)
            velocityY = Vector3.zero;
        else
        {
            Vector3 gravity = new Vector3(0, Physics.gravity.y, 0);
            velocityY.y += gravity.y / 25;
            velocityY.y = Mathf.Clamp(velocityY.y, -100f, 100f);
            _charController.Move(velocityY * Time.deltaTime);
        }
        //Debug.Log(velocityY);
    }

    private void GroundCheck()
    {
        //Debug.Log(isGrounded);
        if (Physics.CheckSphere(_groundCheck.transform.position, 0.1f))
            isGrounded = true;
        else
            isGrounded = false;
    }

    void Rotate()
    {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseLook += Vector2.Scale(md, new Vector2(mouseSensitivity, mouseSensitivity));
        mouseLook.y = Mathf.Clamp(mouseLook.y, -87f, 87f);
        _fpscam.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        _player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, _player.transform.up);
    }

    void AddRecoil(float vRecoilAmount, float hRecoilAmount, bool xdirection)
    {
        
    }

    public void ReceivedDmg(int dmg)
    {
        health -= dmg;
        healthBar.value = health;
    }

    private void UpdateHealthBarAndCheckPlayerDeath()
    {
        if (health > 0)
        {

        }
        else
        {
            disableInput = true;
            healthBar.value = 0;
            deathText.SetActive(true);
            StartCoroutine("Respawn");
            
            //Destroy(transform.parent.gameObject, 3);
        }
    }

    public void SetRespawn(float x, float y, float z)
    {
        spawnPosition = new Vector3(x, y, z);
    }

    private void ResetLifeData()
    {
        health = maxHealth;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5);
        if (lives <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
        {
            transform.parent.position = spawnPosition;
            ResetLifeData();
            lives--;
            deathText.SetActive(false);
            disableInput = false;
            Debug.Log("Test Respawn");
            Debug.Log(lives);
        }
        yield break;
    }

    public void AddHealth(int amount)
    {
        if (amount + health > 100)
        {
            health = 100;
            healthBar.value = health;
        }
        else
        {
            health = health + amount;
            healthBar.value = health;
        }
    }

}
