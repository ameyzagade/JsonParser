using System.Collections;
using JsonParser.App.Lexer;

namespace JsonParser.Tests.Lexer.TestData;

public class ValidJsonLexerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonLexerTestData("", [])
        ];

        yield return [
            new JsonLexerTestData("""{}""", [
                new Token(TokenType.LeftBrace, "{"),
                new Token(TokenType.RightBrace, "}")
            ])
        ];

        yield return [
            new JsonLexerTestData("""{}""", [
                new Token(TokenType.LeftBrace, "{"),
                new Token(TokenType.RightBrace, "}")
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "path": "C:\\Users\\John" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "path"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "C:\\Users\\John"),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "message": "hello\nworld" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "message"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "hellonworld"),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
            new JsonLexerTestData("""{ "Name": "John Doe" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
           new JsonLexerTestData("""{ "Name": "" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, ""),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
           new JsonLexerTestData("""{ "Name": "John \"Doe\"" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John \"Doe\""),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
           new JsonLexerTestData("""{"key": true     }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "key"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "true"),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
            new JsonLexerTestData("""{"key":true}""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "key"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "true"),

                new Token(TokenType.RightBrace, "}"),
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
                new Token(TokenType.PrimitiveValue, "true"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key2"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key3"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "null"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key4"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "value"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key5"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "101"),

                new Token(TokenType.RightBrace, "}"),
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

                new Token(TokenType.RightBrace, "}")
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
                new Token(TokenType.PrimitiveValue, "30"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Citizenship"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "OtherDetails"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "null"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}")
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
                new Token(TokenType.PrimitiveValue, "30"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Citizenship"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "false"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "OtherDetails"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "null"),

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
                new Token(TokenType.PrimitiveValue, "true"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}")
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
                new Token(TokenType.PrimitiveValue, "30"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "Citizenship"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "false"),
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
                new Token(TokenType.PrimitiveValue, "false"),
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
                new Token(TokenType.PrimitiveValue, "true"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}")
            ])
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}