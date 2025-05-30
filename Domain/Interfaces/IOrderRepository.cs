﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order entity);
        Task<bool> GetProcessed(string id,string requestor);
    }
}
