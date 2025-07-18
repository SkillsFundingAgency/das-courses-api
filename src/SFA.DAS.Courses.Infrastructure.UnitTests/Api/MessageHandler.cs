﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    public static class MessageHandler
    {
        public static Mock<HttpMessageHandler> SetupGetMessageHandlerMock(HttpResponseMessage response, Uri uri)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            return httpMessageHandler.AddHandler(response, uri);
        }

        public static Mock<HttpMessageHandler> AddHandler(this Mock<HttpMessageHandler> httpMessageHandler, HttpResponseMessage response, Uri uri)
        {
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.Equals(uri)
                        ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }

        public static Mock<HttpMessageHandler> SetupPostMessageHandlerMock(HttpResponseMessage response, Uri uri)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.Equals(uri)
                        ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }
    }
}
