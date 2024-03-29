﻿using BufTools.AspNet.EndpointReflection;
using BufTools.AspNet.TestFramework;
using BufTools.ObjectCreation.FromXmlComments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Template.ArchitectureTests
{
    [TestClass]
    public class ArchitectureTests
    {
        private readonly Browser<Program> _browser;
        private readonly ObjectMother _mother;

        public ArchitectureTests()
        {
            _mother = new ObjectMother();
            _mother.IgnorePropertiesWithAttribute<JsonIgnoreAttribute>();

            _browser = new Browser<Program>();
        }

        [TestMethod]
        public async Task AllEndpoints_WithXmlExampleValues_Returns200Ok()
        {
            var endpoints = typeof(Program).Assembly.GetEndpoints();
            var tasks = endpoints.Select(e => CallEndpointAsync(e));
            var results = await Task.WhenAll(tasks);

            Assert.IsFalse(results.Any(r => r.Value != HttpStatusCode.OK));
        }

        private async Task<KeyValuePair<string, HttpStatusCode>> CallEndpointAsync(HttpEndpoint endpoint)
        {               
            var payload = (endpoint.BodyPayloadType != null ) ? _mother.Birth(endpoint.BodyPayloadType) : null;

            var request = _browser.CreateRequest(endpoint.ExampleRoute);
            if (payload != null)
                request = request.WithJsonContent(JsonSerializer.Serialize(payload));

            HttpResponseMessage response = null;
            switch (endpoint.Verb)
            {
                case HttpEndpoint.Verbs.Get:
                    response = await request.GetAsync();
                    break;
                case HttpEndpoint.Verbs.Post:
                    response = await request.PostAsync();
                    break;
                case HttpEndpoint.Verbs.Put:
                    response = await request.PutAsync();
                    break;
                case HttpEndpoint.Verbs.Delete:
                    response = await request.DeleteAsync();
                    break;
            }

            return new KeyValuePair<string, HttpStatusCode>(endpoint.ExampleRoute, response.StatusCode);
        }
    }
}