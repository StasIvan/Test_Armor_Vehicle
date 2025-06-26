﻿using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameItems.EnemyItem
{
    public class EnemySeeker : MonoBehaviour
    {
        [SerializeReference] private EnemyItem _item;
        
        private void OnTriggerEnter(Collider other)
        {
            var dmg = other.GetComponent<IDamageable>();
            if (dmg == null) return;
            
            _item.SetTarget(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            _item.SetTarget(null);
        }
    }
}