﻿using UnityEngine;
using System.Collections;

public class PickUpItem : MonoBehaviour
{
    public Item item;
    private Inventory _inventory;
    private GameObject _player;
    private float speedMultiplier;
    private PlayerInventory pi;

    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _inventory = _player.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();
            pi = _player.GetComponent<PlayerInventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.gameObject.transform.position;
        // characPos = center of the player;
        Vector3 characPos = _player.transform.position;
        characPos.y += _player.transform.localScale.y / 2;
        float distance = Vector3.Distance(this.gameObject.transform.position, characPos);
        speedMultiplier = distance * 0.05f;

        if (distance <= 1)
        {
            pi.brokenItemID = this.gameObject.GetComponent<PickUpItem>().item.itemID;
            if (!pi.FullInventory())
            {
                // moving the item to the player
                if (newPos.x - characPos.x < 0)
                {
                    newPos.x += speedMultiplier;
                }
                else if (newPos.x - characPos.x > 0)
                {
                    newPos.x -= speedMultiplier;
                }

                if (newPos.y - characPos.y < 0)
                {
                    newPos.y += speedMultiplier;
                }
                else if (newPos.y - characPos.y > 0)
                {
                    newPos.y -= speedMultiplier;
                }
                this.gameObject.transform.position = newPos;

                // picking up the item
                if (_inventory != null && Mathf.Abs(this.gameObject.transform.position.x - characPos.x) <= 0.3f
                    && Mathf.Abs(this.gameObject.transform.position.y - characPos.y) <= 0.3f * ( _player.GetComponent<CapsuleCollider2D>().offset.y / _player.GetComponent<CapsuleCollider2D>().offset.x ))
                {
                    bool check = _inventory.checkIfItemAllreadyExist(item.itemID, item.itemValue);
                    if (check)
                        Destroy(this.gameObject);
                    else if (_inventory.ItemsInInventory.Count < (_inventory.width * _inventory.height))
                    {
                        _inventory.addItemToInventory(item.itemID, item.itemValue);
                        _inventory.updateItemList();
                        _inventory.stackableSettings();
                        Destroy(this.gameObject);
                    }
                    if(!pi.currentLevel.Equals(pi.maxLevel))
                        pi.GainExp(item.expToGain);
                }
            }
        }
    }
}