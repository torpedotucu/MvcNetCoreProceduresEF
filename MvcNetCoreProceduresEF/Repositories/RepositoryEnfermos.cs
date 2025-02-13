using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using System.Data.Common;

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryEnfermos
    {
        private EnfermosContext context;
        public RepositoryEnfermos(EnfermosContext context)
        {
            this.context=context;
        }
        public async Task<List<Enfermo>> GetEnfermosAsync()
        {
            //PARA CONSULTAS DE SELECCION DEBEMOS MAPEAR MANUALMENTE LOS DATOS
            using(DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_ENFERMOS";
                com.CommandType=System.Data.CommandType.StoredProcedure;
                com.CommandText=sql;
                await com.Connection.OpenAsync();
                //EJECUTAMOS EL READER
                DbDataReader reader =await com.ExecuteReaderAsync();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (await reader.ReadAsync())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion=reader["INSCRIPCION"].ToString(),
                        Apellido=reader["APELLIDO"].ToString(),
                        Direccion=reader["DIRECCION"].ToString(),
                        FechaNacimiento=DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero=reader["S"].ToString()
                    };
                    enfermos.Add(enfermo);

                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
        }

        public async Task<Enfermo> FindEnfermo(string inscripcion)
        {
            /*
             * PARA LLAMAR A PROCEDIMIENTOS ALMACENADOS CON PARAMETROS 
             * LA LLAMADA SE REALIZA MEDIANTE EL NOMBRE DEL PROCEDIMIENTO Y CADA PARAMETRO 
             * A CONTINUACION SEPARADO MEDIANTE COMAS
             */
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            //DEBEMOS CREAR LOS PARAMETROS
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            /*
             * SI LOS DATOS QUE DEVUELVE EL PROCEDMIENTO ESTAN MAPEADOS
             * PODEMOS UTILIZAR EL METODO FromSqlRaw CON LINQ
             * CUANDO UTILIZAMOS LINQ CON PROCEDIMIENTOS ALMACENADOS
             * AL CONSULTA Y LA EXTRACCION DE DATOS SE REALIZAN EN DOS PASOS.
             * NO SE PUEDE HACER LINQ Y EXTRAER A LA VEZ
             */
            var consulta = await this.context.Enfermos.FromSqlRaw(sql, pamInscripcion).ToListAsync();
            //PARA EXTRAER LOS DATOS SE UTILIZA EL METODO AsEnumberable()
            Enfermo enfermo = consulta.AsEnumerable().FirstOrDefault();
            return enfermo;
        }

        public async Task DeleteEnfermo(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            using(DbCommand com=
                this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType=System.Data.CommandType.StoredProcedure;
                com.CommandText=sql;
                await com.Connection.OpenAsync();
                await com.ExecuteNonQueryAsync();
                await com.Connection.CloseAsync();
                com.Parameters.Clear();

            }
        }
        public async Task DeleteEnfermoRaw(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamInscripcion);
        }
        public async Task InsertEnfermoAsync( string apellido, string direccion, DateTime fechaNacimiento, string genero )
        {
            string sql = "SP_INSERT_ENFERMO @apellido,@direccion, @fecha, @genero";
            
            SqlParameter pamApellido =
                new SqlParameter("@apellido", apellido);
            SqlParameter pamDireccion =
                new SqlParameter("@direccion", direccion);
            SqlParameter pamFecha =
                new SqlParameter("@fechanac", fechaNacimiento);
            SqlParameter pamGen =
                new SqlParameter("@genero", genero);
            await this.context.Database
                .ExecuteSqlRawAsync(sql, pamApellido, pamDireccion
                , pamFecha, pamGen);
        }
    }
}
