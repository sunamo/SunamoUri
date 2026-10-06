// variables names: ok


public class UHTests
{
    const string uri = @"https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon";
    static readonly Uri urio = new Uri(uri);

    [Fact]
    public void IsValidUriAndDomainIsTest()
    {
        var input = "naradi-pajky.cz";
        var expected = true;

        bool surelyDomain;
        var actual = UH.IsValidUriAndDomainIs(input, "*", out surelyDomain);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetUriSafeStringTest()
    {
        var input = "AttributeValueOfHtmlUC";
        var expected = "attribute-value-of-html-uc";

        var actual = UH.GetUriSafeString(input);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetHostTest()
    {
        var actual = UH.GetHost(uri);//DebugLogger.Instance.WriteLine("GetHost" + ": " + actual);

    }

    [Fact]
    public void UrlEncodeTest()
    {
        /*
Pokud správně užívám UrlEncode/Decode ani plus ve vstupním řetězci nemůže způsobit žádné škody

         */

        var input = @"a%+ b";
        var encoded = UH.UrlEncode(input);
        var decoded = UH.UrlDecode(encoded);
        Assert.Equal(input, decoded);
    }

    [Fact]
    public void GetDirectoryNameTest()
    {
        var actual = UH.GetDirectoryName(uri);//DebugLogger.Instance.WriteLine("GetDirectoryNameTest" + ": " + actual);
    }

    [Fact]
    public void GetFileNameWithoutExtensionTest()
    {
        var actual = UH.GetFileNameWithoutExtension(uri);//DebugLogger.Instance.WriteLine("GetFileNameWithoutExtension" + ": " + actual);
    }

    [Fact]
    public void GetExtensionTest()
    {
        var actual = UH.GetExtension(uri);//DebugLogger.Instance.WriteLine("GetExtension" + ": " + actual);
    }

    [Fact]
    public void GetQueryAsHttpRequestTest()
    {
        var actual = UH.GetQueryAsHttpRequest(urio);//DebugLogger.Instance.WriteLine("GetQueryAsHttpRequest" + ": " + actual);
    }

    [Fact]
    public void GetPageNameFromUriTest()
    {
        var actual = UH.GetPageNameFromUri(urio);//DebugLogger.Instance.WriteLine("GetPageNameFromUriTest" + ": " + actual);
    }

    [Fact]
    public void GetProtocolStringTest()
    {
        var actual = UH.GetProtocolString( urio);//DebugLogger.Instance.WriteLine("GetProtocolStringTest" + ": " + actual);
    }

    [Fact]
    public void GetFileNameTest()
    {
        var input = @"https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon";
        var actual = UH.GetFileName(input);

        var excepted = "Login.aspx";

        Assert.Equal(excepted, actual);
    }
}
