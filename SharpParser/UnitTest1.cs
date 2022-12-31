using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;
namespace SharpParser;

record Token;

record Keyword(string Literal) : Token;
record Identifier(string Literal) : Token;

record Punctuation(string Literal) : Token;

record ParseError(string Reason);

class Capturer
{
    private readonly Func<string, Token> tokenGenerator;

    public Capturer(Func<string, Token> tokenGenerator)
    {
        this.tokenGenerator = tokenGenerator;
    }

    public Token GetToken(string raw) => tokenGenerator(raw);
}

class Parser
{

    private readonly IImmutableList<Token>? tokens;
    private readonly ParseError? parseError;

    private Parser(IEnumerable<Token> tokens, ParseError? parseError)
    {
        this.tokens = tokens.ToImmutableList();
        this.parseError = parseError;
    }

    public IReadOnlyList<Token> Tokens => tokens?? Enumerable.Empty<Token>().ToImmutableList();

    public static Parser Parse(string code)
    {
        
    }
}


public class TestParser
{
    [Test]
    public void TestParsingProgram()
    {
        var code = @"
Program my_game
Begin
    foo();
End

Process foo()
Begin
    Loop
        Frame;
    End
End
";

        var tokens = new Parser.Parse(code).Tokens;
        var expected = new Token[] {
            new Keyword("Program"),
            new Identifier("my_game"),
            new Punctuation("("),
            new Keyword("Begin"),
            new Identifier("foo"),
            new Punctuation(")"),
            new Keyword("End"),
            new Keyword("Process"),
            new Identifier("foo"),
            new Keyword("Begin"),
        };

        Assert.That(tokens, Is.EqualTo(expected));
    }
}