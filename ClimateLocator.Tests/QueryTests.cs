using ClimateLocator.Core.Models;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace ClimateLocator.Tests
{
    public class QueryTests
    {
        [Test]
        public void Query_Properties_ShouldBeSetCorrectly()
        {
            var ip = "192.168.0.1";
            var now = DateTime.UtcNow;

            var query = new Query { Ip = ip, QuerriedAt = now };

            query.Ip.Should().Be(ip);
            query.QuerriedAt.Should().Be(now);
        }
    }
}