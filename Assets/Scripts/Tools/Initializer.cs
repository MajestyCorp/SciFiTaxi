using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi.Tools
{
    public interface IInitializer
    {
        /// <summary>
        /// Initialize only own independed stuff
        /// </summary>
        void InitInstance();
        /// <summary>
        /// Initialize dependencies
        /// </summary>
        void Initialize();
    }

    /// <summary>
    /// This script runs first in the game, so here we initialize all singleton classes
    /// </summary>
    public class Initializer : MonoBehaviour
    {
        private IInitializer[] array;

        private void Awake()
        {
            array = GetComponentsInChildren<IInitializer>();

            if (array != null && array.Length > 0)
            {
                //initialize all instances first
                for (int i = 0; i < array.Length; i++)
                    array[i].InitInstance();

                //after that initialize dependend stuff
                for (int i = 0; i < array.Length; i++)
                    array[i].Initialize();
            }
            else
            {
                Debug.LogWarning("Empty initializer", this.gameObject);
            }
        }
    }
}