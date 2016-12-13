# RethinkDBEnhancements

## Subclassing
```cs
// The repository subclass
[Repository(Table = "user")]
public class UserRepository : RethinkDbRepository<User>
{
		public UserRepository(IConnection connection) 
			: base(connection)
		{
		}
}

// The document subclass
public class User : Document
{
    public string Username { get; set; }

    public User(string username)
    {
      Username = username;
    }
}
```

## Usage
```cs
// Set to camelCase, if you want JS style lowercase keys 
RethinkDb.Driver.Net.Converter.Serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

// Set up connection
var connection = R.Connection()
    .Hostname("localhost")
    .User("admin", "")
    .Db("test")
    .Connect();

if (connection.Open) 
{
    RepositoryManager.InitializeRepositories(connection);
    var userRepository = RepositoryManager.GetRepository<UserRepository>();

    User user = userRepository.Insert(new User("johndoe"));
    user.Username = "ehehe";
    user = userRepository.Update(user);

    var theUser = userRepository.Get(user.Id);
}
```
