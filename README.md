# RethinkDBEnhancements [![Build Status](https://travis-ci.org/rinukkusu/RethinkDBEnhancements.svg?branch=master)](https://travis-ci.org/rinukkusu/RethinkDBEnhancements)

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
    // Call Initialize on the RepositoryManager 
    // to instantiate all the repositories in the assembly
    RepositoryManager.Initialize(connection);

    // Get a custom repository as defined above
    var userRepository = RepositoryManager.Get<UserRepository>();

    // Insert a new document
    User user = userRepository.Insert(new User("johndoe"));

    // Change something in the document and update it
    user.Username = "ehehe";
    user = userRepository.Update(user);

    // Get a document by its ID
    var theUser = userRepository.Get(user.Id);
}
```
