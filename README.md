# scalesque
## A Scala inspired functional programming library written in c&#35;

Scalesque allows you to write c&#35; that is similar to the code you would write in Scala.  Scalesque is currently approaching v1, it's pretty stable but some api calls may still change.  

### Features list

* Option&lt;T&gt; (aka Maybe&lt;T&gt;)
* Either&lt;T,U&gt;
* Pattern matching and extraction
* Map / Fold / Reduce (via IEnumerable&lt;T&gt;)
* Partial function application and currying
* Scalaz inspired validations
* Exception -> Option wrapper

## Dependencies

.net 3.5 or 4.0

## License

scalesque is licensed under the MIT license, see license.txt for details.

## Nuget package

Usually in synch with head of master

## Roadmap
* I'm working on the [Documentation](http://noelkennedy.github.com/scalesque)
* Extractors for .net framework patterns like Int.TryParse which are a bit horrible from a functional programming perspective
* Not happy with current Head and tail construct

