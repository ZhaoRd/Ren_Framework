// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlWriterExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Xml;

    /// <summary>
    /// The xml writer extensions.
    /// </summary>
    /// <remarks>
    /// codehint: sm-add
    /// </remarks>
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// The write c data.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <param name="ns">
        /// The ns.
        /// </param>
        public static void WriteCData(this XmlWriter writer, string name, string value, string prefix = null, string ns = null)
        {
            if (name.HasValue() && value != null)
            {
                if (prefix == null && ns == null)
                {
                    writer.WriteStartElement(name);
                }
                else
                {
                    writer.WriteStartElement(prefix, name, ns);
                }

                writer.WriteCData(value.RemoveInvalidXmlChars());
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// The write node.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="content">
        /// The content.
        /// </param>
        public static void WriteNode(this XmlWriter writer, string name, Action content)
        {
            if (name.HasValue() && content != null)
            {
                writer.WriteStartElement(name);
                content();
                writer.WriteEndElement();
            }
        }
    }
}