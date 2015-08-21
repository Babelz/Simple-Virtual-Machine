# List of all opcodes

push
------

Pushes a value to the stack.

push [word][value]
- Pushes a specific sized value to the stack. 

push8	[value] 
- Pushes a 8-bit value to the stack.

push16 [value]
- Pushes a 16-bit value to the stack.

push32 [value]
- Pushes a 32-bit value to the stack.

pushreg [register_name]
- Pushes a given register value to the stack. Register will not be cleared.

pop
------

Removes a value from top of the stack.

pop	[count]
- Remove given amount of bytes from the stack.

pop [register_name]
- Remove given amount of bytes contained inside a register from the stack.

pop8
- Remove one byte from the stack.

pop16
- Remove two bytes from the stack.

pop32
- Remove four bytes from the stack

top
------

Copy given amount of bytes from the top of the stack to given register
or to given heap address.

top [bytescount] [register]
- Copy given amount of bytes to given register.

sp
------

Store stack pointers location to given register.

sp [register]

pushb
------

Push given amount of bytes to the stack.

pushb [element size] [elements]

ldstr
------

Loads given string to the stack.

ldstr "[string]"

ldch
------

Loads given character to the stack

ldch '[character]'
