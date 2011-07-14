﻿using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources.Implementations
{
    public class GroupResource : ResourceBase
    {
        public GroupResource(XmlNode node)
            : base(node)
        {
        }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/group.png")); } }
    }
}