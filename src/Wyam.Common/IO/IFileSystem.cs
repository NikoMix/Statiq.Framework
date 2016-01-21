﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization.Configuration;

namespace Wyam.Common.IO
{
    // Initially based on code from Cake (http://cakebuild.net/)
    /// <summary>
    /// Represents a file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets the root path.
        /// </summary>
        /// <value>
        /// The root path.
        /// </value>
        DirectoryPath RootPath { get; }

        /// <summary>
        /// Gets the input paths. These are searched in reverse order for
        /// files and directories. For example, given input paths "A", "B",
        /// and "C" in that order, "C" will be checked for a requested file 
        /// or directory first, and then if it doesn't exist in "C", "B" 
        /// will be checked, and then "A". If none of the input paths contain
        /// the requested file or directory, the last input path (in this case, 
        /// "C") will be used as the location of the requested non-existent file
        /// or directory. If you attempt to create it at this point, it will be
        /// created under path "C".
        /// </summary>
        /// <value>
        /// The input paths.
        /// </value>
        IReadOnlyList<DirectoryPath> InputPaths { get; }

        /// <summary>
        /// Gets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        DirectoryPath OutputPath { get; }

        /// <summary>
        /// Gets a file representing an input.
        /// </summary>
        /// <param name="path">
        /// The path of the input file. If this is an absolute path,
        /// then a file representing the specified path is returned.
        /// If it's a relative path, then operations will search all
        /// current input paths.
        /// </param>
        /// <returns>A path to an input file.</returns>
        IFile GetInput(FilePath path);

        /// <summary>
        /// Gets a directory representing an input.
        /// </summary>
        /// <param name="path">
        /// The path of the input directory. If this is an absolute path,
        /// then a directory representing the specified path is returned.
        /// If it's a relative path, then operations will search all
        /// current input paths.
        /// </param>
        /// <returns>A path to an input directory.</returns>
        IDirectory GetInput(DirectoryPath path);

        /// <summary>
        /// Gets a file representing an output.
        /// </summary>
        /// <param name="path">
        /// The path of the output file. If this is an absolute path,
        /// then a file representing the specified path is returned.
        /// If it's a relative path, then it will be combined with the
        /// current output path.
        /// </param>
        /// <returns>A path to an output file.</returns>
        IFile GetOutput(FilePath path);

        /// <summary>
        /// Gets a directory representing an output.
        /// </summary>
        /// <param name="path">
        /// The path of the output directory. If this is an absolute path,
        /// then a directory representing the specified path is returned.
        /// If it's a relative path, then it will be combined with the
        /// current output path.
        /// </param>
        /// <returns>A path to an output directory.</returns>
        IDirectory GetOutput(DirectoryPath path);

        /// <summary>
        /// Gets a file representing a root file.
        /// </summary>
        /// <param name="path">
        /// The path of the root file. If this is an absolute path,
        /// then a file representing the specified path is returned.
        /// If it's a relative path, then it will be combined with the
        /// current root path.
        /// </param>
        /// <returns>A path to an root file.</returns>
        IFile GetRoot(FilePath path);

        /// <summary>
        /// Gets a directory representing a root directory.
        /// </summary>
        /// <param name="path">
        /// The path of the root directory. If this is an absolute path,
        /// then a directory representing the specified path is returned.
        /// If it's a relative path, then it will be combined with the
        /// current root path.
        /// </param>
        /// <returns>A path to an root directory.</returns>
        IDirectory GetRoot(DirectoryPath path);
    }
}