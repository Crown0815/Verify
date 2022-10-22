﻿[UsesVerify]
public class NameForParameterTests
{
    [Fact]
    public Task Null() =>
        Verify(VerifierSettings.GetNameForParameter(new(), null));

    [Fact]
    public Task StringEmpty() =>
        Verify(VerifierSettings.GetNameForParameter(new(), ""));

    [Fact]
    public Task Int() =>
        Verify(VerifierSettings.GetNameForParameter(new(), 10));

#if NET5_0_OR_GREATER
    [Fact]
    public Task Half() =>
        Verify(VerifierSettings.GetNameForParameter(new(), (Half) 10));
#endif

#if NET6_0_OR_GREATER
    [Fact]
    public Task Date() =>
        Verify(VerifierSettings.GetNameForParameter(new(), new DateOnly(2000, 10, 1)));

    [Fact]
    public Task Time() =>
        Verify(VerifierSettings.GetNameForParameter(new(), new DateOnly(2000, 10, 1)));
#endif

    [Fact]
    public Task DateTimeLocal()
    {
        var date = new DateTime(2000, 10, 1, 0, 0, 0, DateTimeKind.Local);
        return Verify(VerifierSettings.GetNameForParameter(new(), date));
    }

    [Fact]
    public Task DateTimeOffsetLocal()
    {
        var date = new DateTimeOffset(2000, 10, 1, 0, 0, 0, DateTimeOffset.Now.Offset);
        return Verify(VerifierSettings.GetNameForParameter(new(), date));
    }

    [Fact]
    public Task DateTimeUnspecified()
    {
        var date = new DateTime(2000, 10, 1, 0, 0, 0);
        return Verify(VerifierSettings.GetNameForParameter(new(), date));
    }

    [Fact]
    public Task DateTimeUtc()
    {
        var date = new DateTime(2000, 10, 1, 0, 0, 0, DateTimeKind.Utc);
        return Verify(VerifierSettings.GetNameForParameter(new(), date));
    }

    [Fact]
    public Task DateTimeOffsetUtc()
    {
        var date = new DateTimeOffset(2000, 10, 1, 0, 0, 0,TimeSpan.Zero);
        return Verify(VerifierSettings.GetNameForParameter(new(), date));
    }

    [Fact]
    public Task List() =>
        Verify(VerifierSettings.GetNameForParameter(
            new(),
            new List<string>
            {
                "value"
            }));

    [Fact]
    public Task ListMultiple() =>
        Verify(VerifierSettings.GetNameForParameter(
            new(),
            new List<string>
            {
                "value1",
                "value2"
            }));

    [Fact]
    public Task Nested() =>
        Verify(VerifierSettings.GetNameForParameter(
            new(),
            new List<object>
            {
                "value1",
                new Dictionary<string, int>
                {
                    {
                        "value2", 10
                    },
                    {
                        "value3", 20
                    }
                },
                "value4"
            }));

    [Fact]
    public Task EmptyList() =>
        Verify(VerifierSettings.GetNameForParameter(new(), new List<string>()));

    [Fact]
    public Task Dictionary() =>
        Verify(VerifierSettings.GetNameForParameter(
            new(),
            new Dictionary<string, int>
            {
                {
                    "value", 10
                }
            }));

    [Fact]
    public Task DictionaryMultiple() =>
        Verify(VerifierSettings.GetNameForParameter(
            new(),
            new Dictionary<string, int>
            {
                {
                    "value1", 10
                },
                {
                    "value2", 11
                }
            }));

    [Fact]
    public Task EmptyDictionary() =>
        Verify(VerifierSettings.GetNameForParameter(new(), new Dictionary<string, int>()));

    [Fact]
    public Task EnumerableStaticEmpty() =>
        Verify(VerifierSettings.GetNameForParameter(new(), Enumerable.Empty<string>()));

    [Fact]
    public Task Array() =>
        Verify(VerifierSettings.GetNameForParameter(
            new(),
            new[]
            {
                "value"
            }));

    [Fact]
    public Task ArrayMultiple() =>
        Verify(VerifierSettings.GetNameForParameter(
            new(),
            new[]
            {
                "value1",
                "value2",
            }));

    [Fact]
    public Task ArrayEmpty() =>
        Verify(VerifierSettings.GetNameForParameter(new(), System.Array.Empty<string>()));
}