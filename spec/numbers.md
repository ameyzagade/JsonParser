p The JSON number supports following grammar:

number := integer fraction? exponent?
integer := (-)? (0 | [1-9][0-9]*)
fraction := . [0-9]+
exponent := (e | E) (+ | -)? [0-9]+

h2 Valid numbers:
h4 0, 12, -12, 12.3, -12.3, 10e5, -10e5, -10e+5, 10e-5, -10e-5, 10.1e5, -12.1e1

h2 Invalid numbers:
h4 +0, -0, 01, +23, '22,', 3., .3, 2.3.3, f, f5, e5, E-10, 3f10 3.f-5, 3e-10, 3.1e-, 3,1e-f


h3 Lexical analysis:
p 
  - Start reading a number when you see a - (minus) or 0 or anything between 1-9
  - After e or E, expect + or -
  - After -, expect anything between 0-9
  - After +, expect anything between 0-9
  - After 0, expect a . or end of input
  - After 1-9, expect anything between 0-9, zero or more times
  - After ., expect anything between 0-9, one or more times
  - If at any point, end of input is encountered, then return error
  - If at any point, expected value is not encountered, stop reading further