/////////////////////////////////////////////////////////////////////////////////
//
// Photoshop PSD FileType Plugin for Paint.NET
// http://psdplugin.codeplex.com/
//
// This software is provided under the MIT License:
//   Copyright (c) 2006-2007 Frank Blumenberg
//   Copyright (c) 2010-2016 Tao Yue
//
// Portions of this file are provided under the BSD 3-clause License:
//   Copyright (c) 2006, Jonas Beckeman
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoshopFile
{
    /// <summary>
    /// Contains settings and callbacks that affect the loading of a PSD file.
    /// </summary>
    internal class LoadContext
    {
        public Encoding Encoding { get; set; }

        public LoadContext()
        {
            Encoding = Encoding.Default;
        }

        public virtual void OnLoadLayersHeader(PsdFile psdFile)
        {
        }

        public virtual void OnLoadLayerHeader(Layer layer)
        {
        }
    }
}
