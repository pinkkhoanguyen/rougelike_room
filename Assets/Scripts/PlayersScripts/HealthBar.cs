using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class HealthBar : MonoBehaviour
    {
        private Slider hpBar;

        private void Awake()
        {
            hpBar = GetComponentInChildren<Slider>();
        }

        public void configDefault(int maxValue) {
            hpBar.maxValue = maxValue;
            setHP(maxValue);
        }

        public void setHP(int hp) {
            hpBar.value = hp; 
        }
    }
}