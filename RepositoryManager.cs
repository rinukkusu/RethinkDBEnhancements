using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RethinkDb.Driver.Net;

namespace RethinkDBEnhancements
{
	public class RepositoryManager
	{
		public static IDictionary<Type, IRethinkDbRepository> Repositories = new Dictionary<Type, IRethinkDbRepository>();

		public static void Initialize(IConnection connection)
		{
			List<Type> types = Assembly.GetEntryAssembly()
				.GetTypes()
				.Where(t => typeof(IRethinkDbRepository).IsAssignableFrom(t)).ToList();

			foreach (var type in types)
			{
				var constructorInfo = type.GetConstructor(new[] {typeof(IConnection)});
				IRethinkDbRepository repo = (IRethinkDbRepository)constructorInfo.Invoke(new object[] {connection});
				Repositories.Add(type, repo);
			}
		}

		public static T Get<T>() where T : class
		{
			if (!Repositories.ContainsKey(typeof(T)))
				throw new Exception($"No repository initialized for {typeof(T).Name}");

			return Repositories[typeof(T)] as T;
		}
	}
}