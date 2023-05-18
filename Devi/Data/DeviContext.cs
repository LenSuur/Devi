using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Devi.Data;

namespace Devi.Data
{
    [ExcludeFromCodeCoverage]
    public class DeviContext : DbContext
    {
        public DeviContext (DbContextOptions<DeviContext> options)
            : base(options)
        {
        }

        public DbSet<Devi.Data.Device> Device { get; set; } = default!;
    }
}
