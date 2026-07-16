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
p Start reading when you see a - (minus) or 0 or anything between 1-9
  Then expect anything between 1-9 or a period (.)
  Then expect anything between 0-9 or an e or E
  Then expect a +(plus) or - (minus) or anything between 0-9
  After any point if you encounter a EOF, throw exception

