
# Moonet

A semi-static typed superset of the programming language, Lua. Implemented in dotnet and use dotnet as its runtime.

## Type

Typed local variables can be declared as

```
local v : typename = initVal
```

In this case, we declared a local variable `v` of type `typename` with initial value `initVal`.

A type can be either a basic type or a defined class. Basic types include `integer`, `float`, `string` and `table`.

## Object Orientation Based on Classes

While still have a full feature on prototype-based object orientation, we introduce the class-based system in.

A class can be defined as

```
class A : Interface1, Base, Interface2
	field : string
	initialised : integer = initConstExpr
	function .method()
	end
	function :static_member()
	end
	function .new(f : string)
		self.field = f
	end
end
```

The order of base class and interfaces is not restricted, but only one class allowed among them.

If no base class specified, `table` is the default base class.

`.new` stands for constructor of this class.

## Using Mechanism

A file starts with an optional using phase. A class not be able to get inferred from this using phase will be treated with runtime binding, which is slower.

There are three forms of a using statement.

```
using 'file1.lua' as file1Res
using 'file2.lua'
using namespace System.Collections.Generic
```
