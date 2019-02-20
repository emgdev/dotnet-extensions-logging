﻿using System;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using EMG.Extensions.Logging.Loggly;
using Moq;
using NUnit.Framework;

namespace Tests.Loggly
{
    [TestFixture]
    public class LogglyProcessorTests
    {
        [Test, AutoMoqData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(LogglyProcessor).GetConstructors());
        }

        [Test, AutoMoqData]
        public async Task Message_is_published_after_adding_to_queue([Frozen] ILogglyClient client, LogglyProcessor sut, LogglyMessage message)
        {
            sut.EnqueueMessage(message);

            await Task.Delay(TimeSpan.FromMilliseconds(10));

            Mock.Get(client).Verify(p => p.PublishAsync(message));
        }

        [Test, AutoMoqData]
        public void Message_is_published_after_adding_to_queue_when_disposed([Frozen] ILogglyClient client, LogglyProcessor sut, LogglyMessage message)
        {
            sut.Dispose();

            sut.EnqueueMessage(message);

            Mock.Get(client).Verify(p => p.PublishAsync(message));
        }
    }
}