namespace DinoBlazorApp.Models;

public class FeedbackForm
{
    public string Nickname { get; set; } = "";
    public string Type     { get; set; } = "general";
    public int    Rating   { get; set; } = 0;
    public string Message  { get; set; } = "";
}

public class FeedbackEntry
{
    public string   Nickname { get; set; } = "";
    public string   Type     { get; set; } = "general";
    public int      Rating   { get; set; } = 0;
    public string   Message  { get; set; } = "";
    public DateTime At       { get; set; }
}

public class IdeaForm
{
    public string Title    { get; set; } = "";
    public string Category { get; set; } = "feature";
    public string Desc     { get; set; } = "";
}

public class WishlistItem
{
    public int    Id       { get; set; }
    public string Title    { get; set; } = "";
    public string Category { get; set; } = "";
    public string Desc     { get; set; } = "";
    public int    Votes    { get; set; } = 0;
    public bool   Voted    { get; set; } = false;
    public bool   IsNew    { get; set; } = false;
}

public class FeedbackDataStore
{
    public List<FeedbackEntry> RecentFeedback { get; set; } = new();
    public List<WishlistItem> WishlistItems { get; set; } = new();
}
