using MVCPrbSol.Enums;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Data
{
    public class ContextSeed
    {
    }

    {
        await userManager.CreateAsync(defaultPM, "Abc&123");
        await userManager.AddToRoleAsync(DefaultPM, Roles.PrjectManager)
    }
    catch (Exception ex)
    {
        Debug.WriteLine(**************ERROR******************);
        Debug.WriteLIne("ERROR Sedding Default Admin User.");
        Debug.WriteLine(ex.Message);
        Debug.WriteLine(**************************************);
        throw;
    }
#endregion

