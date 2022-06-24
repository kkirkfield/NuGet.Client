// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Events
{
    public static class ProtocolDiagnostics
    {
        public delegate void ProtocolDiagnosticHttpEventHandler(ProtocolDiagnosticHttpEvent pdEvent);

        public static event ProtocolDiagnosticHttpEventHandler HttpEvent;

        public delegate void ProtocolDiagnosticResourceEventHandler(ProtocolDiagnosticResourceEvent pdrEvent);

        public static event ProtocolDiagnosticResourceEventHandler ResourceEvent;

        public delegate void ProtocolDiagnosticsNupkgCopiedEventHandler(ProtocolDiagnosticNupkgCopiedEvent ncEvent);

        public static event ProtocolDiagnosticsNupkgCopiedEventHandler NupkgCopiedEvent;

        public delegate void HttpCacheHitEventHandler();

        public static event HttpCacheHitEventHandler HttpCacheHitEvent;

        public delegate void HttpCacheMissEventHandler(bool cacheByPass, bool isExpired);

        public static event HttpCacheMissEventHandler HttpCacheMissEvent;

        internal static void RaiseEvent(ProtocolDiagnosticHttpEvent pdEvent)
        {
            HttpEvent?.Invoke(pdEvent);
        }

        internal static void RaiseEvent(ProtocolDiagnosticResourceEvent pdrEvent)
        {
            ResourceEvent?.Invoke(pdrEvent);
        }

        internal static void RaiseEvent(ProtocolDiagnosticNupkgCopiedEvent ncEvent)
        {
            NupkgCopiedEvent?.Invoke(ncEvent);
        }

        internal static void RaiseHttpCacheHitEvent()
        {
            HttpCacheHitEvent?.Invoke();
        }

        internal static void RaiseHttpMissCacheEvent(bool cacheBypass, bool isExpired)
        {
            HttpCacheMissEvent?.Invoke(cacheBypass, isExpired);
        }
    }
}
