﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using EMG.Extensions.Logging.Loggly;
using Moq;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Loggly
{
    [TestFixture]
    public class LogglyHttpClientTests
    {
        [Test, AutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(LogglyHttpClient).GetConstructors());
        }

        [Test, AutoMoqData]
        public async Task Messages_are_published_to_given_host([Frozen] LogglyOptions options, IFixture fixture, LogglyMessage message)
        {
            var registration = new HttpMessageOptions
            {
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                HttpMethod = HttpMethod.Post
            };

            var handler = new FakeHttpMessageHandler(registration);

            fixture.Register((LogglyOptions o) => new HttpClient(handler)
            {
                BaseAddress = new Uri($"{o.LogglyScheme}://{o.LogglyHost}/inputs/{o.ApiKey}/tag/{o.Environment}")
            });

            var sut = fixture.Create<LogglyHttpClient>();

            await sut.PublishAsync(message);

            Assert.That(registration.HttpResponseMessage.RequestMessage.RequestUri.Host, Contains.Substring(options.LogglyHost));
        }

        [Test, AutoMoqData]
        public async Task Messages_are_published_to_given_scheme([Frozen] LogglyOptions options, IFixture fixture, LogglyMessage message)
        {
            var registration = new HttpMessageOptions
            {
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                HttpMethod = HttpMethod.Post
            };

            var handler = new FakeHttpMessageHandler(registration);

            fixture.Register((LogglyOptions o) => new HttpClient(handler)
            {
                BaseAddress = new Uri($"{o.LogglyScheme}://{o.LogglyHost}/inputs/{o.ApiKey}/tag/{o.Environment}")
            });

            var sut = fixture.Create<LogglyHttpClient>();

            await sut.PublishAsync(message);

            Assert.That(registration.HttpResponseMessage.RequestMessage.RequestUri.Scheme, Contains.Substring(options.LogglyScheme));
        }

        [Test, AutoMoqData]
        public async Task Messages_are_published_to_url_with_ApiKey([Frozen] LogglyOptions options, IFixture fixture, LogglyMessage message)
        {
            var registration = new HttpMessageOptions
            {
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                HttpMethod = HttpMethod.Post
            };

            var handler = new FakeHttpMessageHandler(registration);

            fixture.Register((LogglyOptions o) => new HttpClient(handler)
            {
                BaseAddress = new Uri($"{o.LogglyScheme}://{o.LogglyHost}/inputs/{o.ApiKey}/tag/{o.Environment}")
            });

            var sut = fixture.Create<LogglyHttpClient>();

            await sut.PublishAsync(message);

            Assert.That(registration.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring(options.ApiKey));
        }

        [Test, AutoMoqData]
        public async Task Messages_are_published_to_url_with_Environment([Frozen] LogglyOptions options, IFixture fixture, LogglyMessage message)
        {
            var registration = new HttpMessageOptions
            {
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                HttpMethod = HttpMethod.Post
            };

            var handler = new FakeHttpMessageHandler(registration);

            fixture.Register((LogglyOptions o) => new HttpClient(handler)
            {
                BaseAddress = new Uri($"{o.LogglyScheme}://{o.LogglyHost}/inputs/{o.ApiKey}/tag/{o.Environment}")
            });

            var sut = fixture.Create<LogglyHttpClient>();

            await sut.PublishAsync(message);

            Assert.That(registration.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring(options.Environment));
        }
    }
}
