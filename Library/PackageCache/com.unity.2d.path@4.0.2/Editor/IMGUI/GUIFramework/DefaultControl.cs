﻿using System;
using UnityEngine;

namespace UnityEditor.U2D.Path.GUIFramework
{
    public abstract class DefaultControl : Control
    {
        public static readonly float kPickDistance = 5f;
        
        public DefaultControl(string name) : base(name)
        {
        }

        protected override LayoutData OnBeginLayout(LayoutData data, IGUIState guiState)
        {
            data.distance = kPickDistance;
            return data;
        }
    }
}
