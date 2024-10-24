# Dear Delta Lake

![](logo.svg)

This is an ongoing attempt to implement [delta.io](https://delta.io/) in pure .net.

## Why implement the Delta Lake transaction log protocol in .NET?

Delta Spark depends on Java and Spark, which is fine for many use cases, but not all Delta Lake users want to depend on these libraries. Delta.Net allows using Delta Lake in C# or other languages using .NET runtime.

Delta.Net lets you query Delta tables without depending on Java/Scala.

Suppose you want to query a Delta table with on your local machine. Delta.Net makes it easy to query the table with a simple `dotnet add package` command - no need to install Java or Spark.

## Status

Very early draft. Nothing is working yet.


## Contributing

Bookmark, star, start discussions if you are interested in the future of this project!

## Useful Links

- [Delta Transaction Log Protocol](https://github.com/delta-io/delta/blob/master/PROTOCOL.md).
- [Integration Test Dataset](https://github.com/delta-io/delta-rs/tree/main/crates/test/tests/data).
