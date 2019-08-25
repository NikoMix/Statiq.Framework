﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Statiq.Common;

namespace Statiq.Core
{
    /// <summary>
    /// Sets the document destination.
    /// </summary>
    /// <remarks>
    /// This module is typically used before <see cref="WriteFiles"/> to set the
    /// <see cref="IDocument.Destination"/> prior to writing the document.
    /// If an extension is provided, this module will change the extension of the
    /// destination path for each input document.
    /// If the metadata keys <c>DestinationPath</c>, <c>DestinationFileName</c>,
    /// or <c>DestinationExtension</c> are set (in that order of precedence), the value
    /// will be used instead of the specified extension. For example, if you have a bunch
    /// of Razor .cshtml files that need to be rendered to .html files but one of them
    /// should be output as a .xml file instead, define the <c>DestinationExtension</c>
    /// metadata value in the front matter of the page.
    /// If a delegate is provided, it takes precedence over the metadata values (but can
    /// use them if needed).
    /// </remarks>
    /// <metadata cref="Keys.DestinationPath" usage="Input" />
    /// <metadata cref="Keys.DestinationFileName" usage="Input" />
    /// <metadata cref="Keys.DestinationExtension" usage="Input" />
    /// <category>Input/Output</category>
    public class SetDestination : ParallelConfigModule<FilePath>
    {
        private readonly Config<FilePath> _destination;

        /// <summary>
        /// Sets the destination of input documents according to the metadata values for
        /// <c>DestinationPath</c>, <c>DestinationFileName</c>, or <c>DestinationExtension</c>
        /// (in that order of precedence). If none of those metadata values are set, the
        /// destination will remain unchanged.
        /// </summary>
        public SetDestination()
            : base(Config.FromDocument(GetPathFromMetadata), true)
        {
            _destination = Config.FromDocument(GetPathFromMetadata);
        }

        /// <summary>
        /// Changes the destination extension of input documents to the specified extension.
        /// If <c>DestinationPath</c>, <c>DestinationFileName</c>, or <c>DestinationExtension</c>
        /// metadata values are set, those will take precedence.
        /// </summary>
        /// <param name="extension">The extension to set the destination to.</param>
        public SetDestination(string extension)
            : base(Config.FromDocument(doc => GetPathFromMetadata(doc) ?? doc.Destination?.ChangeExtension(extension ?? throw new ArgumentNullException(nameof(extension)))), true)
        {
        }

        /// <summary>
        /// Changes the destination of input documents to that of the delegate.
        /// </summary>
        /// <param name="destination">A delegate that returns a <see cref="FilePath"/> with the desired destination path.</param>
        public SetDestination(Config<FilePath> destination)
            : base(destination ?? throw new ArgumentNullException(nameof(destination)), true)
        {
        }

        private static FilePath GetPathFromMetadata(IDocument doc)
        {
            FilePath path = doc.FilePath(Keys.DestinationPath);
            if (path != null)
            {
                return path;
            }
            path = doc.FilePath(Keys.DestinationFileName);
            if (path != null)
            {
                return doc.Destination == null ? path : doc.Destination.ChangeFileName(path);
            }
            string extension = doc.String(Keys.DestinationExtension);
            if (!string.IsNullOrEmpty(extension) && doc.Destination != null)
            {
                return doc.Destination.ChangeExtension(extension);
            }
            return null;
        }

        protected override Task<IEnumerable<IDocument>> ExecuteAsync(IDocument input, IExecutionContext context, FilePath value) =>
            Task.FromResult(value == null ? input.Yield() : input.Clone(value).Yield());
    }
}