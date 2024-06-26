﻿
namespace NUV.Comics.Infra.Data.Repositories
{
    public class GlobalCentroDeCustoRepositorio : BaseRepositoryReadOnly<object>
    {

        private readonly Db2DbContext _db2Context;

        public GlobalCentroDeCustoRepositorio(Db2DbContext dbContext,
            IMapper mapper)
            : base(dbContext, mapper)
        {
            ownerDB = dbContext.ownerDB;
            var cnn = dbContext.Configuration.GetSectionValue("AppConfig:OwnerDB:Cnn");
            SetDbConnection(dbContext?.Database.GetDbConnection(), dbContext.Configuration.GetConnectionString(cnn));

            _db2Context = dbContext;
        }


        /// <summary>
        /// Exemplo de update em massa BulkUpdate
        /// </summary>
        /// <returns></returns>
        public async Task<int> ExemploUpdateMassa()
        {

            var result = await _db2Context.Embalagens
                .Where(x => x.EMB_CD_CTB != null)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.EMB_V_PREC_UNT, 0));

            return result;
        }

        // public async Task<IEnumerable<GlobalCentroDeCustoQueryResult>> ObterContaPorCodigo(string empresa, string centroCusto, DateTime vigencia)
        // {
        //     var vigenciaFinal = new DateTime(9999, 12, 01);

        //     var orgType = "S";

        //     var centro = centroCusto?.Trim().ToUpper().Replace(".", "");
        //     centro = centro?.Length > 5 ? centro[..5] : centro;

        //     var query = _dbSet.AsNoTracking();

        //     query = query.Where(x => x.FAC_CD == empresa &&
        //         x.FNCL_ORG_CD == centro &&
        //         x.FNCL_ORG_TYP == orgType &&
        //         x.EFF_DT <= vigencia && (x.OBS_DT >= vigencia || x.OBS_DT == vigenciaFinal))
        //         .OrderBy(o => o.FAC_CD)
        //             .ThenBy(o => o.FNCL_ORG_CD)
        //             .ThenBy(o => o.FNCL_ORG_TYP)
        //             .ThenBy(o => o.EFF_DT)
        //             .ThenBy(o => o.OBS_DT);

        //     var toList = await query.ToArrayAsync();

        //     var queryResult = _mapper.Map<IEnumerable<GlobalCentroDeCustoQueryResult>>(toList);
        //     return queryResult;
        // }

        // public async Task<IEnumerable<GlobalCentroDeCustoQueryResult>> ObterContaPorSecao(string empresa, string centroCusto)
        // {
        //     empresa = empresa?.Trim().ToUpper();

        //     var centro = centroCusto?.Trim().ToUpper().Replace(".", "");
        //     centro = centro?.Length > 5 ? centro[..5] : centro;

        //     var orgType = "S";
        //     var valdInd = "Y";
        //     var vigenciaFinal = new DateTime(9999, 12, 01);
        //     var orgCd = "0%";

        //     var query = _dbSet.AsNoTracking();

        //     query = query.Where(x => x.FAC_CD == empresa &&
        //         x.FNCL_ORG_TYP == orgType &&
        //         x.VALD_ACT_IND == valdInd &&
        //         x.OBS_DT == vigenciaFinal &&
        //         EF.Functions.Like(x.FNCL_ORG_CD, orgCd));

        //     if (!string.IsNullOrWhiteSpace(centro))
        //     {
        //         query = query.Where(x => x.FNCL_ORG_CD == centro);
        //     }

        //     query = query.OrderBy(o => o.FAC_CD)
        //         .ThenBy(o => o.FNCL_ORG_CD);

        //     var toList = await query.ToArrayAsync();

        //     var queryResult = _mapper.Map<IEnumerable<GlobalCentroDeCustoQueryResult>>(toList);
        //     return queryResult;
        // }

        // public async Task<IPagedList<GlobalCentroDeCustoQueryResult>> ObterTodos(GlobalCentroDeCustoPorEmpresaInputQuery parameter)
        // {
        //     var pagedResult = await GetPagedListAsync(
        //         predicate: x => x.LAST_UPDT_TS >= parameter.Vigencia &&
        //             x.FAC_CD == parameter.CodigoEmpresa,
        //         orderBy: x => x.OrderBy(o => o.FAC_CD)
        //             .ThenBy(o => o.FNCL_ORG_CD)
        //             .ThenBy(o => o.FNCL_ORG_TYP)
        //             .ThenBy(o => o.EFF_DT)
        //             .ThenBy(o => o.OBS_DT),
        //         pageIndex: parameter.PageIndex,
        //         pageSize: parameter.PageSize);

        //     var queryResult = _mapper.Map<PagedList<GlobalCentroDeCustoQueryResult>>(pagedResult);
        //     return queryResult;
        // }

        // public async Task<IEnumerable<GlobalCentroDeCustoQueryResult>> ObterUltimaAlteracao(string empresa, DateTime ultimaAlteracao, SkipTake skipTake)
        // {
        //     empresa = empresa?.Trim().ToUpper();

        //     var query = _dbSet.AsNoTracking();

        //     query = query.Where(x => x.FAC_CD == empresa &&
        //         x.LAST_UPDT_TS >= ultimaAlteracao)
        //         .OrderBy(o => o.FAC_CD)
        //             .ThenBy(o => o.FNCL_ORG_CD)
        //             .ThenBy(o => o.FNCL_ORG_TYP)
        //             .ThenBy(o => o.EFF_DT)
        //             .ThenBy(o => o.OBS_DT)
        //         .Skip(skipTake.Skip)
        //         .Take(skipTake.MinTake());

        //     var toList = await query.ToArrayAsync();

        //     var queryResult = _mapper.Map<IEnumerable<GlobalCentroDeCustoQueryResult>>(toList);
        //     return queryResult;
        // }

        // public async Task<IEnumerable<GlobalCentroDeCustoFacilityQueryResult>> ObterPorCodigoFacility(GlobalCentroDeCustoFacilityInputQuery parameter)
        // {
        //     var vigenciaFinal = new DateTime(9999, 12, 01);
        //     var orgType = "S";

        //     var query = _dbContext.Set<CentroDeCustoSumary>().AsNoTracking();

        //     query = query.Where(x => x.FNCL_ORG_TYP == orgType);

        //     if (!string.IsNullOrWhiteSpace(parameter.CodigoEmpresa))
        //     {
        //         query = query.Where(x => x.FAC_CD == parameter.CodigoEmpresa);
        //     }
        //     if (!string.IsNullOrWhiteSpace(parameter.CentroCusto))
        //     {
        //         query = query.Where(x => x.FNCL_ORG_CD == parameter.CentroCusto);
        //     }
        //     if (!string.IsNullOrWhiteSpace(parameter.Facility))
        //     {
        //         query = query.Where(x => x.TO_FAC_CD == parameter.Facility);
        //     }
        //     if (!parameter.Vigencia.Equals(DateTime.MinValue))
        //     {
        //         query = query.Where(x => x.EFF_DT <= parameter.Vigencia && (x.OBS_DT >= parameter.Vigencia || x.OBS_DT == vigenciaFinal));
        //     }

        //     query = query.OrderBy(o => o.FAC_CD)
        //             .ThenBy(o => o.FNCL_ORG_CD)
        //             .ThenBy(o => o.FNCL_ORG_TYP)
        //             .ThenBy(o => o.EFF_DT)
        //             .ThenBy(o => o.OBS_DT)
        //         .Skip(parameter.Skip)
        //         .Take(parameter.MinTake());

        //     var toList = await query.ToArrayAsync();

        //     var queryResult = _mapper.Map<IEnumerable<GlobalCentroDeCustoFacilityQueryResult>>(toList);
        //     return queryResult;
        // }
    }
}