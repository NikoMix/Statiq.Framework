﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Wyam.Common;
using Wyam.Common.Configuration;
using Wyam.Common.Documents;
using Wyam.Common.IO;
using Wyam.Common.Meta;
using Wyam.Core.Modules.IO;
using Wyam.Testing;
using Wyam.Testing.Documents;

namespace Wyam.Core.Tests.Modules.IO
{
    [TestFixture]
    public class SetDestinationFixture : BaseFixture
    {
        public class ExecuteTests : SetDestinationFixture
        {
            [TestCase(Keys.DestinationPath, "OtherFolder/foo.bar", "OtherFolder/foo.bar")]
            [TestCase(Keys.DestinationPath, "/OtherFolder/foo.bar", "/OtherFolder/foo.bar")]
            [TestCase(Keys.DestinationFileName, "foo.bar", "Subfolder/foo.bar")]
            [TestCase(Keys.DestinationFileName, "fizz/foo.bar", "Subfolder/fizz/foo.bar")]
            [TestCase(Keys.DestinationFileName, "../fizz/foo.bar", "fizz/foo.bar")]
            [TestCase(Keys.DestinationFileName, "/foo.bar", "/foo.bar")]
            [TestCase(Keys.DestinationFileName, "/fizz/foo.bar", "/fizz/foo.bar")]
            [TestCase(Keys.DestinationExtension, "foo", "Subfolder/write-test.foo")]
            [TestCase(Keys.DestinationExtension, ".foo", "Subfolder/write-test.foo")]
            public async Task SetsDestinationFromMetadata(string key, string value, string expected)
            {
                // Given
                TestDocument input = new TestDocument(new FilePath("Subfolder/write-test.abc"))
                {
                    { key, value }
                };
                SetDestination setDestination = new SetDestination();

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe(expected);
            }

            [TestCase(Keys.DestinationPath, "OtherFolder/foo.bar", "OtherFolder/foo.bar")]
            [TestCase(Keys.DestinationPath, "/OtherFolder/foo.bar", "/OtherFolder/foo.bar")]
            [TestCase(Keys.DestinationFileName, "foo.bar", "Subfolder/foo.bar")]
            [TestCase(Keys.DestinationFileName, "fizz/foo.bar", "Subfolder/fizz/foo.bar")]
            [TestCase(Keys.DestinationFileName, "../fizz/foo.bar", "fizz/foo.bar")]
            [TestCase(Keys.DestinationFileName, "/foo.bar", "/foo.bar")]
            [TestCase(Keys.DestinationFileName, "/fizz/foo.bar", "/fizz/foo.bar")]
            [TestCase(Keys.DestinationExtension, "foo", "Subfolder/write-test.foo")]
            [TestCase(Keys.DestinationExtension, ".foo", "Subfolder/write-test.foo")]
            public async Task SetsDestinationFromMetadataOverridesExtension(string key, string value, string expected)
            {
                // Given
                TestDocument input = new TestDocument(new FilePath("Subfolder/write-test.abc"))
                {
                    { key, value }
                };
                SetDestination setDestination = new SetDestination(".txt");

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe(expected);
            }

            [TestCase(Keys.DestinationPath)]
            [TestCase(Keys.DestinationFileName)]
            [TestCase(Keys.DestinationExtension)]
            public async Task SetsDestinationFromDelegateOverridesMetadata(string key)
            {
                // Given
                TestDocument input = new TestDocument(new FilePath("Subfolder/write-test.abc"))
                {
                    { key, "foo" }
                };
                SetDestination setDestination = new SetDestination(
                    Config.FromDocument(doc => doc.Destination.ChangeExtension(".bar")));

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe("Subfolder/write-test.bar");
            }

            public async Task SetsDestinationFromDocumentDelegate()
            {
                // Given
                TestDocument input = new TestDocument(new FilePath("Subfolder/write-test.abc"));
                SetDestination setDestination = new SetDestination(
                    Config.FromDocument(doc => doc.Destination.ChangeExtension(".bar")));

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe("Subfolder/write-test.bar");
            }

            public async Task SetsDestinationFromStringDelegate()
            {
                // Given
                TestDocument input = new TestDocument(new FilePath("Subfolder/write-test.abc"));
                SetDestination setDestination = new SetDestination("foo/bar.txt");

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe("foo/bar.txt");
            }

            [Test]
            public async Task ExtensionWithDot()
            {
                // Given
                TestDocument input = new TestDocument(new FilePath("Subfolder/write-test.abc"));
                SetDestination setDestination = new SetDestination(".txt");

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe("Subfolder/write-test.txt");
            }

            [Test]
            public async Task ExtensionWithoutDotWritesFiles()
            {
                // Given
                TestDocument input = new TestDocument(new FilePath("Subfolder/write-test.abc"));
                SetDestination setDestination = new SetDestination("txt");

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe("Subfolder/write-test.txt");
            }

            [Test]
            public async Task ShouldWriteDotFile()
            {
                // Given
                TestDocument input = new TestDocument();
                SetDestination setDestination = new SetDestination((FilePath)".dotfile");

                // When
                TestDocument result = await ExecuteAsync(input, setDestination).SingleAsync();

                // Then
                result.Destination.ShouldBe(".dotfile");
            }
        }
    }
}