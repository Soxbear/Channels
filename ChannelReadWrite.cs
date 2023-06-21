using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Soxbear.Channels;

namespace Soxbear.Channels {
    [Serializable]
    public class ChannelReadWrite<T> {
        // public delegate void setup();

        // public static setup onSetup;

        public Type type {
            get {
                return typeof(T);
            }
        }

        public string typeName;        

        public bool enabled = true;

        public Channeller channeller;

        public int channelNumber = 0;

        public Channel channel;

        public T channelValue {
            get {
                if (channel == null) {Setup();}
                return (T) channel.value;
            }
            set {
                if (channel == null) {Setup();}
                channel.setValue(value);
            }
        }

        public delegate void update(T value);

        public update onUpdate {
            get {
                if (channel == null) {Setup();}
                return updateInternal;
            }
            set {
                if (channel == null) {Setup();}
                updateInternal = value;
            }
        }

        private update updateInternal;

        void Setup() {
            if (channeller == null) {
                Debug.LogError("Cannot setup ChannelReadWrite without a Channeller");
                return;
            }

            if (channeller.Channels.ContainsKey(type)) {
                if (channeller.Channels[type].ContainsKey(channelNumber))
                    channel = channeller.Channels[type][channelNumber];
                else {
                    channeller.Channels[type].Add(channelNumber, new Channel());
                    channel = channeller.Channels[type][channelNumber];
                }
            }
            else {
                channeller.Channels.Add(type, new Dictionary<int, Channel>());
                channeller.Channels[type].Add(channelNumber, new Channel());
                channel = channeller.Channels[type][channelNumber];
            }

            updateInternal += (obj) => {}; 

            channel.onUpdate += (obj) => {
                updateInternal((T) obj);
            };
            //Channeller.onSetup -= Setup;
        }

        public ChannelReadWrite() {
            typeName = type.Name;
            //Channeller.onSetup += Setup;
        }
    }

    [CustomPropertyDrawer(typeof(ChannelReadWrite<>))]
    public class ChannelReadWriteUI : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement ui = new VisualElement();

            // TextElement nameLabel = new TextElement();
            // nameLabel.text = 
            // ui.Add(nameLabel);

            Foldout fold = new Foldout();
            fold.text = ObjectNames.NicifyVariableName(property.name) + "   -   " + property.FindPropertyRelative("typeName").stringValue;
            fold.contentContainer.Add(new PropertyField(property.FindPropertyRelative("enabled")));
            fold.contentContainer.Add(new PropertyField(property.FindPropertyRelative("channeller")));
            fold.contentContainer.Add(new PropertyField(property.FindPropertyRelative("channelNumber")));

            ui.Add(fold);

            return ui;
        }
    }
}