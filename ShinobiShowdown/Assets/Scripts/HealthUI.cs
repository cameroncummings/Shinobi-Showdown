using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour {

    public List<GameObject> healthUIList;
    public PlayerHealth health;
    private Stack<GameObject> healthUIStack;

    // Use this for initialization
    void Start () {
        PlayerHealth.DamageUI += TakeDamage;
        healthUIStack = new Stack<GameObject>(healthUIList);
    }

    // Update is called once per frame
    void TakeDamage ()
    {
        healthUIStack.Peek().SetActive(false);
        healthUIStack.Pop();
    }
}
