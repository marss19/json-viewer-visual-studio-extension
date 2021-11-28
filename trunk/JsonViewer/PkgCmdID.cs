// PkgCmdID.cs
// MUST match PkgCmdID.h
using System;

namespace Marss.JsonViewer
{
    static class PkgCmdIDList
    {
        public const uint cmdidPasteJsonFromClipboard = 0x100;
        public const uint cmdidOpenEmptyJsonFile = 0x101;
        public const uint cmdidCompareJsonData = 0x102;
        public const uint cmdidEvaluateJSONPath = 0x103;
        public const uint cmdidSendFeedback = 0x104;
    };
}