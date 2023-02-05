# Contributing

First off, welcome and thanks for taking the time to contribute!

libplctag.NET is a wrapper for libplctag, and as such is intended to expose libplctag functionality in a way that is convenient to use in .NET applications.

## How can I contribute?

### Reporting bugs

We appreciate that you are developing something complex, difficult, important, commercially sensitive, and don't always have a complete understanding of the problem.

However, you are in the best position to describe your situation, so when creating an issue, provide as much context and detail as possible. Help us help you!

#### Guidance when creating an issue

* Review the [documentation](https://github.com/libplctag/libplctag.NET/tree/master/docs), [examples](https://github.com/libplctag/libplctag.NET/tree/master/src/Examples), [previous Github issues](https://github.com/libplctag/libplctag.NET/issues?q=is%3Aissue) and [previous Google Groups discussions](https://groups.google.com/g/libplctag) for similar questions.
* What are you expecting to occur, or what do you need to occur?
* Can you provide evidence of what is actually occuring instead of your expectation?
* Supply a [minimal reproducible example](https://stackoverflow.com/help/minimal-reproducible-example).
* What else have you tried? And why did it not work?
* Supply [libplctag debug logs](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/LoggingExample.cs).
* Are external conditions relevant (e.g. network latency)? Document these. 

As the conversation progresses, you may be asked to try various things.
Keeping track of what is being tested can be very difficult, so please post details of the experiment along with the results - i.e. provide any logs, screenshots, source code, and any other relevant external conditions - regardless of whether they have changed or not.

#### General debugging guidance
* The [libplctag core releases](https://github.com/libplctag/libplctag/releases) contain a [number of tools](https://github.com/libplctag/libplctag/blob/release/src/examples/README.txt) useful during debugging (tag_rw and list_tags in particular).
* Read the [libplctag wiki](https://github.com/libplctag/libplctag/wiki).
* Use a simulator or a PLC Fake to eliminate calls to libplctag and/or the hardware.
  * [ab_server](https://github.com/libplctag/libplctag/releases/)
  * [Modbus server docker image](https://hub.docker.com/r/oitc/modbus-server)
  * Develop your own [PLC Fake](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/ExampleSimulator.cs)
* Enable and read the [libplctag debug logs](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/LoggingExample.cs).


#### Pull Requests
We recognize that libplctag.NET is primarily used in commercial contexts, where copyright and legal concerns carry significant weight.
To simplify adoption, we require that all contributions are licensed using the same license (see [LICENSE](https://github.com/libplctag/libplctag.NET/blob/master/LICENSE), you will become one of the collective "authors of libplctag.NET".
