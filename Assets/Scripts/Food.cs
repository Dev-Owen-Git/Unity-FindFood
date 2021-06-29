using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
   public void EatFood()
   {
       gameObject.SetActive( false );
   }
}
