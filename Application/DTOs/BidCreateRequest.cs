using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.DTOInterfaces;

namespace Application.DTOs
{
    public class BidCreateRequest : IBidCreateRequest
    {
        public decimal Amount { get; set; }
    }
}