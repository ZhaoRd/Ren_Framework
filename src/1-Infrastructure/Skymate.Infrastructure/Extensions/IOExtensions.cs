// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System.IO;

    /// <summary>
    /// The io extensions.
    /// </summary>
    public static class IoExtensions
    {
        /// <summary>
        /// The is file locked.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsFileLocked(this FileInfo file)
        {
            if (file == null)
            {
                return false;
            }

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // the file is unavailable because it is:
                // still being written to
                // or being processed by another thread
                // or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }
    }
}