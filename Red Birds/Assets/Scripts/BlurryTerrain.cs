    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [ExecuteInEditMode]
    public class BlurryTerrain : MonoBehaviour
    {
        //Add as Terrain Component and modify in text box
        public float BasemapDistance = 10000; // 10 km distance
        private Terrain terrain;
     
        void OnEnable ()
        {
            terrain = GetComponent<Terrain> ();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.update += Set;
            #endif
        }
     
        #if !UNITY_EDITOR
        void Update ()
        {
            Set();
        }
        #endif
     
        void OnDisable()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= Set;
            #endif
        }
     
        void Set ()
        {
            if (terrain == null)
                terrain = GetComponent<Terrain> ();
            else if (terrain.basemapDistance != BasemapDistance)
                terrain.basemapDistance = BasemapDistance;
        }
    }

