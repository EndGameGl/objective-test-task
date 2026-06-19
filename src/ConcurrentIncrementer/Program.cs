using ConcurrentIncrementer;

Parallel.For(
    fromInclusive: 1,
    toExclusive: 201,
    body: (i) =>
    {
        ServerImplementation.AddToCount(i);
        var value = ServerImplementation.GetCount();
    }
);

Console.WriteLine(ServerImplementation.GetCount());
