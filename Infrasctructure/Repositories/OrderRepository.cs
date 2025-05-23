using Domain.Entities;
using Domain.Interfaces;
using Infrasctructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasctructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TesteAmbevContext _context;

        public OrderRepository(TesteAmbevContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order entity)
        {
            var newOrder = await _context.Orders.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return newOrder.Entity;
        }

        public async Task<bool> GetProcessed(string id)
        {
            return await _context.Orders.AnyAsync(p => p.ExternalId == id && p.Status == OrderStatus.Processed);
        }
    }
}
