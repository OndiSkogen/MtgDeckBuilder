#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MtGDeckBuilder.Models;

namespace MtGDeckBuilder.Data
{
    public class MtGDeckBuilderContext : DbContext
    {
        public MtGDeckBuilderContext (DbContextOptions<MtGDeckBuilderContext> options)
            : base(options)
        {
        }

        public DbSet<MtGDeckBuilder.Models.Movie> Movie { get; set; }

        public DbSet<MtGDeckBuilder.Models.Card> Card { get; set; }

        public DbSet<MtGDeckBuilder.Models.Deck> Deck { get; set; }
    }
}
