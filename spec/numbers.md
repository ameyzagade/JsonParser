p The JSON number supports following grammar:

number := integer fraction? exponent?
integer := (-)? (0 | [1-9][0-9]\*)
fraction := . [0-9]+
exponent := (e | E) (+ | -)? [0-9]+

h2 Valid numbers:
h4 0, 12, -12, 12.3, -12.3, 10e5, -10e5, -10e+5, 10e-5, -10e-5, 10.1e5, -12.1e1

h2 Invalid numbers:
h4 +0, 01, +23, 3., .3, 2.3.3, f, f5, e5, E-10, 3f10 3.f-5, 3e-10, 3.1e-, 3,1e-f

h3 Lexical analysis and rules:
p

- Start reading a number when you see a - (minus) sign or 0 or anything between 1-9

- Start with lexical analysis for an integer:
  - Expect zero or one minus sign
  - Expect either 0 or anything between 1-9, followed by 0-9, zero or more times
  - If input ends after minus sign, then return error

- Start with lexical analysis for an fraction:
  - Since it is optional, it is okay to skip reading it if input ends or no . (period) sign is encountered
  - If a period is encountered, expect anything between 0-9, one or more time
  - If input ends after period sign, then return error

- Start with lexical analysis for an fraction:
  - Since it is optional, it is okay to skip reading it if input ends or no e or E (exponent sign) is encountered
  - If an exponent sign is encountered, expect either + (plus) or - (minus) sign
  - Expect + or - sign to be followed by anything between 0-9, one or more time
  - If input ends after exponent sign, plus or minus sign, then return error
  - If at any point, end of input is encountered after the e or E sign, + or - sign, then return error

- If at any point, expected value is not encountered, stop reading further
