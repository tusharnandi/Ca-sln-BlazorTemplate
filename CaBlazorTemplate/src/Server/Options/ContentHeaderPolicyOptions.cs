namespace CaBlazorTemplate.Server.Options;

public class ContentHeaderPolicyOptions
{
    public const string ContentHeaderPolicy = "ContentHeaderPolicy";

    public FormActionList FormAction { get; set; }
    public StyleSrcList StyleSrc { get; set; }
    public ScriptSrcList ScriptSrc { get; set; }

    public class FormActionList
    {
        public string AzureB2CInstance { get; set; }
    }

    public class StyleSrcList
    {
        public string StyleCdn { get; set; }
    }

    public class ScriptSrcList
    {
        public string ScriptCdn { get; set; }
    }

}
