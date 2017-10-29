﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour {

    // Use this for initialization
    public GameObject player;

    private List<GameObject> enemies;
    public float attackX;
    private bool hitNextEnemy;
    private int hitCounter = 0;
    public float knockBackVelocity = 1.5f;
    public float holdAttackFrameDuration;
    private IEnumerator attackFrameFunction;
    public Sprite[] attackFrames;
    private int lastAttackIndex;
    void Start () {
        enemies = new List<GameObject>();
        attackFrameFunction = holdAttackFrame(0, 0);
        lastAttackIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
		if(enemies.Count > 0 && GameController.gameState == GameController.GameState.InGame)
        {
            GameObject closestEnemy = getClosestEnemy();
            int nextNote = closestEnemy.GetComponent<NoteManager>().getNextNoteDirection();
            if(Input.GetKeyDown(KeyCode.UpArrow) && nextNote == 0)
            {
                hitNextEnemy = true;
                closestEnemy.GetComponent<NoteManager>().removeFront();
                closestEnemy.GetComponent<ColorLerp>().startColorChange();

            }
            else if(Input.GetKeyDown(KeyCode.RightArrow) && nextNote == 1)
            {
                hitNextEnemy = true;
                closestEnemy.GetComponent<NoteManager>().removeFront();
                closestEnemy.GetComponent<ColorLerp>().startColorChange();

            }
            else if(Input.GetKeyDown(KeyCode.DownArrow) && nextNote == 2)
            {
                hitNextEnemy = true;
                closestEnemy.GetComponent<NoteManager>().removeFront();
                closestEnemy.GetComponent<ColorLerp>().startColorChange();

            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow) && nextNote == 3)
            {
                hitNextEnemy = true;
                closestEnemy.GetComponent<NoteManager>().removeFront();
                closestEnemy.GetComponent<ColorLerp>().startColorChange();

            }
            if (hitNextEnemy)
            {
                if(closestEnemy.transform.position.x < attackX)
                {
                    hitNextEnemy = false;
                    knockBackAllEnemies(knockBackVelocity/2);
                    attack();
                    CamShake.shaking = true;
                    if (closestEnemy.gameObject.GetComponent<NoteManager>().empty())
                    {
                        GameObject boneExplosion = (GameObject)Instantiate(Resources.Load("BoneExplosion"));
                        boneExplosion.transform.position = closestEnemy.transform.position+ Vector3.down*0.5f;
                        boneExplosion.GetComponent<ParticleExplosion>().explode(20, 2);
                        Spawn.enemyList.Remove(closestEnemy);
                        GameController.defaultSpeed = Mathf.Min(GameController.defaultSpeed + .03f, GameController.maxSpeed);
                        Destroy(closestEnemy);

                        knockBackAllEnemies(knockBackVelocity);

                    }
                    GameController.globalSpeed = GameController.defaultSpeed;
                } else
                {
                    GameController.globalSpeed = 5;
                }

            }
            
        }
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") == true)
        {
            enemies.Add(other.gameObject);
        }

    }

   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.Equals(null))
        {
            enemies.Remove(collision.gameObject);
        }

    }
    public GameObject getClosestEnemy()
    {
        float min = 10000000;
        int index = 0;
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i].transform.position.x < min)
            {
                min = enemies[i].transform.position.x;
                index = i;
            }
        }

        return enemies[index];
    }
    public void knockBackAllEnemies(float force)
    {
        int enemyCount = Spawn.enemyList.Count;
        for (int i = 0; i < enemyCount; ++i)
        {
            Spawn.enemyList[i].GetComponent<MoveLeft>().speed = -force;
        }
    }
    private void attack()
    {
        hitCounter++;
        StopCoroutine(attackFrameFunction);
        int rng = 0;
        while (attackFrames.Length > 1)
        {
            rng = Random.Range(0, attackFrames.Length);
            if (rng != lastAttackIndex)
            {
                break;
            }
        }
        lastAttackIndex = rng;
        attackFrameFunction = holdAttackFrame(holdAttackFrameDuration, rng);
        StartCoroutine(attackFrameFunction);
    }
    private IEnumerator holdAttackFrame(float duration, int attackFrameIndex)
    {
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<SpriteRenderer>().sprite = attackFrames[attackFrameIndex];
        yield return new WaitForSeconds(duration);
        player.GetComponent<Animator>().enabled = true;
    }
}
