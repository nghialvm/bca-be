using dvd.bca.Offers.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.Offers
{
    public interface IOfferAppService :
        ICrudAppService<
            OfferDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateOfferDto,
            UpdateOfferDto>
    {
    }
}
