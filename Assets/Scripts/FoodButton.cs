using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodButton : MonoBehaviour
{
    [SerializeField] public Transform food;

    public bool isCanPushButton = false;
    public bool isCanUseButton { get;set;}
    public bool isSpawnFood
    {
        get
        {
            return food.gameObject.activeSelf;
        }
    }
    BlueAgent _agnet;

    public void SpawnFood()
    {
        food.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f));
        food.gameObject.SetActive(true);
        isCanUseButton = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<BlueAgent>(out _agnet))
        {
            isCanPushButton = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BlueAgent>(out _agnet))
        {
            isCanPushButton = false;
        }
    }
}
