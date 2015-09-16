// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlNodeExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Xml;

    /// <summary>
    /// The xml node extensions.
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Safe way to get inner text of an attribute.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="attributeName">
        /// The attribute Name.
        /// </param>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T GetAttributeText<T>(this XmlNode node, string attributeName, T defaultValue = default(T))
        {
            try
            {
                if (node != null && attributeName.HasValue())
                {
                    XmlAttribute attr = node.Attributes[attributeName];
                    if (attr != null)
                    {
                        return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(attr.InnerText);
                    }
                }
            }
            catch (Exception exc)
            {
                exc.Dump();
            }

            return defaultValue;
        }

        /// <summary>
        /// Safe way to get inner text of an attribute.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="attributeName">
        /// The attribute Name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetAttributeText(this XmlNode node, string attributeName)
        {
            return node.GetAttributeText<string>(attributeName, null);
        }

        /// <summary>
        /// Safe way to get inner text of a node.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="xpath">
        /// The xpath.
        /// </param>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T GetText<T>(this XmlNode node, string xpath = null, T defaultValue = default(T))
        {
            try
            {
                if (node != null)
                {
                    if (xpath.IsEmpty())
                        return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(node.InnerText);

                    XmlNode n = node.SelectSingleNode(xpath);
                    if (n != null)
                        return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(n.InnerText);
                }
            }
            catch (Exception exc)
            {
                exc.Dump();
            }

            return defaultValue;
        }

        /// <summary>
        /// Safe way to get inner text of a node.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="xpath">
        /// The xpath.
        /// </param>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetText(this XmlNode node, string xpath = null, string defaultValue = default(string))
        {
            return node.GetText<string>(xpath, defaultValue);
        }
    }
}