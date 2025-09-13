using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.DTOInterfaces
{
    public interface ILotUpdateRequest
    {
        string Title { get; set; }
        string Description { get; set; }
        decimal StartingPrice { get; set; }
        decimal BidIncrement { get; set; }
        DateTime EndDate { get; set; }
        string? ImageUrl { get; set; }

    }
}
