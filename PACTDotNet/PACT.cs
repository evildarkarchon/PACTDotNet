using System.Diagnostics;

namespace PACTDotNet
{
    public class PACT
    {
        public static bool IsMo2Running()
        {
            if (File.Exists(Globals.Info.MO2_PATH))
            {
                var mo2Procs = Process.GetProcesses()
                                      .Where(proc => proc.ProcessName.ToLower().Contains(Globals.Info.MO2_EXE.ToLower()));
                foreach (var proc in mo2Procs)
                {
                    if (proc.ProcessName.ToLower().Contains(Globals.Info.MO2_EXE.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static void EnsureMo2NotRunning()
        {
            if (IsMo2Running())
            {
                throw new InvalidOperationException("❌ ERROR : CANNOT START PACT WHILE MOD ORGANIZER 2 IS ALREADY RUNNING!\n"
                                                    + "PLEASE CLOSE MO2 AND RUN PACT AGAIN! (DO NOT RUN PACT THROUGH MO2)");
            }
        }
    }
}
