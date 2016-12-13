using System;
using System.Linq;
using System.Reflection;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;
using RethinkDBEnhancements.Attributes;

namespace RethinkDBEnhancements
{
	public interface IRethinkDbRepository
	{
		Table Table();
	}

	public class RethinkDbRepository<TDoc> : IRethinkDbRepository
		where TDoc : Document
	{
		private readonly IConnection _connection;

		public RethinkDbRepository(IConnection connection)
		{
			_connection = connection;
		}

		public Table Table()
		{
			var attribute = GetType().GetTypeInfo().GetCustomAttribute<RepositoryAttribute>();
			if (attribute == null)
				throw new Exception($"RepositoryAttribute incomplete or missing on Type {GetType().Name}");

			return attribute.Database != null 
				? RethinkDB.R.Db(attribute.Database).Table(attribute.Table) 
				: RethinkDB.R.Table(attribute.Table);
		}

		public TDoc Get(Guid id)
		{
			return Table().Get(id).RunAtom<TDoc>(_connection);
		}

		public TDoc Get(string id)
		{
			return Get(Guid.Parse(id));
		}

		public TDoc Insert(TDoc document)
		{
			var result = Table().Insert(document).RunResult(_connection);

			if (result.GeneratedKeys.Any())
				document.Id = result.GeneratedKeys[0];

			return document;
		}

		public TDoc Update(TDoc document)
		{
			var result = Table().Update(document).RunResult(_connection);

			if (result.GeneratedKeys != null)
				document.Id = result.GeneratedKeys[0];

			return document;
		}
	}
}