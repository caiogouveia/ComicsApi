﻿using AutoMapper;
using NUV.Cep.Infra.Data.Commands;
using NUV.Cep.Infra.Data.DomainModel;
using Nuuvify.CommonPack.UnitOfWork.Abstraction.Collections;

namespace NUV.Cep.Infra.IoC.Dto
{
    public class CentroDeCustoProfile : Profile
    {
        public CentroDeCustoProfile()
        {
            CreateMap<CentroDeCustoSumary, GlobalCentroDeCustoFacilityQueryResult>();
            CreateMap<GlobalCentroDeCusto, GlobalCentroDeCustoQueryResult>();

            CreateMap<PagedList<GlobalCentroDeCusto>, PagedList<GlobalCentroDeCustoQueryResult>>();
        }
    }
}