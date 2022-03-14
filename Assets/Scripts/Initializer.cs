using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    /// <summary>
    /// This script runs first in the game, so here we initialize all singleton classes
    /// </summary>
    public class Initializer : MonoBehaviour
    {
        private IInitializer[] array;

        private void Awake()
        {
            array = GetComponentsInChildren<IInitializer>();

            if (array != null)
            {
                //initialize all instances first
                for (int i = 0; i < array.Length; i++)
                    array[i].InitInstance();

                //after that initialize linked stuff
                for (int i = 0; i < array.Length; i++)
                    array[i].Initialize();
            }
        }
    }
}