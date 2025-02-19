﻿using System;
using NUnit.Framework;

namespace Azure.Messaging.EventHubs.Tests
{
    /// <summary>
    ///   The suite of tests for the <see cref="SenderOptions" />
    ///   class.
    /// </summary>
    ///
    [TestFixture]
    [Category(TestCategory.BuildVerification)]
    public class SenderOptionsTests
    {
        /// <summary>
        ///   Verifies functionality of the <see cref="SenderOptions.Clone" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public void CloneProducesACopy()
        {
            var options = new SenderOptions
            {
                PartitionId = "some_partition_id_123",
                Retry = new ExponentialRetry(TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(5), 6),
                Timeout = TimeSpan.FromMinutes(65)
            };

            var clone = options.Clone();
            Assert.That(clone, Is.Not.Null, "The clone should not be null.");

            Assert.That(clone.PartitionId, Is.EqualTo(options.PartitionId), "The partition identifier of the clone should match.");
            Assert.That(clone.Timeout, Is.EqualTo(options.Timeout), "The  timeout of the clone should match.");

            Assert.That(clone.Retry, Is.EqualTo(options.Retry), "The retry of the clone should be considered equal.");
            Assert.That(clone.Retry, Is.Not.SameAs(options.Retry), "The retry of the clone should be a copy, not the same instance.");
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="SenderOptions.Timeout" />
        ///   property.
        /// </summary>
        ///
        [Test]
        public void DefaultTimeoutIsValidated()
        {
            Assert.That(() => new SenderOptions { Timeout = TimeSpan.FromMilliseconds(-1) }, Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="SenderOptions.Timeout" />
        ///   property.
        /// </summary>
        ///
        [Test]
        [TestCase(null)]
        [TestCase(0)]
        public void DefaultTimeoutUsesDefaultValueIfNormalizesValueINotSpecified(int? noTimeoutValue)
        {
            var options = new SenderOptions();
            var timeoutValue = (noTimeoutValue.HasValue) ? TimeSpan.Zero : (TimeSpan?)null;

            options.Timeout = timeoutValue;
            Assert.That(options.Timeout, Is.EqualTo(timeoutValue), "The value supplied by the caller should be preserved.");
            Assert.That(options.TimeoutOrDefault, Is.Null, "The timeout value should be normalized to null internally.");
        }
    }
}
