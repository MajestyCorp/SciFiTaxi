using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scifi
{
    public interface IInitializer
    {
        /// <summary>
        /// Initialize own class data stuff
        /// </summary>
        void InitInstance();
        /// <summary>
        /// Here can be initialized cross-linked data stuff
        /// </summary>
        void Initialize();
    }
}