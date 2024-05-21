namespace SunamoUri;


public class UriShortConsts
{
    public const string DevCz = "dev.sunamo.net";
    public const string AppCz = "app.sunamo.net";
    public const string GeoCz = "geo.sunamo.net";
    public const string ErtCz = "var.sunamo.net";
    public const string ShoCz = "sho.sunamo.net";
    public const string RpsCz = "rps.sunamo.net";
    public const string PhsCz = "phs.sunamo.net";
    public const string HtpCz = "htp.sunamo.net";
    public const string LyrCz = "lyr.sunamo.net";
    // miss acs
    public static List<string> All = CAGConsts.ToList(DevCz, LyrCz, AppCz, GeoCz, ErtCz, RpsCz, ShoCz, PhsCz);
}