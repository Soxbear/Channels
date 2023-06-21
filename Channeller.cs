using System;
using System.Collections.Generic;
using UnityEngine;
using Soxbear.Channels;

namespace Soxbear.Channels {
    public class Channeller : MonoBehaviour
    {
        // public delegate void setup();
        // public static setup onSetup;

        public Dictionary<Type, Dictionary<int, Channel>> Channels = new Dictionary<Type, Dictionary<int, Channel>>();

        bool hasSetup;

        void Awake() {
            //onSetup();
        }
    }
}