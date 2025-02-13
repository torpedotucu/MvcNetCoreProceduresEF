using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryDoctores
    {
        private DoctoresContext context;
        public RepositoryDoctores(DoctoresContext context)
        {
            this.context=context;
        }

        public async Task<List<string>> GetEspecialidadesAsync()
        {
            using(DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_ESPECIALIDAD";
                com.CommandType=System.Data.CommandType.StoredProcedure;
                com.CommandText=sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<string> especialidades = new List<string>();
                while(await reader.ReadAsync())
                {
                    string especialidad = reader["ESPECIALIDAD"].ToString();
                    especialidades.Add(especialidad);
                }
                
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return especialidades;
            }
        }

        public async Task UpdateSalarioOficioAsync(int incremento, string especialidad)
        {

        }
    }
}
