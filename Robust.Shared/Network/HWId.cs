using System;
using System.Security.Cryptography;
using Microsoft.Win32;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.IoC;

namespace Robust.Shared.Network
{
    internal static class HWId
    {
        public const int LengthHwid = 32;

        public static byte[] Calc(IConfigurationManagerInternal configMan)
        {
            if (OperatingSystem.IsWindows())
            {
                var regKey = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Space Wizards\Robust", "Hwid", null);
                if (regKey is byte[] {Length: LengthHwid} bytes)
                {
                    var archived = configMan.GetCVar(CVars.LastKnownHwid);
                    if (archived.Length == 0)
                    {
                        configMan.SetCVar(CVars.LastKnownHwid, Convert.ToBase64String(bytes));
                        configMan.SaveToFile();
                        return bytes;
                    }

                    return archived != Convert.ToBase64String(bytes) ? Convert.FromBase64String(archived) : bytes;
                }
                var newId = new byte[LengthHwid];
                RandomNumberGenerator.Fill(newId);
                var existingHwid = configMan.GetCVar(CVars.LastKnownHwid);
                if (existingHwid.Length != 0)
                {
                    newId = Convert.FromBase64String(existingHwid);
                }

                Registry.SetValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Space Wizards\Robust",
                    "Hwid",
                    newId,
                    RegistryValueKind.Binary);

                configMan.SetCVar(CVars.LastKnownHwid, Convert.ToBase64String(newId));
                configMan.SaveToFile();

                return newId;
            }

            return Array.Empty<byte>();
        }
    }

#if DEBUG
    internal sealed class HwidCommand : IConsoleCommand
    {
        public string Command => "hwid";
        public string Description => "Returns the current HWID.";
        public string Help => "Returns the current HWID.";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            shell.WriteLine(Convert.ToBase64String(HWId.Calc(), Base64FormattingOptions.None));
        }
    }
#endif
}
