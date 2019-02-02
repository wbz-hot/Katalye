﻿using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Katalye.Components.Configuration.Providers
{
    [UsedImplicitly]
    public class BuiltInProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public int Priority => 100;

        public string Name => "Environment";

        public BuiltInProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (bool Success, string Value) TryGet(string path)
        {
            var value = _configuration.GetValue(path, "");
            return (!string.IsNullOrWhiteSpace(value), value);
        }
    }
}