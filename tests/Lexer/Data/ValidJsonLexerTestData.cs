using System.Collections;
using JsonParser.App.Lexer;

namespace JsonParser.Tests.Lexer.TestData;

public class ValidJsonLexerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonLexerTestData("", [
                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{}""", [
                new Token(TokenType.LeftBrace, "{"),
                new Token(TokenType.RightBrace, "}"),
                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""[]""", [
                new Token(TokenType.LeftBracket, "["),
                new Token(TokenType.RightBracket, "]"),
                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "Name": "John Doe" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "Age": 30 }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Age"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "30"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "Salaried": true }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Salaried"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "true"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "Details": null }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Details"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "null"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "path": "C:\\Users\\John" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "path"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "C:\\Users\\John"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "message": "hello\nworld" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "message"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "hellonworld"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
           new JsonLexerTestData("""{ "Name": "" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, ""),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
           new JsonLexerTestData("""{ "Name": "John \"Doe\"" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John \"Doe\""),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{"key": true     }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "key"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "true"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""{"key":true}""", [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "key"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.Identifier, "true"),

                    new Token(TokenType.RightBrace, "}"),

                    new Token(TokenType.EndOfStream, string.Empty)
                ])
        ];

        yield return [
           new JsonLexerTestData("""
           {
            "key": "value",
            "key2": "value"
           }
           """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "key"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "value"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key2"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "value"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
           ])
        ];

        yield return [
            new JsonLexerTestData("""
            {
                "key1": true,
                "key2": false,
                "key3": null,
                "key4": "value",
                "key5": 101
            }
            """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "key1"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "true"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key2"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key3"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "null"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key4"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "value"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key5"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "101"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData(""" 
                { 
                    "Person": { 
                        "Name": "John Doe"
                    } 
                } 
            """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Person"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData(""" 
                { 
                    "Person": { 
                        "Name": "John Doe",
                        "Age": 30,
                        "Citizenship": false,
                        "OtherDetails": null
                    } 
                } 
            """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Person"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Age"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "30"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Citizenship"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "OtherDetails"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "null"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData(""" 
                { 
                    "PersonalDetails": { 
                        "Name": "John Doe", 
                        "Age": 30,
                        "Citizenship": false,
                        "OtherDetails": null
                    }, 
                    "BankDetails": { 
                        "AccountNumber": "100-23435454", 
                        "Bank": "New Bank", 
                        "Active": true 
                    } 
                } 
                """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "PersonalDetails"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Age"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "30"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Citizenship"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "OtherDetails"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "null"),

                new Token(TokenType.RightBrace, "}"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "BankDetails"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "AccountNumber"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "100-23435454"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Bank"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "New Bank"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Active"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "true"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData(""" 
                { 
                    "PersonalDetails": { 
                        "Name": "John Doe", 
                        "Age": 30,
                        "Citizenship": false,
                        "Contact Number": "+1 123232",
                        "OtherDetails": {
                            "Blood Group": "O+",
                            "Visa Expired": false,
                            "Emergency Contact Number": "+1 234355"
                        }
                    }, 
                    "BankDetails": { 
                        "AccountNumber": "100-23435454", 
                        "Bank": "New Bank", 
                        "Active": true
                    } 
                } 
                """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "PersonalDetails"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Age"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "30"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Citizenship"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Contact Number"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "+1 123232"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "OtherDetails"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Blood Group"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "O+"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Visa Expired"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Emergency Contact Number"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "+1 234355"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "BankDetails"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "AccountNumber"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "100-23435454"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Bank"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "New Bank"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Active"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "true"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""
                [ 1, 2, 3]
            """, [
                new Token(TokenType.LeftBracket, "["),

                new Token(TokenType.Number, "1"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.Number, "2"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.Number, "3"),

                new Token(TokenType.RightBracket, "]"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""
                [
                    {
                        "Name": "John Doe",
                        "Age": 30,
                        "Other Details": null,
                        "Salaried": true
                    },
                    {
                        "Name": "Jane Doe",
                        "Age": 29,
                        "Other Details": {
                            "Nationality": "American",
                            "Visa Expired": false,
                            "Rank": 12
                        },
                        "Salaried": true
                    }
                ]
            """, [
                new Token(TokenType.LeftBracket, "["),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Age"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "30"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Other Details"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "null"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Salaried"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "true"),

                new Token(TokenType.RightBrace, "}"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "Jane Doe"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Age"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "29"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Other Details"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Nationality"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "American"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Visa Expired"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Rank"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Number, "12"),

                new Token(TokenType.RightBrace, "}"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Salaried"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Identifier, "true"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBracket, "]"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];

        yield return [
            new JsonLexerTestData("""
            {
                "Fruits": ["Apple", "Banana"]
            }
            """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Fruits"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBracket, "["),
                new Token(TokenType.String, "Apple"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.String, "Banana"),
                new Token(TokenType.RightBracket, "]"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty),
            ])
        ];

        yield return [
            new JsonLexerTestData("""
            {
                "Numbers": [1, 2, 3]
            }
            """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Numbers"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBracket, "["),

                new Token(TokenType.Number, "1"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.Number, "2"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.Number, "3"),

                new Token(TokenType.RightBracket, "]"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.EndOfStream, string.Empty)
            ])
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}