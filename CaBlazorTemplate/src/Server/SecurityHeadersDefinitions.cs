using Microsoft.AspNetCore.Builder;
using CaBlazorTemplate.Server.Options;

namespace CaBlazorTemplate.Server;

public static class SecurityHeadersDefinitions
{
    public static HeaderPolicyCollection GetHeaderPolicyCollection(bool isDev, ContentHeaderPolicyOptions csp)
    {
        var policy = new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddXssProtectionBlock()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .AddCrossOriginOpenerPolicy(builder => builder.SameOrigin())
            .AddCrossOriginResourcePolicy(builder => builder.SameOrigin())
            .AddCrossOriginEmbedderPolicy(builder => builder.RequireCorp()) // remove for dev if using hot reload
            .AddContentSecurityPolicy(builder =>
            {
                builder.AddObjectSrc().None();
                builder.AddBlockAllMixedContent();
                builder.AddImgSrc().Self().From("data:");
                builder.AddFormAction().Self().From(csp.FormAction.AzureB2CInstance);
                builder.AddFontSrc().Self();
                if (!string.IsNullOrEmpty(csp.StyleSrc?.StyleCdn))
                    builder.AddStyleSrc().Self().UnsafeInline().From(csp.StyleSrc.StyleCdn);
                else
                    builder.AddStyleSrc().Self().UnsafeInline();
                builder.AddBaseUri().Self();
                builder.AddFrameAncestors().None();

                // due to Blazor
                if (!string.IsNullOrEmpty(csp.ScriptSrc?.ScriptCdn))
                {
                    builder.AddScriptSrc()
                    .From(csp.ScriptSrc.ScriptCdn)
                    .Self()
                    .WithHash256("v8v3RKRPmN4odZ1CWM5gw80QKPCCWMcpNeOmimNL2AA=")
                    .UnsafeEval();
                }
                else
                {
                    builder.AddScriptSrc()
                    .Self()
                    .WithHash256("v8v3RKRPmN4odZ1CWM5gw80QKPCCWMcpNeOmimNL2AA=")
                    .UnsafeEval();
                }
                // disable script and style CSP protection if using Blazor hot reload
                // if using hot reload, DO NOT deploy with an insecure CSP
            })
            .RemoveServerHeader()
            .AddPermissionsPolicy(builder =>
            {
                builder.AddAccelerometer().None();
                builder.AddAutoplay().None();
                builder.AddCamera().None();
                builder.AddEncryptedMedia().None();
                builder.AddFullscreen().All();
                builder.AddGeolocation().None();
                builder.AddGyroscope().None();
                builder.AddMagnetometer().None();
                builder.AddMicrophone().None();
                builder.AddMidi().None();
                builder.AddPayment().None();
                builder.AddPictureInPicture().None();
                builder.AddSyncXHR().None();
                builder.AddUsb().None();
            });

        if (!isDev)
        {
            // maxage = one year in seconds
            policy.AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365);
        }

        return policy;
    }
}
