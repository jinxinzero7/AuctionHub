using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces.DTOInterfaces
{
    public interface IBidCreateRequest
    {
        public decimal Amount { get; set; }
    }
}