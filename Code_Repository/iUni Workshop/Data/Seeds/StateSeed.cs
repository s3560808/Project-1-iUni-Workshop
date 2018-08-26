using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models;
using iUni_Workshop.Models.StateModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

//namespace iUni_Workshop.Data.Seeds
//{
//    public class StateSeed
//    {
//        
//        public static void Initialize(IServiceProvider serviceProvider)
//        {
//            var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
//
//            if (!_context.States.Any())
//            {
//                var states = new List<State>
//                {
//
//                };
//                _context.AddRange(states);
//
//                _context.SaveChanges();
//            }
//
//            
//        }
//    }
//
//
//}