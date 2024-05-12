# DotnetHoneyApi
Hello! Welcome to the DotnetHoneyApi project. This is a side project that emulates a Dotnet WebApi to allow defenders to create threat intelligence. 

Some key things to note about the project:
- I do not make guarantees that this application is "secure"
- There is no data access layer
- If you send secrets to this API, it will be copied out to a log in clear-text
- This project is meant to be used in combination with a SIEM or other detection platform
	- Example: Elasticsearch or SecurityOnion

# Facts about the project
- Requires dotnet 8.0
- There is a /healthz endpoint that is exposed to allow for deployment in cloud providers via a container
	- Highly recommend putting this honeypot behind an application loadbalancer of some kind.
# How are requests logged?

All requests are logged using NLog and formatted in JSON compliant with RFC 8259. The output value is formatted in JSON and includes the following elements:
- Request time => DateTime
- Request host => string
- Request method => string
- Request path => string
- Request headers => Dictionary[]
- Request body => string*

\* The body may be serialized differently depending on what is sent to the honeypot.

# Contributing
If you want to contribute to this open-source project, by all means please do! I will to consider any ideas that people would like to include in this repo. 

I don't really have a strict "vision" of what the project will be, though I know I want it to be relatively simple. 