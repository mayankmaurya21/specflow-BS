using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace SpecflowBrowserStack.Drivers
{
	public class ConfigurationDriver
	{
		private const string SeleniumBaseUrlConfigFieldName = "host";
		private readonly Lazy<IConfiguration> _configurationLazy;

		public ConfigurationDriver()
		{
			_configurationLazy = new Lazy<IConfiguration>(GetConfiguration);
		}

		public IConfiguration Configuration => _configurationLazy.Value;
		public string SeleniumBaseUrl => Configuration[SeleniumBaseUrlConfigFieldName];
		public string Username => Configuration["username"];
		public string AccessKey => Configuration["access_key"];

		public IEnumerable<IConfigurationSection> CommonCapabilities => Configuration.GetSection("commonCapabilities").GetChildren();
		public IEnumerable<IConfigurationSection> Single => Configuration.GetSection("single").GetChildren();
		public IEnumerable<IConfigurationSection> Local => Configuration.GetSection("local").GetChildren();
		public IEnumerable<IConfigurationSection> Parallel => Configuration.GetSection("parallel").GetChildren();
		public IEnumerable<IConfigurationSection> Mobile => Configuration.GetSection("mobile").GetChildren();
		private IConfiguration GetConfiguration()
		{
			var configurationBuilder = new ConfigurationBuilder();
			string directoryName = Path.GetDirectoryName(typeof(ConfigurationDriver).Assembly.Location);
		/*	if(System.Environment.GetEnvironmentVariable("TEST_INFRA").Equals("DOCKER"))
            {
				configurationBuilder.AddJsonFile(Path.Combine(directoryName, @"docker_conf.json"));
            }
            else { */
				configurationBuilder.AddJsonFile(Path.Combine(directoryName, @"conf.json"));
		// 	}
			return configurationBuilder.Build();
		}
	}
}