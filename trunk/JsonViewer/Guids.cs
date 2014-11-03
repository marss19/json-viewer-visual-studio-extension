// Guids.cs
// MUST match guids.h
using System;

namespace Marss.JsonViewer
{
    static class GuidList
    {
        public const string guidJsonViewerPkgString = "c21a2a7b-7b1c-45c9-bee3-3bdc0715bdc9";
        public const string guidJsonViewerCmdSetString = "faf211b8-a86a-49a0-a769-e15f98292c0a";
        public const string guidToolWindowPersistanceString = "c04033dc-fa25-4ece-85d3-8ed65d600602";

        public static readonly Guid guidJsonViewerCmdSet = new Guid(guidJsonViewerCmdSetString);
    };
}