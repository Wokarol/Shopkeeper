using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shopkeeper
{
    public class MenuBrain : MonoBehaviour
    {
        public void ChangeScene(string target)
        {
            SceneManager.LoadScene(target);
        }
    }
}
