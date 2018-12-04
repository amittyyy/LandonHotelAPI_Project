using LandonAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandonAPI
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            await addTestData(services.GetRequiredService<HotelApiDbContext>());
        }

        public static async Task addTestData(HotelApiDbContext context)
        {
            if(context.Rooms.Any())
            {
                //already has data;
                return;

            }

            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("E9089998-ACA7-4123-94DE-5FEFDC0C9837"),
                Name = "King Suite",
                Rate = 1020
            });
            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("105D31B8-3D4E-4C03-8B06-5FD8C960F445"),
                Name = "Queen Suite",
                Rate = 2111
            });

            await context.SaveChangesAsync();
        }
    }
}
