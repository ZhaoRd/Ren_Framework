// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.IO;

    /// <summary>
    /// The stream extensions.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// The to file.
        /// </summary>
        /// <param name="srcStream">
        /// The src stream.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ToFile(this Stream srcStream, string path)
        {
            if (srcStream == null)
            {
                return false;
            }

            const int BuffSize = 32768;
            var result = true;
            Stream dstStream = null;
            var buffer = new byte[BuffSize];

            try
            {
                using (dstStream = File.OpenWrite(path))
                {
                    int len;
                    while ((len = srcStream.Read(buffer, 0, BuffSize)) > 0)
                    {
                        dstStream.Write(buffer, 0, len);
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (dstStream != null)
                {
                    dstStream.Close();
                    dstStream.Dispose();
                }
            }

            return result && File.Exists(path);
        }

        /// <summary>
        /// The contents equal.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ContentsEqual(this Stream src, Stream other)
        {
            Guard.ArgumentNotNull(() => src);
            Guard.ArgumentNotNull(() => other);

            if (src.Length != other.Length)
            {
                return false;
            }

            const int BufferSize = 2048;
            var buffer1 = new byte[BufferSize];
            var buffer2 = new byte[BufferSize];

            while (true)
            {
                var len1 = src.Read(buffer1, 0, BufferSize);
                var len2 = other.Read(buffer2, 0, BufferSize);

                if (len1 != len2)
                {
                    return false;
                }

                if (len1 == 0)
                {
                    return true;
                }

                var iterations = (int)Math.Ceiling((double)len1 / sizeof(long));
                for (var i = 0; i < iterations; i++)
                {
                    if (BitConverter.ToInt64(buffer1, i * sizeof(long)) != BitConverter.ToInt64(buffer2, i * sizeof(Int64)))
                    {
                        return false;
                    }
                }
            }
        }
    }
}