namespace SunamoUri;


internal class UriShortConsts
{
    internal const string DevCz = "dev.sunamo.net";
    internal const string AppCz = "app.sunamo.net";
    internal const string GeoCz = "geo.sunamo.net";
    internal const string ErtCz = "var.sunamo.net";
    internal const string ShoCz = "sho.sunamo.net";
    internal const string RpsCz = "rps.sunamo.net";
    internal const string PhsCz = "phs.sunamo.net";
    internal const string HtpCz = "htp.sunamo.net";
    internal const string LyrCz = "lyr.sunamo.net";
    // miss acs
    internal static List<string> All = CAGConsts.ToList(DevCz, LyrCz, AppCz, GeoCz, ErtCz, RpsCz, ShoCz, PhsCz);
}