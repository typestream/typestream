# TypeStream 

The goal of this library is to provide easy way to send and recieve messages between different application in realtime.

For example, send message from Unity to web server or an other instance of Unity.

# Installation

You can install it with nuget:

    Install-Package TypeStream
    Install-Package TypeStream.Json // for JSON support
    Install-Package TypeStream.MessagePack // for MessagePack support

# Examples

Client example

    var stream = new TypeStream.TypeStream(networkStream, networkStream, new JsonFormatter(), new ByNameIdResolver());

    // Adding allowed types
    stream.Register<User>();

    var user = new User { Id = Guid.NewGuid() };
    stream.Write(user);

Server example

    var stream = new TypeStream.TypeStream(networkStream, networkStream, new JsonFormatter(), new ByNameIdResolver());
    
    // Adding allowed types
    stream.Register<User>();

    var newUser = stream.Read<User>();

For MessagePack you have to replace **JsonFormatter** with **MessagePackFormatter**
