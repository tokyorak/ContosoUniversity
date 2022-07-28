namespace CUniversity.Utility;

#if true || SQLiteVersion

public static class Utils
{
    public static string GetLastChars(Guid token)
    {
        return token.ToString().Substring(token.ToString().Length - 3);
    }
}

#else

public static class Utils
{
    public static string GetLastChars(byte[] token)
    {
        return token[7].ToString();
    }
}

#endif