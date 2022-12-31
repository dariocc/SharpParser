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
    private readonly Func<string, Token?> tokenMatcher;

    public Capturer(Func<string, Token?> tokenMatcher)
    {
        this.tokenMatcher = tokenMatcher;
    }

    public Token? GetToken(string raw) => tokenMatcher(raw);

    public Capturer CaseSensitiveWord(string word, Func<string, Token> tokenFactory) => new Capturer(raw => string.Equals(raw, word, StringComparison.InvariantCulture) ? tokenFactory(word) : null);
    public Capturer CaseInsensitiveWord(string word, Func<string, Token> tokenFactory) => new Capturer(raw => string.Equals(raw, word, StringComparison.InvariantCultureIgnoreCase) ? tokenFactory(word) : null);
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
        var capturers = new Capturer[] {
            new Capturer(raw => raw == "Program" ? new Keyword("Program") : null),
        };
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