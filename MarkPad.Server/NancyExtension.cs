// <copyright file="NancyExtension.cs" company="AMPLITUDE Studios">Copyright AMPLITUDE Studios. All rights reserved.</copyright>

namespace MarkPad.Server
{
    public static class NancyExtension
    {
        public static bool IsAuthenticated(this Nancy.NancyContext context)
        {
            return context.CurrentUser != null && !string.IsNullOrEmpty(context.CurrentUser.Identity.Name);
        }
    }
}
