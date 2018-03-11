# RedisNetCore11Example
A Redis client in .NET 1.1 Core

A .net core 1.1 example of a Redis client.

This project was thrown together very quick as a spike to create a Redis client in .NET Core 1.1. Ultimately, this was to explore aws ElasticCache using the Redis engine.

Please forgive me as I was in a hurry to get a demo up and running. Very little comments. The program will compile and run. See the HomeController and import the redisdemopostman.json as it calls methods in both of those controllers to perform crud operations in the Redis cache.  The .NET 2.0 implementation has a Widget controller.  I did not have time to create a Widget controller for this but it would be easy enough to do.  So don't run the postman requests where you see api/Widget for this  :-)

You will also need to go to https://redis.io/download and download / startup the redis cache. I just installed the zip and started up redis-server.

The ability to store complex objects was mandatory.

I did not see where the free StackExchange.Redis had a .NET Core 1.1 port.  It's a .NET Core 2.0 lib (and a very nice one at that).
I tried quickly to compile that against .NET Core 1.1 but
ran into some errors with unknown namespaces (ie, the Binary formatters, etc..). In the interest of trying to get a demo up for someone,
I did not attempt to port that back down to .NET Core 1.1.

So instead I chose this route for .NET 1.1 to get something up there quick.

https://dotnetcoretutorials.com/2017/01/06/using-redis-cache-net-core/
and used the Microsoft api for it
https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.redis.rediscache?view=aspnetcore-1.1

You can import the following file into Postman which will give you a collection of the various ways you can hit the API redisdemopostman.json

Youtube for this showing how it runs.

https://youtu.be/PQBytm8o7zU
